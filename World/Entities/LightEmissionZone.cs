using Godot;
using Godot.Collections;
using SpookyBotanyGame.World.Entities.Properties;

namespace SpookyBotanyGame.World.Entities
{
    [GlobalClass]
    public partial class LightEmissionZone : Node2D
    {
        [Export] public Area2D HitBox { get; set; }
        
        private Dictionary<Node2D, LightSensor> _trackedEntities = new Dictionary<Node2D, LightSensor>();

        public bool IsEnabled { get; private set; } = true;

        public void Light(Killable entity)
        {
            entity.Kill();
        }

        public override void _Ready()
        {
            base._Ready();
            HitBox.Monitoring = true;
            HitBox.BodyEntered += HandleEntered;
            HitBox.BodyExited += HandleExited;
            HitBox.AreaEntered += HandleEntered;
            HitBox.AreaExited += HandleExited;
        }
        
        public float GetLightAmount()
        {
            return 1;
        }
        
        private void HandleEntered(Node2D area)
        {
            if (GameEntity.TryGetProperty(area, out LightSensor lightSensor, out GameEntity gameEntity))
            {
                float lightAmount = GetLightAmount();
                Add(lightSensor, gameEntity);
                lightSensor.LitBy(this, lightAmount);
            }
        }
        
        private void HandleExited(Node2D area)
        {
            if (GameEntity.TryGetProperty(area, out LightSensor lightSensor, out GameEntity gameEntity))
            {
                float lightAmount = GetLightAmount();
                lightSensor.UnlitBy(this, lightAmount);
                Remove(gameEntity);
            }
        }
        
        private void Add(LightSensor interactable, GameEntity gameEntity)
        {
            if (!_trackedEntities.ContainsKey(gameEntity))
            {
                _trackedEntities.Add(gameEntity, interactable);
            }
        }

        private void Remove(GameEntity gameEntity)
        {
            if (_trackedEntities.ContainsKey(gameEntity))
            {
                _trackedEntities.Remove(gameEntity);
            }
        }
        
        public override void _Process(double delta)
        {
            base._Process(delta);
            if (!IsEnabled)
            {
                return;
            }
            if (_trackedEntities.Count > 0)
            {
                foreach (var kvp in _trackedEntities)
                {
                    float lightAmount = GetLightAmount();
                    kvp.Value.ApplyLight(this, lightAmount);
                }
            }
        }

        public void SetEnabled(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }
    }
}