using SpookyBotanyGame.Core.StateMachines;

namespace SpookyBotanyGame.World.Entities.Plants.States.DarkEye
{
    public class DestroyState : State<DarkPlantEye>
    {
        public DestroyState(DarkPlantEye owner) : base(owner)
        {
            _owner.Animation.Play("Destroyed");
            _owner.OnDayTick += HandleDayTick;
            _owner.DaysToRegrow += 2;
        }

        protected override void Exit(State state = null)
        {
            _owner.OnDayTick -= HandleDayTick;
            base.Exit(state);
        }

        private void HandleDayTick(int dayCount)
        {
            Exit(new GrowingState(_owner, 0));
        }
    }
}