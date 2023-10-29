using System;
using Godot;
using SpookyBotanyGame.Core;
using SpookyBotanyGame.Core.StateMachines;
using SpookyBotanyGame.World.Entities.Plants.States.DarkStem;
using SpookyBotanyGame.World.Entities.Properties;

namespace SpookyBotanyGame.World.Entities.Plants;

[GlobalClass]
public partial class DarkPlantStem : GameEntity
{
    [Export] public LightSensor LightSensor { get; set; }
    
    [Export] public AnimationPlayer Animation { get; set; }
    
    [Export] public SimAdvanceable Sim { get; set; }
    
    [Export] public StateMachine StateMachine { get; set; }
    
    public Range<float> Health { get; protected set; }
    public bool CanAttack { get; set; } = false;
            
    public event Action<string> OnAnimationFinished;

    public override void _Ready()
    {
        base._Ready();
        StateMachine.SetState(new IdleState(this));
        Animation.AnimationFinished += HandleAnimationFinished;
    }

    private void HandleAnimationFinished(StringName animname)
    {
        OnAnimationFinished?.Invoke(animname);
    }

    public void Destroy()
    {
        QueueFree();
    }
}