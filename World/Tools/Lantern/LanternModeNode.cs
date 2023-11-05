using Godot;
using SpookyBotanyGame.Core;
using SpookyBotanyGame.World.Entities;

namespace SpookyBotanyGame.World.Tools.Lantern
{
    [GlobalClass]
    public partial class LanternModeNode : Node2D, IEnabled
    {
        private bool _isEnabled;
        [Export] public float FuelBurnRate { get; set; }
        [Export] public LightEmissionZone[] LightEmissionZones { get; set; }
        [Export] public LanternPointLight LanternBeamLight { get; set; }

        public bool IsEnabled => _isEnabled;

        public void UpdateFuelLevel(int fuelLevel, float fuelPercentage)
        {
            float energy = 0.4f + 0.6f * fuelLevel;
            float scale = 0.8f + 0.2f * fuelLevel;
            LanternBeamLight.SetEnergy(energy);
            LanternBeamLight.SetScale(scale);
            //AmbientLight.Energy = 0.05f + fuelPerc * 0.4f;
            //AmbientLight.TextureScale = _pointLightDefaultScale + (fuelPerc * 0.3f);
        }
        
        public void SetEnabled(bool isEnabled)
        {
            _isEnabled = isEnabled;
            Visible = isEnabled;
            foreach (var emissionZones in LightEmissionZones)
            {
                emissionZones.SetEnabled(isEnabled);
            }
        }
    }
}