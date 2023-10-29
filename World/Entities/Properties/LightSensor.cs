using System;
using System.Diagnostics;
using Godot;

namespace SpookyBotanyGame.World.Entities.Properties
{
    [GlobalClass,Icon("res://World/Entities/icon-entity-property.svg")]
    public partial class LightSensor : EntityProperty
    {
        public struct LightLevelDelta
        {
            public float ThisFrame;
            public float LastFrame;
            public float Value => ThisFrame - LastFrame;
        }
        [Export] public CollisionObject2D HitBox { get; set; }
        
        public delegate void LightHit(LightEmissionZone lightEmissionZone, float lightPower);

        public delegate void LightAppliedChanged(float lightPower);
        public event LightHit OnEnter;
        public event LightHit OnExit;
        public event LightHit OnApply;

        public event LightAppliedChanged OnAppliedLightChange;

        public LightLevelDelta MaxLightDelta = new LightLevelDelta();
        private float _lastDelta;
        
        public override void _Ready()
        {
            base._Ready();
            _entity?.AddProperty(this);
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            
            if (MaxLightDelta.Value > LIGHT_CHANGE_TOLERANCE)
            {
                OnAppliedLightChange?.Invoke(MaxLightDelta.ThisFrame);
                MaxLightDelta.ThisFrame = MaxLightDelta.LastFrame;
            }
        }

        private const double LIGHT_CHANGE_TOLERANCE = 0.0001d;

        public void LitBy(LightEmissionZone lightEmissionZone, float lightPower)
        {
            OnEnter?.Invoke(lightEmissionZone, lightPower);
        }

        public void UnlitBy(LightEmissionZone lightEmissionZone, float lightPower)
        {
            OnExit?.Invoke(lightEmissionZone, lightPower);
            MaxLightDelta.ThisFrame = 0;
        }

        public void ApplyLight(LightEmissionZone lightEmissionZone, float lightPower)
        {
            OnApply?.Invoke(lightEmissionZone, lightPower);
            if (lightPower > MaxLightDelta.LastFrame)
            {
                MaxLightDelta.ThisFrame = lightPower;
            }
        }
    }
}