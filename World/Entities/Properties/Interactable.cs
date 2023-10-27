using Godot;
namespace SpookyBotanyGame.World.Entities.Properties
{
    [GlobalClass,Icon("res://World/Entities/icon-entity-property.svg")]
    public partial class Interactable : EntityProperty
    {
        public delegate bool InteractionTriggered(GameEntity other, GameEntity self);
        [Export] private InteractionHandler Handler { get; set; }

        private bool _isTargeted;
        public event InteractionTriggered OnInteractionTriggered;
        
        public override void _Ready()
        {
            base._Ready();
            _entity?.AddProperty<Interactable>(this);
            OnEnabledUpdate += HandleEnabledUpdate;
        }

        private void HandleEnabledUpdate(bool isEnabled)
        {
            UpdateTargeted();
        }

        public void SetTargeted(bool isTargeted)
        {
            _isTargeted = isTargeted;
            UpdateTargeted();
        }

        private void UpdateTargeted()
        {
            if (_isTargeted && IsEnabled)
            {
                Handler.OutlineShaderShow();
            }
            else
            {
                Handler.OutlineShaderHide();
            }
        }

        public bool DoInteraction(GameEntity interactingEntity)
        {
            if (!IsEnabled)
            {
                return false;
            }
            if (OnInteractionTriggered == null)
            {
                return false;
            }
            bool interactionResult = OnInteractionTriggered.Invoke(interactingEntity, _entity);
            return interactionResult;
        }
    }
}