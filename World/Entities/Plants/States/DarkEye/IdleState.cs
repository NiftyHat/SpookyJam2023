using Godot;
using SpookyBotanyGame.Core.StateMachines;

namespace SpookyBotanyGame.World.Entities.Plants.States.DarkEye
{
    public class IdleState : State<DarkPlantEye>, IUpdatableState
    {
        private bool _isDamagedByLight;

        public IdleState(DarkPlantEye plantEye) : base(plantEye)
        {
            if (_owner.LookDetectionZone != null && _owner.LookAnimation != null)
            {
                var playerDetector = new EntityDetectionZone.Detector<PlayerEntity>();
                playerDetector.OnOverlap = HandlePlayerInLookZone;
                playerDetector.OnExit = HandlePlayerExitLookZone;
                _owner.LookDetectionZone.Set(playerDetector);
                _owner.LookAnimation.SetEnabled(true);
            }
            _owner.LightSensor.OnApply += HandleLightApply;
            _owner.Interactable.SetEnabled(true);
            _owner.Interactable.OnInteractionTriggered += HandleInteractionTriggered;
        }

        protected override void Exit(State state = null)
        {
            _owner.LookAnimation.SetEnabled(false);
            _owner.Interactable.SetEnabled(false);
            _owner.LightSensor.OnApply -= HandleLightApply;
            _owner.Interactable.OnInteractionTriggered -= HandleInteractionTriggered;
            base.Exit(state);
        }

        private bool HandleInteractionTriggered(GameEntity other, GameEntity self)
        {
            if (self == null)
            {
                GD.PushError($"Failed ${nameof(HandleInteractionTriggered)} on '{_owner.Name}' sending entity was null");
                return false;
            }

            if (self != _owner)
            {
                GD.PushError(
                    $"Failed ${nameof(HandleInteractionTriggered)} on '{_owner.Name}' as sending entity {self.Name} didn't match plant");
                return false;
            }

            if (other == null)
            {
                GD.PushError($"Failed ${nameof(HandleInteractionTriggered)} on '{_owner.Name}' as other entity was null");
                return false;
            }
        
            if (other is PlayerEntity playerEntity && playerEntity.CarriedEyeSlot != null && !playerEntity.CarriedEyeSlot.IsFull)
            {
                playerEntity.CarriedEyeSlot.Add(_owner.Output);
                _owner.Interactable.SetEnabled(false);
                _owner.LookAnimation.SetEnabled(false);
                Exit(new HarvestState(_owner));
            }
            return false;
        }
        
        public void Process(double delta)
        {
            if (_isDamagedByLight)
            {
                if (_owner.Health.Value > 0)
                {
                    Exit(new HurtState(_owner, 1));
                }
                else
                {
                    Exit(new DestroyState(_owner));
                }
            }
        }

        private void HandlePlayerExitLookZone(PlayerEntity player)
        {
            _owner.LookAnimation.ClearTarget();
        }

        private void HandlePlayerInLookZone(PlayerEntity player)
        {
            _owner.LookAnimation.SetTarget(player.GlobalPosition);
        }
        
        private void HandleLightApply(LightEmissionZone zone, float lightPower)
        {
            if (_isDamagedByLight)
            {
                GD.Print("_isDamagedByLight");
                return;
            }
            if (lightPower >= 0.5f)
            {
                _isDamagedByLight = true;
            }
            else
            {
                _isDamagedByLight = false;
            }
        }
    }
}