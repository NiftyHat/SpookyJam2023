using Godot;
using SpookyBotanyGame.Core.StateMachines;

namespace SpookyBotanyGame.World.Entities.Plants.States.DarkEye
{
    public class GrowingState : State
    {
        protected DarkPlantEye _plant;
        public const string AnimationName = "Growing";
        protected int _progress;

        public GrowingState(DarkPlantEye plant, int progress)
        {
            _plant = plant;
            _plant.OnDayTick += HandleDayTick;
            PlayGrowingAnimation(_plant.Animation);
            _progress = progress;
        }
        
        private void PlayGrowingAnimation(AnimationPlayer animationPlayer)
        {
            animationPlayer.Play(AnimationName);
        }

        private void HandleDayTick(int dayCount)
        {
            if (_progress >= _plant.DaysToRegrow)
            {
                Exit(new IdleState(_plant));
            }
            else
            {
                Exit(new GrowingState(_plant, _progress + 1));
            }
        }
    }
}