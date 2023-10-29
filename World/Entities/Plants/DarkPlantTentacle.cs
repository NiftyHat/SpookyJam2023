using System;
using Godot;
using SpookyBotanyGame.Core.StateMachines;
using SpookyBotanyGame.World.Entities.Plants.States.DarkPlant;
using SpookyBotanyGame.World.Entities.Properties;

namespace SpookyBotanyGame.World.Entities.Plants
{
    [GlobalClass]
    public partial class DarkPlantTentacle : GameEntity
    {
        [Export] public LightSensor LightSensor { get; set; }
    
        [Export] public SimAdvanceable Sim { get; set; }
    
        [Export] public AnimationPlayer Animation { get; set; }
    
        [Export] public StateMachine StateMachine { get; set; }
        
        [Export] public DarkPlantStem PlantStem { get; set; }

        [Export(PropertyHint.Range, "0,1,0.05")]
        public float LightTriggerAttackThreshold { get; set; } = 0.1f;
        public bool IsLightTriggeringAttack { get; set; } = false;

        public bool HasAttackFromStem => PlantStem != null && PlantStem.CanAttack;
        
        public event Action<string> OnAnimationFinished;
        
        public override void _Ready()
        {
            base._Ready();
            Animation.AnimationFinished += HandleAnimationFinished;
            LightSensor.OnApply += HandleLightApply;
            StateMachine.SetState(new WaitForTargetState(this));
        }

        private void HandleAnimationFinished(StringName animName)
        {
            OnAnimationFinished?.Invoke(animName);
        }

        private void HandleLightApply(LightEmissionZone zone, float lightPower)
        {
            if (!HasAttackFromStem)
            {
                IsLightTriggeringAttack = false;
                return;
            }
            
            if (lightPower > 0)
            {
                IsLightTriggeringAttack = lightPower >= LightTriggerAttackThreshold;
            }
            else
            {
                IsLightTriggeringAttack = false;
            }
        }
        
    }
}

