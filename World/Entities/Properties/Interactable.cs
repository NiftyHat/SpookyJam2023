using System;
using Godot;
namespace SpookyBotanyGame.World.Entities.Properties
{
    [GlobalClass,Icon("res://World/Entities/icon-entity-property.svg")]
    public partial class Interactable : EntityProperty
    {
        public delegate bool InteractionTriggered(GameEntity other, GameEntity self);
        [Export] private InteractionHandler Handler { get; set; }

        private GameEntity _targetingEntity;
        public event InteractionTriggered OnInteractionTriggered;
        
        public Func<GameEntity, bool> CanInteractFilter;
        
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

        public void SetTargeted(GameEntity entity)
        {
            _targetingEntity = entity;
            UpdateTargeted();
        }

        private void UpdateTargeted()
        {
            if (IsEnabled && _targetingEntity != null && !IsInteractionFiltered(_targetingEntity))
            {
                Handler.OutlineShaderShow();
            }
            else
            {
                Handler.OutlineShaderHide();
            }
        }

        public bool IsInteractionFiltered(GameEntity gameEntity)
        {
            if (CanInteractFilter == null)
            {
                return false;
            }
            return !CanInteractFilter(gameEntity);
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
            if (IsInteractionFiltered(interactingEntity))
            {
                return false;
            }
            bool interactionResult = OnInteractionTriggered.Invoke(interactingEntity, _entity);
            return interactionResult;
        }
    }
}