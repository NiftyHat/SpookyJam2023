using System.Diagnostics;
using System.Drawing;
using Godot;
using SpookyBotanyGame.Core;
using SpookyBotanyGame.World.Entities.Animation;

namespace SpookyBotanyGame.World.Entities.Tools
{
    [GlobalClass]
    public partial class LanternTool : Node2D
    {
        public delegate void OnEmptyChanged(bool isEmpty, LanternTool tool);
        
        [Export] public ToolAnimationPlayer Animation;

        [Export] public float FuelDrainPerSecond { get; set; } = 1;
        
        [Export] public PointLight2D AmbientLight { get; set; }
        
        [Export] public LanternPointLight PointLight { get; set; }
        
        [Export] public InputDirectionalPointing Pointing { get; set; }
        
        [Export] public LightEmissionZone LightZone { get; set; }
        
        public Range<float> Fuel { get; private set; }  = new Range<float>(100,0, 50);

        //private Core.Range<float> _fuelLevel = new Range<float>(100,0, 50);
        private int _visibleFuelLevel;
        private Vector2 _pointing;
        private float _pointLightDefaultScale;
        private bool _isEmpty;
        private bool _isDestroying;

        private OnEmptyChanged OnEmptyChange;

        public override void _Ready()
        {
            _pointLightDefaultScale = AmbientLight.TextureScale;
            Pointing.OnDirectionChanged += HandlePointingDirectionChanged;
            _isEmpty = Fuel.IsZero;
            base._Ready();
        }

        private void HandlePointingDirectionChanged(Vector2 direction, Vector2 position)
        {
            SetPointing(direction);
        }

        public override void _Process(double delta)
        {
            if (_isDestroying)
            {
                return;
            }
            Fuel.Value -= (float)delta * FuelDrainPerSecond;
            if (Fuel.IsMin)
            {
                if (!_isEmpty)
                {
                    _isEmpty = true;
                    OnEmptyChange?.Invoke(_isEmpty, this);
                    PointLight.SetEnergy(0.1f);
                    PointLight.SetScale(0.3f);
                    AmbientLight.Energy = 0;
                    AmbientLight.TextureScale = 0;
                    LightZone.SetEnabled(false);
                }
                Animation.Play(_pointing,"0");
            }
            else
            {
                if (_isEmpty)
                {
                    _isEmpty = false;
                    OnEmptyChange?.Invoke(_isEmpty, this);
                    PointLight.Reset();
                    LightZone.SetEnabled(true);
                }
                float fuelPerc = Core.Range.Percentage(Fuel);
                int newFuelLevel = Mathf.CeilToInt(Core.Range.Percentage(Fuel) * 5);
                if (newFuelLevel != _visibleFuelLevel)
                {
                    _visibleFuelLevel = newFuelLevel;
                }
                Animation.Play(_pointing,_visibleFuelLevel.ToString());
                AmbientLight.Energy = 0.05f + fuelPerc * 0.4f;
                AmbientLight.TextureScale = _pointLightDefaultScale + (fuelPerc * 0.3f);
            }
            base._Process(delta);
        }

        public void Destroy()
        {
            _isDestroying = true;
            Animation.PlayOneShot("Destroy", HandleDestroyAnimationComplete);
        }

        private void HandleDestroyAnimationComplete(StringName animName)
        {
            _isDestroying = false;
            _isEmpty = !_isEmpty;
        }

        public void SetPointing(Vector2 direction)
        {
            _pointing = direction;
        }
    }
}

