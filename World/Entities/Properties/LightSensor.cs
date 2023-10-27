using Godot;

namespace SpookyBotanyGame.World.Entities.Properties
{
    [GlobalClass,Icon("res://World/Entities/icon-entity-property.svg")]
    public partial class LightSensor : EntityProperty
    {
        [Export] public CollisionObject2D HitBox { get; set; }
        
        public delegate void LightHit(LightEmissionZone lightEmissionZone, float lightPower);
        public event LightHit OnEnter;
        public event LightHit OnExit;
        public event LightHit OnApply;
        
        public override void _Ready()
        {
           
            base._Ready();
            _entity?.AddProperty(this);
        }

        public void LitBy(LightEmissionZone lightEmissionZone, float lightPower)
        {
            OnEnter?.Invoke(lightEmissionZone, lightPower);
        }

        public void UnlitBy(LightEmissionZone lightEmissionZone, float lightPower)
        {
            OnExit?.Invoke(lightEmissionZone, lightPower);
        }

        public void ApplyLight(LightEmissionZone lightEmissionZone, float lightAmount)
        {
            OnApply?.Invoke(lightEmissionZone, lightAmount);
        }
    }
}