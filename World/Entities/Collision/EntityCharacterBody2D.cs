using System;
using Godot;

namespace SpookyBotanyGame.World.Entities.Collision
{
    [GlobalClass, Icon("res://World/Entities/icon-game-entity.svg")]
    public partial class EntityCharacterBody2D : CharacterBody2D, IEntityProvider
    {
        [Export]
        private GameEntity Entity
        {
            get;
            set;
        }

        public override void _Ready()
        {
            if (Entity == null)
            {
                Entity = GetFirstEntityChild();
            }
            base._Ready();
        }

        private GameEntity GetFirstEntityChild()
        {
            int childCount = GetChildCount(true);
            for (int i = 0; i < childCount; i++)
            {
                GameEntity child = GetChildOrNull<GameEntity>(i, true);
                if (child != null)
                {
                    return child;
                }
            }
            return null;
        }
        
        private TEntityType GetFirstEntityChild<TEntityType>(Func<TEntityType, bool> predicate = null) where TEntityType : Node2D
        {
            int childCount = GetChildCount(true);
            bool usePredicate = predicate != null;
            for (int i = 0; i < childCount; i++)
            {
                TEntityType child = GetChildOrNull<TEntityType>(i, true);
                if (usePredicate)
                {
                    if (child != null && predicate(child))
                    {
                        return child;
                    }
                }
                else
                {
                    if (child != null)
                    {
                        return child;
                    }
                }
            }
            return null;
        }
        

        public GameEntity GetEntity()
        {
            return Entity;
        }
    }
}