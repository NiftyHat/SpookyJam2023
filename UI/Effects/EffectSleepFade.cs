using Godot;
using SpookyBotanyGame.World.Entities;
using SpookyBotanyGame.World.Entities.Properties;
using SpookyBotanyGame.World.Systems;

namespace SpookyBotanyGame.UI.Effects;

public partial class EffectSleepFade : Node
{
    [Export] public ColorRect Rect { get; set; }
    [Export] public AudioStreamPlayer SleepJingle { get; set; }

    private Tween _fadeTween;

    private event System.Action OnSleepFadeComplete;

    public override void _Ready()
    {
        base._Ready();
        var sim = GetNode<SimSystem>("/root/SimSystem");
        if (sim == null)
        {
            GD.PrintErr($"{nameof(EffectSleepFade)} didn't attach to SimSystem.Time");
            return;
        }
        sim.OnPlayerSleep += HandlePlayerSleep;
    }

    private void HandlePlayerSleep(PlayerEntity player)
    {
        DoFade();
    }

    private void DoFade()
    {
        if (_fadeTween != null)
        {
            _fadeTween.Kill();
        }
        _fadeTween = CreateTween();
        Color colorAlpha = new Color(0,0,0, 0);
        Color colorSolid = new Color(0,0,0, 1f);
        Rect.Visible = true;
        Rect.Modulate = colorAlpha;
        _fadeTween.TweenProperty(Rect, "modulate", colorSolid, 1.5f).SetTrans(Tween.TransitionType.Cubic);
        _fadeTween.TweenInterval(2f);
        _fadeTween.TweenProperty(Rect, "modulate", colorAlpha, 1.5f).SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Cubic);
        /*
        _fadeTween.TweenInterval(2f);
        _fadeTween.TweenProperty(Rect, "modulate", colorAlpha, 1.5f).SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Cubic).SetDelay(2.0f);
        //_fadeTween.TweenCallback(Callable.From(() => Rect.Visible = false));
        /*
        //_fadeTween.TweenCallback(Callable.From(() => OnSleepFadeComplete?.Invoke()));
        _fadeTween.TweenProperty(Rect, "modulate", colorAlpha, 1.5f).SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Cubic);
        _fadeTween.TweenCallback(Callable.From(() => Rect.Visible = false)).SetDelay(1.5);*/
        SleepJingle.Play();
        //_fadeTween.TweenCallback(Callable.From(() => Rect.Modulate.A = 0 ))
        //_fadeTween.TweenCallback(Callable.From(() => SetAnimationState(texture, frame))).SetDelay(0.1f);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }
}