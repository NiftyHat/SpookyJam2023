using Godot;
using SpookyBotanyGame.Core.StateMachines;

namespace SpookyBotanyGame.World.Entities.Plants.States.DarkEye
{
    public class IdleState : State
    {
        protected readonly DarkPlantEye _plant;

        public IdleState(DarkPlantEye plantEye)
        {
            _plant = plantEye;
            if (_plant.LookDetectionZone != null && _plant.LookAnimation != null)
            {
                var playerDetector = new EntityDetectionZone.Detector<PlayerEntity>();
                playerDetector.OnOverlap = HandlePlayerInLookZone;
                playerDetector.OnExit = HandlePlayerExitLookZone;
                _plant.LookDetectionZone.Set(playerDetector);
                _plant.LookAnimation.SetEnabled(true);
            }
            _plant.Interactable.SetEnabled(true);
            _plant.Interactable.OnInteractionTriggered += HandleInteractionTriggered;
        }

        protected override void Exit(State state = null)
        {
            _plant.Interactable.OnInteractionTriggered += HandleInteractionTriggered;
            base.Exit(state);
        }

        private bool HandleInteractionTriggered(GameEntity other, GameEntity self)
        {
            if (self == null)
            {
                GD.PushError($"Failed ${nameof(HandleInteractionTriggered)} on '{_plant.Name}' sending entity was null");
                return false;
            }

            if (self != _plant)
            {
                GD.PushError(
                    $"Failed ${nameof(HandleInteractionTriggered)} on '{_plant.Name}' as sending entity {self.Name} didn't match plant");
                return false;
            }

            if (other == null)
            {
                GD.PushError($"Failed ${nameof(HandleInteractionTriggered)} on '{_plant.Name}' as other entity was null");
                return false;
            }
        
            if (other is PlayerEntity playerEntity && playerEntity.CarriedSlot != null && !playerEntity.CarriedSlot.IsFull)
            {
                playerEntity.CarriedSlot.Add(_plant.Output);
                _plant.Interactable.SetEnabled(false);
                _plant.LookAnimation.SetEnabled(false);
                Exit(new HarvestState(_plant));
            }
            return false;
        }

        private void HandlePlayerExitLookZone(PlayerEntity player)
        {
            _plant.LookAnimation.ClearTarget();
        }

        private void HandlePlayerInLookZone(PlayerEntity player)
        {
            _plant.LookAnimation.SetTarget(player.GlobalPosition);
        }
    }
}