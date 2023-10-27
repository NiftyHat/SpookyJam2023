using Godot;

namespace SpookyBotanyGame.World.Entities.Properties;

[GlobalClass,Icon("res://World/Entities/icon-entity-property.svg")]
public partial class SimAdvanceable : Node
{
    public delegate void OnTick(int amount);

    public event OnTick OnDayTick;

    public void AdvanceDay(int i)
    {
        OnDayTick?.Invoke(i);
    }
}