using Godot;

namespace SpookyBotanyGame.World.Entities
{
    
    public partial class EntityProperty : Node2D
    {
        protected GameEntity _entity;
        
        [Export]
        protected bool _isEnabled {
            get;
            set;
        }

        public override void _Ready()
        {
            _entity = GetParent<GameEntity>();
            base._Ready();
        }
    }
}