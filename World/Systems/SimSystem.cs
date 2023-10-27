using System;
using Godot;

namespace SpookyBotanyGame.World.Systems;

public partial class SimSystem : Resource
{
    public delegate void OnDaysTicked(int dayCount);
    public class WorldTime : IReadOnlyTime
    {

        
        public int Day { get; private set; }

        public event OnDaysTicked OnDayTick;

        public void AdvanceDay(int days)
        {
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

    public WorldTime Time { get; set; }

    public SimSystem()
    {
        Time = new WorldTime();
    }
}