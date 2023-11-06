using System;
using Godot;
using SpookyBotanyGame.World.Entities;

namespace SpookyBotanyGame.World.Systems;

public partial class SimSystem : Node
{
    public delegate void OnDaysTicked(int dayCount);

    public delegate void OnPlayerDied(PlayerEntity player);
    public delegate void OnPlayerSlept(PlayerEntity player);
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
    public event OnPlayerSlept OnPlayerSleep;

    public void PlayerSleep(PlayerEntity playerEntity)
    {
        _time.AdvanceDay(1);
        OnPlayerSleep?.Invoke(playerEntity);
    }

    public void PlayerDied(PlayerEntity playerEntity)
    {
        OnPlayerDie?.Invoke(playerEntity);
    }
    
    public void PlayerRespawn(PlayerEntity playerEntity)
    {
        _time.AdvanceDay(1);
    }
}