using Godot;
using SpookyBotanyGame.World.Systems;

namespace SpookyBotanyGame.World.Entities.Properties;

[GlobalClass,Icon("res://World/Entities/icon-entity-property.svg")]
public partial class SimAdvanceable : Node
{
    public event SimSystem.OnDaysTicked OnDayTick;

    private SimSystem.IReadOnlyTime _time;
    private int _daysAlive;

    public override void _Ready()
    {
        base._Ready();
        var sim = GetNode<SimSystem>("/root/SimSystem");
        if (sim == null)
        {
            GD.PrintErr($"{nameof(SimAdvanceable)} didn't attach to SimSystem.Time");
        }
        _time = sim.Time;
        _time.OnDayTick += HandleSimDayTick;
    }

    private void HandleSimDayTick(int dayCount)
    {
        _daysAlive += dayCount;
        OnDayTick?.Invoke(dayCount);
    }
}