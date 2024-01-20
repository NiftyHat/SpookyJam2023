using System;
using Godot;
using SpookyBotanyGame.Core;
using SpookyBotanyGame.UI;
using SpookyBotanyGame.UI.Screens.Pause;
using SpookyBotanyGame.World.Entities;

namespace SpookyBotanyGame.World.Systems;

public partial class SimSystem : Node
{
    public delegate void OnDaysTicked(int dayCount);
    public delegate void OnPlayerDied(PlayerEntity player);
    
    public class SimTime : IReadOnlyTime
    {
        public int Day { get; private set; }

        public event OnDaysTicked OnDayTick;
        public void AdvanceDay(int days)
        {
            GD.Print($"Advance day {days}");
            if (days <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(days),
                    $"Sim.Time.AdvanceDay() param {nameof(days)} cannot zero or less");
            }
            Day += days;
            OnDayTick?.Invoke(days);
        }
    }
    
    public interface IReadOnlyTime
    {
        public event OnDaysTicked OnDayTick;
    }

    private SimTime _time = new SimTime();
    public IReadOnlyTime Time => _time;

    public event OnPlayerDied OnPlayerDie;

    public readonly FadeEvents DayEndEvents = new FadeEvents();
    public readonly FadeEvents DayStartEvents = new FadeEvents();
    
    private UIController _ui;
    private bool _isPaused;
    public bool IsPaused => _isPaused;

    public override void _Ready()
    {
        _ui = GetNode<UIController>("/root/UIController");
        base._Ready();
    }

    public void PlayerRest(PlayerEntity playerEntity)
    {
        Tween restTween = CreateTween();
        restTween.TweenCallback(DayEndEvents.Start);
        restTween.TweenInterval(1f);
        restTween.TweenCallback(DayEndEvents.Complete);
        restTween.TweenCallback(Callable.From(() => { _time.AdvanceDay(1); }));
        restTween.TweenInterval(2f);
        restTween.TweenCallback(DayStartEvents.Start);
        restTween.TweenInterval(1f);
        restTween.TweenCallback(DayStartEvents.Complete);
    }

    public void PlayerDied(PlayerEntity playerEntity)
    {
        OnPlayerDie?.Invoke(playerEntity);
    }
    
    public void PlayerRespawn(PlayerEntity playerEntity)
    {
        _time.AdvanceDay(1);
    }

    public void Pause()
    {
        if (_isPaused)
        {
            return;
        }
        _ui.Open<PauseScreen>();
        GetTree().Paused = true;
        _isPaused = true;
        
    }

    public void Resume()
    {
        if (!_isPaused)
        {
            return;
        }
        GetTree().Paused = false;
        _isPaused = false;
        _ui.Close<PauseScreen>();
    }
}