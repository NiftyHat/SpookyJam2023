using SpookyBotanyGame.Core.StateMachines;

namespace SpookyBotanyGame.World.Entities.Plants.States.DarkEye
{
    public class HurtState : State<DarkPlantEye>
    {
        public HurtState(DarkPlantEye owner, int damage) : base (owner)
        {
            _owner.Health.Value -= damage;
            _owner.OnAnimationFinished += HandleAnimationFinished;
            _owner.Animation.Play("Hurt");
        }

        protected override void Exit(State state = null)
        {
            base.Exit(state);
        }

        private void HandleAnimationFinished(string animationName)
        {
            _owner.OnAnimationFinished -= HandleAnimationFinished;
            if (animationName == "Hurt")
            {
                if (_owner.Health.Value <= 0)
                {
                    Exit(new DestroyState(_owner));
                }
                else
                {
                    Exit(new DarkEye.IdleState(_owner));
                }
            }
        }
    }
}