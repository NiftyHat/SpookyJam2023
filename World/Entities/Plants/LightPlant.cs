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
        
        [Export] public GpuParticles2D GrowParticles { get; set; }
        
        //[Export] public PathLineRenderer LightLineEffect { get; set; }
        
        [Export] public SpriteShaderEffect LightGlowEffect { get; set; }

        private bool _isMaxGrowthState;

        public RandomNumberGenerator _rng = new RandomNumberGenerator();
        public event SimSystem.OnDaysTicked OnDayTick;
        public event Action OnDestroyed;
        public event Action<bool, IPlantable> OnMaxGrowthStateChanged;

        private float _lightGrowthMax = 0.4f;

        public override void _Ready()
        {
            base._Ready();
            StateMachine.SetState(new GrowingState(this, InitialGrowthState));
            Sim.OnDayTick += HandleDayTick;
            LightGlowEffect.TweenIn = TweenInGlow;
            LightGlowEffect.TweenOut = TweenOutGlow;
        }

        private Tween TweenInGlow(ShaderMaterial material)
        {
            var tween = LightGlowEffect.CreateTween();
            //tween.TweenProperty(LightGlowEffect.Material, "shader_parameter/amount", 1d, 0.3f).From(0d);
            tween.TweenMethod(Callable.From((double value) =>
            {
                GD.Print(nameof(TweenInGlow), value);
                material.SetShaderParameter("amount", value);
            }), 0f, _lightGrowthMax, 0.6f).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Cubic);
            return tween;
        }
        private Tween TweenOutGlow(ShaderMaterial material)
        {
            var tween = LightGlowEffect.CreateTween();
            tween.TweenMethod(Callable.From((double value) =>
            {
                GD.Print(nameof(TweenOutGlow), value);
                LightGlowEffect.Material.SetShaderParameter("amount", value);
            }), _lightGrowthMax, 0d, 0.3f).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Circ);
            //tween.TweenMethod(Callable.From (() => { LightGlowEffect.Material.SetShaderParameter("radius");}), 0, 1.0f, 0.5f);
            //tween.TweenProperty(LightGlowEffect.Material, "shader_parameter/amount", 0f, 0.1f);
            return tween;
        }

        private void HandleDayTick(int daycount)
        {
            OnDayTick?.Invoke(daycount);
        }

        public void SetMaxGrowthState(bool state)
        {
            if (_isMaxGrowthState != state)
            {
                _isMaxGrowthState = state;
                OnMaxGrowthStateChanged?.Invoke(_isMaxGrowthState, this);
            }
        }

        public void Destroy()
        {
            OnDayTick = null;
            OnDestroyed?.Invoke();
            StateMachine.Destroy();
            StateMachine = null;
            QueueFree();
        }
        public void SetSpot(TillableSpot spot)
        {
            spot.GrowSpot.AddChild(this);
            Position = Vector2.Zero;
            StateMachine.SetState(new GrowingState(this, InitialGrowthState));
        }
    }
}

