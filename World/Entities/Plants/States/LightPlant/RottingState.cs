using SpookyBotanyGame.Core.StateMachines;

namespace SpookyBotanyGame.World.Entities.Plants.States
{
    public class RottingState  : State
    {
        protected Plants.LightPlant _plant;
        
        public RottingState(Plants.LightPlant plant)
        {
            _plant = plant;
            _plant.Sim.OnDayTick += HandleDayAdvance;
            _plant?.Animation.Play("Rotting");
            _plant?.Effects.SetIsLit(false);
        }

        protected override void Exit(State state = null)
        {
            _plant.Sim.OnDayTick -= HandleDayAdvance;
            base.Exit(state);
        }

        private void HandleDayAdvance(int amount)
        {
            _plant.Destroy();
        }
    }
}

