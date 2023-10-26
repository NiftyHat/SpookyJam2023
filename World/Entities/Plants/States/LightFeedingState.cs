using Godot;
using SpookyBotanyGame.Core;
using SpookyBotanyGame.Core.StateMachines;
using SpookyBotanyGame.World.Entities.Properties;

namespace SpookyBotanyGame.World.Entities.Plants.States
{
    public class LightFeedingState : State, IUpdatableState
    {
        private readonly Range<float> _growthEnergy;
        private readonly AnimationPlayer _animation;
        private readonly LightSensor _lightSensor;
        private readonly string _animationName;

        private readonly int _progress = 0;
        private SimAdvanceable _time;
        private float _lightThisFrame;

        private LightPlant _plant;
        
        public const string GrowingAnimationName = "Growing";

        public LightFeedingState(LightPlant plant, int progress)
        {
            _plant = plant;
            _animation = plant.Animation;
            _lightSensor = plant.LightSensor;
            _time = plant.Time;
            _growthEnergy = new Range<float>(20);
            _time.OnDayTick += HandleDayAdvance;
            _lightSensor.OnApply += HandleLightApply;
            _growthEnergy.OnMax += HandleMaxEnergy;
            _animationName = GetProgressAnimation(GrowingAnimationName, progress, _animation);
            if (_animationName != null)
            {
                PauseOnFirstFrame();
            }
            else
            {
                GD.PushWarning($"{nameof(LightFeedingState)} null ref on {_animationName}");
            }
        }

        private void PauseOnFirstFrame()
        {
            _animation.Play(_animationName);
            _animation.Pause();
        }

        private static string GetProgressAnimation(string name, int progress, AnimationPlayer animation)
        {
            string animName = $"{name}-{progress}";
            if (animation.HasAnimation(animName))
            {
                return animName;
            }
            GD.PushWarning($"{nameof(LightFeedingState)} {nameof(GetProgressAnimation)} on {animation.Name} with progress {animName}");
            return null;
        }

        private void HandleDayAdvance(int amount)
        {
            if (_growthEnergy.IsMax)
            {
                if (_animation.HasAnimation("grow"))
                {
                    Exit(new LightFeedingState(_plant, _progress + 1));
                }
            }
        }

        private void HandleMaxEnergy(float value)
        {
            _animation.Play(_animationName);
            _plant.Light.Energy = 0.2f;
        }

        public void Process(double delta)
        {
            if (!_growthEnergy.IsMax)
            {
                if (_lightThisFrame > 0)
                {
                    _growthEnergy.Value += _lightThisFrame * (float)delta;
                    _lightThisFrame = 0;
                }
            }
        }

        private void HandleLightApply(LightZone lightZone, float lightPower)
        {
            if (lightPower > _lightThisFrame)
            {
                _lightThisFrame = lightPower;
            }
        }
    }
}