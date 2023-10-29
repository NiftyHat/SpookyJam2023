using SpookyBotanyGame.Core.StateMachines;

namespace SpookyBotanyGame.World.Entities.Plants.States.DarkPlant
{
    public class WaitForTargetState : State, IUpdatableState
    {
        private float _minLightToAttack = 0.1f;
        private float _aggroResetTimer = 0;
        private DarkPlantTentacle _tentacle;
    
        public WaitForTargetState(DarkPlantTentacle tentacle)
        {
            _tentacle = tentacle;
            _tentacle.LightSensor.OnApply += OnLightApply;
        }

        protected override void Exit(State state = null)
        {
            base.Exit(state);
            _tentacle.LightSensor.OnApply -= OnLightApply;
        }

        private void OnLightApply(LightEmissionZone lightEmissionZone, float lightPower)
        {
            if (lightPower > _minLightToAttack || _aggroResetTimer > 1.0f)
            {
                if (AttackSwipeState.CanTarget(_tentacle, lightEmissionZone))
                {
                    Exit(new AttackSwipeState(_tentacle, lightEmissionZone));
                }
                else if (AttackSideState.CanTarget(_tentacle, lightEmissionZone))
                {
                    Exit(new AttackSideState(_tentacle, lightEmissionZone));
                }
            }
            _aggroResetTimer = 1.0f;
        }

        public void Process(double delta)
        {
            if (_aggroResetTimer > 0)
            {
                _aggroResetTimer -= (float)delta;
                if (_aggroResetTimer <= 0)
                {
                    _aggroResetTimer = 0;
                }
            }
        }
    }
}