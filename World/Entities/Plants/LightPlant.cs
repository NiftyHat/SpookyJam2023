using Godot;
using SpookyBotanyGame.Core.StateMachines;
using SpookyBotanyGame.World.Entities.Plants.States;
using SpookyBotanyGame.World.Entities.Properties;

namespace SpookyBotanyGame.World.Entities.Plants
{
    public partial class LightPlant : GameEntity
    {
        [Export] public LightSensor LightSensor { get; set; }
        [Export] public float LightRequired { get; set; }
        [Export] public SimAdvanceable Time { get; set; }
        [Export] public AnimationPlayer Animation { get; set; }
        [Export] public StateMachine StateMachine { get; set; }
        [Export] public PointLight2D Light { get; set; }

        public override void _Ready()
        {
            base._Ready();
            StateMachine.SetState(new LightFeedingState(this, 1));
        }
        
        public void Destroy()
        {
            this.QueueFree();
        }
    }
}

