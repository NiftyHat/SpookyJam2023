using System.Collections.Generic;
using Godot;

namespace SpookyBotanyGame.World.Entities.Properties
{
    [GlobalClass,Icon("res://World/Entities/icon-entity-property.svg")]
    public partial class LightSensor : EntityProperty
    {
        [Export] public CollisionObject2D HitBox { get; set; }
        
        public delegate void LightHit(LightEmissionZone lightEmissionZone, float lightPower);
        public delegate void LightAppliedChanged(float lightPower);
        public event LightHit OnEnter;
        public event LightHit OnExit;
        public event LightHit OnApply;

        private readonly Dictionary<LightEmissionZone, float> _items = new Dictionary<LightEmissionZone, float>();
        
        public override void _Ready()
        {
            base._Ready();
            _entity?.AddProperty(this);
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
        }

        public bool TryGetStrongestLight(out LightEmissionZone zone, out float strongestLight)
        {
            if (_items.Count == 0)
            {
                zone = null;
                strongestLight = 0;
                return false;
            }
            GD.Print($"Has lights to count!");
            zone = null;
            strongestLight = 0;
            foreach (var item in _items)
            {
                if (item.Value > strongestLight)
                {
                    GD.Print($"Added light {item.Key} {item.Value}");
                    strongestLight = item.Value;
                    zone = item.Key;
                }
            }
            return true;
        }

        public void LitBy(LightEmissionZone lightEmissionZone, float lightPower)
        {
            OnEnter?.Invoke(lightEmissionZone, lightPower);
            _items.Add(lightEmissionZone, lightPower);
        }

        public void UnlitBy(LightEmissionZone lightEmissionZone, float lightPower)
        {
            OnExit?.Invoke(lightEmissionZone, lightPower);
            _items.Remove(lightEmissionZone);
        }

        public void ApplyLight(LightEmissionZone lightEmissionZone, float lightPower)
        {
            OnApply?.Invoke(lightEmissionZone, lightPower);
            _items[lightEmissionZone] = lightPower;
        }
    }
}