using System;
using Godot;
using SpookyBotanyGame.Collectable;
using SpookyBotanyGame.Core.StateMachines;
using SpookyBotanyGame.World.Entities.Effects;
using SpookyBotanyGame.World.Entities.Farm.Tillable;
using SpookyBotanyGame.World.Entities.Plants.Planting;
using SpookyBotanyGame.World.Entities.Plants.States;
using SpookyBotanyGame.World.Entities.Properties;
using SpookyBotanyGame.World.Systems;

namespace SpookyBotanyGame.World.Entities.Plants
{
    public partial class LightPlant : GameEntity, IPlantable
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
        [Export] public int InitialGrowthState { get; set; } = 1;

        public RandomNumberGenerator _rng = new RandomNumberGenerator();
        public event SimSystem.OnDaysTicked OnDayTick;
        public event Action OnDestroyed;

        public override void _Ready()
        {
            base._Ready();
            StateMachine.SetState(new GrowingState(this, InitialGrowthState));
            Sim.OnDayTick += HandleDayTick;
        }

        private void HandleDayTick(int daycount)
        {
            OnDayTick?.Invoke(daycount);
        }

        public void Destroy()
        {
            OnDayTick = null;
            OnDestroyed?.Invoke();
            QueueFree();
        }
        public void SetSpot(TillableSpot spot)
        {
            spot.AddChild(this);
            Position = Vector2.Zero;
            StateMachine.SetState(new GrowingState(this, InitialGrowthState));
        }
    }
}

