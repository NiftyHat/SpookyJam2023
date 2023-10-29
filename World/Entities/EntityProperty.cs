using Godot;

namespace SpookyBotanyGame.World.Entities
{
    [GlobalClass,Icon("res://World/Entities/icon-entity-property.svg")]
    public partial class EntityProperty : Node2D
    {
        public delegate void EnabledUpdated(bool enabledState);
        protected GameEntity _entity;
        public event EnabledUpdated OnEnabledUpdate;

        [Export]
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetEnabled(value);
        }

        private bool _isEnabled = true;

        public void SetEnabled(bool isEnabled)
        {
            if (_isEnabled != isEnabled)
            {
                _isEnabled = isEnabled;
                OnEnabledUpdate?.Invoke(isEnabled);
            }
        }

        public override void _Ready()
        {
            _entity = GetParent<GameEntity>();
            GD.Print(_entity);
            base._Ready();
        }
    }
}