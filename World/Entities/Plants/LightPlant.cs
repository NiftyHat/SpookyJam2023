using Godot;
using SpookyBotanyGame.Collectable;
using SpookyBotanyGame.Core.StateMachines;
using SpookyBotanyGame.World.Entities.Effects;
using SpookyBotanyGame.World.Entities.Plants.States;
using SpookyBotanyGame.World.Entities.Properties;

namespace SpookyBotanyGame.World.Entities.Plants
{
    public partial class LightPlant : GameEntity
    {
        [Export] public LightSensor LightSensor { get; set; }
        
        [Export(PropertyHint.Range, "0.05,2,0.05,or_greater")] public float LightRequired { get; set; }
        [Export] public SimAdvanceable Sim { get; set; }
        [Export] public AnimationPlayer Animation { get; set; }
        [Export] public StateMachine StateMachine { get; set; }
        [Export] public PointLight2D Light { get; set; }
        [Export] public CollectableResource Output { get; set; }
        [Export(PropertyHint.Range, "1,10,1,or_greater")] public int OutputAmount { get; set; }
        
        [Export] public EffectsLightPlant Effects { get; set; }
        
        [Export] public Interactable Interactable { get; set; }

        public static int DUMB_COUNTER = 1;

        public override void _Ready()
        {
            base._Ready();
            StateMachine.SetState(new GrowingState(this, DUMB_COUNTER));
            DUMB_COUNTER++;
        }
        
        public void Destroy()
        {
            this.QueueFree();
        }
    }
}

