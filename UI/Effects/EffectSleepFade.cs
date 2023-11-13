using Godot;
using SpookyBotanyGame.Core;
using SpookyBotanyGame.World.Entities;
using SpookyBotanyGame.World.Systems;

namespace SpookyBotanyGame.UI.Effects;

public partial class EffectSleepFade : Node
{
    [Export] public ColorRect Rect { get; set; }
    [Export] public AudioStreamPlayer SleepJingle { get; set; }

    private Tween _fadeTween;
    
    Color COLOR_TRANSPARENT = new Color(0,0,0, 0);
    Color COLOR_SOLID_BLACK = new Color(0,0,0, 1f);

    public override void _Ready()
    {
        base._Ready();
        var sim = GetNode<SimSystem>("/root/SimSystem");
        if (sim == null)
        {
            GD.PrintErr($"{nameof(EffectSleepFade)} didn't attach to SimSystem.Time");
            return;
        }

        sim.DayEndEvents.Started += HandleFadeOut;
        sim.DayStartEvents.Started += HandleFadeIn;
        //sim.OnPlayerSleep += HandlePlayerSleep;
    }

    private void HandleFadeOut()
    {
        _fadeTween?.Kill();
        _fadeTween = CreateTween();
        SleepJingle.Play();
        Rect.Visible = true;
        Rect.Modulate = COLOR_TRANSPARENT;
        _fadeTween.TweenProperty(Rect, "modulate",  COLOR_SOLID_BLACK, 2f)
            .From( COLOR_TRANSPARENT)
            .SetEase(Tween.EaseType.In)
            .SetTrans(Tween.TransitionType.Cubic);
        
    }
    
    private void HandleFadeIn()
    {
        if (_fadeTween != null)
        {
            _fadeTween.Kill();
        }
        _fadeTween = CreateTween();
        _fadeTween.TweenProperty(Rect, "modulate", COLOR_TRANSPARENT, 0.5f)
            .From(COLOR_SOLID_BLACK)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Cubic);
        _fadeTween.TweenCallback(Callable.From(() =>
        {
            Rect.Visible = false;
        }));
    }
    
    private void DoFade(FadeEvents fadeInEvents, FadeEvents fadeOutEvents)
    {
        /*
        if (_fadeTween != null)
        {
            _fadeTween.Kill();
        }
        _fadeTween = CreateTween();
        Rect.Visible = true;
        Rect.Modulate = COLOR_TRANSPARENT;
        _fadeTween.TweenCallback(fadeInEvents.Start);
        _fadeTween.TweenProperty(Rect, "modulate", COLOR_SOLID_BLACK, 1.5f).SetTrans(Tween.TransitionType.Cubic);
        _fadeTween.TweenCallback(fadeInEvents.Complete);
        _fadeTween.TweenInterval(2f);
        _fadeTween.TweenCallback(fadeOutEvents.Start);
        _fadeTween.TweenProperty(Rect, "modulate", COLOR_TRANSPARENT, 1.5f).SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Cubic);
        _fadeTween.TweenCallback(fadeOutEvents.Complete);
        */
    }
}