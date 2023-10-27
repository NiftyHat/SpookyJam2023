using Godot;
using SpookyBotanyGame.Core;
using SpookyBotanyGame.Core.StateMachines;
using SpookyBotanyGame.World.Entities.Properties;

namespace SpookyBotanyGame.World.Entities.Plants.States
{
    public class GrowingState : State, IUpdatableState
    {
        private readonly Range<float> _growthEnergy;
        private readonly AnimationPlayer _animation;
        private readonly LightSensor _lightSensor;
        private readonly string _animationName;
        private readonly int _progress = 0;
        
        private SimAdvanceable _sim;
        private float _lightThisFrame;

        private LightPlant _plant;
        
        public const string GrowingAnimationName = "Growing";
        private bool _hasAnimationHandler;

        public GrowingState(LightPlant plant, int progress, float energyRequired = 1.0f)
        {
            _plant = plant;
            _animation = plant.Animation;
            _lightSensor = plant.LightSensor;
            _sim = plant.Sim;
            _progress = progress;
            plant.Interactable.SetEnabled(false);
            plant.Light.Energy = 0;
            _growthEnergy = new Range<float>(energyRequired);
            _sim.OnDayTick += HandleDayAdvance;
            _lightSensor.OnApply += HandleLightApply;
            _lightSensor.OnEnter += HandleLightEnter;
            _lightSensor.OnExit += HandleLightExit;
            _growthEnergy.OnMax += HandleMaxEnergy;
            _plant.Animation.AnimationFinished += HandleAnimationFinish;
            _hasAnimationHandler = true;
            _animationName = GetProgressAnimation(GrowingAnimationName, progress, _animation);
            if (_animationName != null)
            {
                PauseOnFirstFrame();
            }
            else
            {
                GD.PushWarning($"{nameof(GrowingState)} null ref on {_animationName}");
            }
        }

        protected override void Exit(State state = null)
        {
            _lightSensor.OnApply -= HandleLightApply;
            _lightSensor.OnEnter -= HandleLightEnter;
            _lightSensor.OnExit -= HandleLightExit;
            _growthEnergy.OnMax -= HandleMaxEnergy;
            if (_hasAnimationHandler)
            {
                _plant.Animation.AnimationFinished -= HandleAnimationFinish;
                _hasAnimationHandler = false;
            }
            base.Exit(state);
        }


        private void PauseOnFirstFrame()
        {
            _plant.Animation.Play(_animationName);
            _plant.Animation.Advance(0);
            _plant.Animation.Pause();
            
        }

        private void HandleAnimationFinish(StringName animName)
        {
            if (animName.ToString() == _animationName)
            {
                if (!TryGetProgressAnimation("Growing", _progress + 1, _animation, out string animationName))
                {
                    Exit(new HarvestState(_plant, _plant.OutputAmount, _plant.Output));
                }
            }
        }

        private static string GetProgressAnimation(string name, int progress, AnimationPlayer animation)
        {
            string animName = $"{name}-{progress}";
            if (animation.HasAnimation(animName))
            {
                return animName;
            }
            GD.PushWarning($"{nameof(GrowingState)} {nameof(GetProgressAnimation)} on {animation.Name} with progress {animName}");
            return null;
        }

        public static bool TryGetProgressAnimation(string name, int progress, AnimationPlayer animationPlayer,
            out string animationName)
        {
            animationName = $"{name}-{progress}";
            if (animationPlayer.HasAnimation(animationName))
            {
                return true;
            }
            return false;
        }

        private void HandleDayAdvance(int amount)
        {
            GD.Print("GrowingState", "HandleDayAdvance");
            if (_growthEnergy.IsMax)
            {
                if (TryGetProgressAnimation("Growing", _progress + 1, _animation, out string animationName))
                {
                    Exit(new GrowingState(_plant, _progress + 1));
                }
                else
                {
                    Exit(new HarvestState(_plant, _plant.OutputAmount, _plant.Output));
                }
            }
            else
            {
                Exit(new RottingState(_plant));
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
                    if (_plant.Effects != null)
                    {
                        _plant.Effects.SetScale(EnergyPercentage(_growthEnergy));
                        _plant.Effects.SetIsLit(true);
                    }
                    _lightThisFrame = 0;
                }
            }
        }

        public static float EnergyPercentage(Range<float> range)
        {
            if (range.Min == 0 && range.Max == 0)
            {
                return 1.0f;
            }
            if (range.Min == 0)
            {
                return 1.0f / (range.Max - range.Min) * range.Value;
            }

            return 0;
        }

        private void HandleLightApply(LightEmissionZone lightEmissionZone, float lightPower)
        {
            if (lightPower > _lightThisFrame)
            {
                _lightThisFrame = lightPower;
            }
        }
        
        private void HandleLightEnter(LightEmissionZone lightEmissionZone, float lightPower)
        {
            _plant.Effects?.SetIsLit(true);
        }

        private void HandleLightExit(LightEmissionZone lightEmissionZone, float lightPower)
        {
            _plant.Effects?.SetIsLit(false);
        }

    }
}