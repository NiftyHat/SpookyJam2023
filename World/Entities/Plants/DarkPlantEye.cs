using System;
using Godot;
using SpookyBotanyGame.Collectable;
using SpookyBotanyGame.Core.StateMachines;
using SpookyBotanyGame.World.Entities.Animation;
using SpookyBotanyGame.World.Entities.Farm.Tillable;
using SpookyBotanyGame.World.Entities.Plants.Planting;
using SpookyBotanyGame.World.Entities.Plants.States.DarkEye;
using SpookyBotanyGame.World.Entities.Properties;
using SpookyBotanyGame.World.Systems;

namespace SpookyBotanyGame.World.Entities.Plants;

[GlobalClass]
public partial class DarkPlantEye : GameEntity, IPlantable
{
    [Export] public LightSensor LightSensor { get; set; }
    
    [Export] public AnimationPlayer Animation { get; set; }
    
    [Export] public SimAdvanceable Sim { get; set; }
    
    [Export] public StateMachine StateMachine { get; set; }
    
    [Export] public Interactable Interactable { get; set; }
    
    [Export] public CollectableResource Output { get; set; }
    
    [Export] public LookDirectionAnimationPlayer LookAnimation { get; set; }
    
    [Export] public EntityDetectionZone LookDetectionZone { get; set; }

    [Export] public int DaysToRegrow { get; set; } = 3;

    public event SimSystem.OnDaysTicked OnDayTick;
    public event Action OnDestroyed;

    public override void _Ready()
    {
        base._Ready();
        StateMachine.SetState(new IdleState(this));
        Sim.OnDayTick += HandleDayTick;
    }

    private void HandleDayTick(int daycount)
    {
        OnDayTick?.Invoke(daycount);
    }

    public void Destroy()
    {
        OnDestroyed?.Invoke();
        QueueFree();
    }


    public void SetSpot(TillableSpot spot)
    {
        spot.AddChild(this);
        Position = Vector2.Zero;
        StateMachine.SetState(null);
        StateMachine.SetState(new GrowingState(this, 1));
    }
}