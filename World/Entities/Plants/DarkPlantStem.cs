using System;
using Godot;
using SpookyBotanyGame.Core;
using SpookyBotanyGame.Core.StateMachines;
using SpookyBotanyGame.World.Entities.Plants.States.DarkStem;
using SpookyBotanyGame.World.Entities.Properties;
using SpookyBotanyGame.World.Systems;

namespace SpookyBotanyGame.World.Entities.Plants;

[GlobalClass]
public partial class DarkPlantStem : GameEntity
{
    [Export] public LightSensor LightSensor { get; set; }
    
    [Export] public AnimationPlayer Animation { get; set; }
    
    [Export] public SimAdvanceable Sim { get; set; }
    
    [Export] public StateMachine StateMachine { get; set; }

    public Range<float> Health { get; protected set; } = new Range<float>(3, 0, 3);
    public bool CanTriggerAttack { get; set; } = false;

    public event Action<string> OnAnimationFinished;
    
    public event SimSystem.OnDaysTicked OnDayTick;


    public override void _Ready()
    {
        base._Ready();
        StateMachine.SetState(new StemGrowingState(this, 1));
        Animation.AnimationFinished += HandleAnimationFinished;
        Sim.OnDayTick += HandleDayTick;
    }
    
    private void HandleDayTick(int daycount)
    {
        OnDayTick?.Invoke(daycount);
    }

    private void HandleAnimationFinished(StringName animname)
    {
        OnAnimationFinished?.Invoke(animname);
    }

    public void Destroy()
    {
        OnDayTick = null;
        Animation.AnimationFinished -= HandleAnimationFinished;
        QueueFree();
    }
}