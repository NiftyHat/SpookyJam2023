using Godot;
using SpookyBotanyGame.Core;

namespace SpookyBotanyGame.World.Entities
{
    [GlobalClass]
    public partial class GameEntity : Node2D, IEntityProvider
    {
        protected readonly TypedStorage<EntityProperty> _properties = new TypedStorage<EntityProperty>();
        public TProp GetProperty<TProp>() where TProp : EntityProperty
        {
            return _properties.Get<TProp>();
        }

        public bool TryGetProperty<TProp>(out TProp property) where TProp : EntityProperty
        {
            return _properties.TryGet<TProp>(out property);
        }
        
        public static bool TryGetProperty<TProp>(Node2D node2D, out TProp property, out GameEntity gameEntity) where TProp : EntityProperty
        {
            bool isInGroup = node2D.IsInGroup("EntityProvider");
            bool hasMethod = node2D.HasMethod("GetEntity");
            if (isInGroup && hasMethod)
            {
                gameEntity = node2D.Call("GetEntity").As<GameEntity>();
                if (gameEntity != null)
                {
                    return gameEntity.TryGetProperty<TProp>(out property);
                }
            }
            gameEntity = null;
            property = null;
            return false;
        }
        
        public static bool TryGetProperty<TProperty>(Node2D node2D, out TProperty property) where TProperty : EntityProperty
        {
            if (node2D.IsInGroup("EntityProvider") && node2D.HasMethod("GetEntity"))
            {
                var gameEntity = node2D.Call("GetEntity").As<GameEntity>();
                if (gameEntity != null)
                {
                    return gameEntity.TryGetProperty(out property);
                }
            }
            property = null;
            return false;
        }

        public GameEntity GetEntity()
        {
            return this;
        }

        public void AddProperty<TPropType>(TPropType property) where TPropType : EntityProperty
        {
            _properties.Add<TPropType>(property);
        }
    }
}