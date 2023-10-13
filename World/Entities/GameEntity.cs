using Godot;
using SpookyBotanyGame.Core;

namespace SpookyBotanyGame.World.Entities
{
    [GlobalClass]
    public partial class GameEntity : Node2D
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
        
    }
}