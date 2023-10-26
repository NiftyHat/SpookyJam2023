using Godot;

namespace SpookyBotanyGame.World.Entities.Properties
{
    [GlobalClass,Icon("res://World/Entities/icon-entity-property.svg")]
    public partial class LightSensor : EntityProperty
    {
        [Export] public CollisionObject2D LightBox { get; set; }
        
        public delegate void LightHit(LightZone lightZone, float lightPower);
        public event LightHit OnEnter;
        public event LightHit OnExit;
        public event LightHit OnApply;
        
        public override void _Ready()
        {
            _entity?.AddProperty(this);
            base._Ready();
        }

        public void LitBy(LightZone lightZone, float lightPower)
        {
            OnEnter?.Invoke(lightZone, lightPower);
        }

        public void UnlitBy(LightZone lightZone, float lightPower)
        {
            OnExit?.Invoke(lightZone, lightPower);
        }

        public void ApplyLight(LightZone lightZone, float lightAmount)
        {
            OnApply?.Invoke(lightZone, lightAmount);
        }
    }
}