using Godot;
using SpookyBotanyGame.Core;
using SpookyBotanyGame.Core.StateMachines;
using SpookyBotanyGame.World.Entities.Properties;

namespace SpookyBotanyGame.World.Entities.Plants.States.DarkStem;

public class StemGrowingState : State
{
    private readonly string _animationName;
    private readonly int _progress = 0;
    
    private readonly DarkPlantStem _plant;

    public const string GrowingAnimationName = "Growing";
    private bool _wasAnimationPlayed;

    public StemGrowingState(DarkPlantStem plant, int progress)
    {
        _plant = plant;
        _progress = progress;
        _plant.Sim.OnDayTick += HandleDayAdvance;
        _plant.LightSensor.OnApply += HandleLightApply;
        _plant.OnAnimationFinished += HandleAnimationFinish;
        _animationName = GetProgressAnimation(GrowingAnimationName, progress, _plant.Animation);
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
        _plant.LightSensor.OnApply -= HandleLightApply;
        _plant.OnAnimationFinished -= HandleAnimationFinish;
        _plant.Sim.OnDayTick -= HandleDayAdvance;
        base.Exit(state);
    }


    private void PauseOnFirstFrame()
    {
        _plant.Animation.Play(_animationName);
        _plant.Animation.Advance(0);
        _plant.Animation.Pause();
    }

    private void HandleAnimationFinish(string animName)
    {
        if (animName == _animationName)
        {
            if (!TryGetProgressAnimation("Growing", _progress + 1, _plant.Animation, out _))
            {
                Exit(new IdleState(_plant));
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

        GD.PushWarning(
            $"{nameof(GrowingState)} {nameof(GetProgressAnimation)} on {animation.Name} with progress {animName}");
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
        //TODO - dsaunders let the player use light to block growth progress of Stems.
        if (!TryGetProgressAnimation("Growing", _progress + 1, _plant.Animation, out _))
        {
            Exit(new IdleState(_plant));
        }
        else
        {
            Exit(new StemGrowingState(_plant, _progress + 1));
        }
    }

    private void HandleLightApply(LightEmissionZone lightEmissionZone, float lightPower)
    {
        if (_wasAnimationPlayed)
        {
            return;
        }

        if (lightPower > 0.1f)
        {
            _plant.Animation.Play(_animationName);
            _wasAnimationPlayed = true;
        }
    }
}