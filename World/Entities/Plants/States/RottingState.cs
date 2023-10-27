using SpookyBotanyGame.Core.StateMachines;

namespace SpookyBotanyGame.World.Entities.Plants.States
{
    public class RottingState  : State
    {
        protected LightPlant _plant;
        
        public RottingState(LightPlant plant)
        {
            _plant = plant;
            _plant.Sim.OnDayTick += HandleDayAdvance;
            _plant.Animation.Play("Rotting");
        }

        private void HandleDayAdvance(int amount)
        {
            _plant.Destroy();
        }
    }
}

