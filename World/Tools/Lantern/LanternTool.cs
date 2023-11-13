using Godot;
using SpookyBotanyGame.Core;
using SpookyBotanyGame.World.Entities;
using SpookyBotanyGame.World.Entities.Animation;

namespace SpookyBotanyGame.World.Tools.Lantern
{
    [GlobalClass]
    public partial class LanternTool : Node2D, IEnabled
    {
        public delegate void OnEmptyChanged(bool isEmpty, LanternTool tool);
        
        [Export] public ToolAnimationPlayer ToolAnimation;
        [Export] public InputDirectionalPointing Pointing { get; set; }
        [Export] public LanternModeNode ModeHigh { get; set; }
        [Export] public LanternModeNode ModeMedium { get; set; }
        [Export] public LanternModeNode ModeLow { get; set; }
        
        public Range<float> Fuel { get; private set; }  = new Range<float>(300,0, 300);

        public bool IsEmpty => _isEmpty;

        //private Core.Range<float> _fuelLevel = new Range<float>(100,0, 50);
        private int _visibleFuelLevel;
        private Vector2 _pointing;
        private bool _isEmpty;
        private bool _isDestroying;
        private LanternModeNode _lanternMode;

        public event OnEmptyChanged OnEmptyChange;

        public override void _Ready()
        {
            Pointing.OnDirectionChanged += HandlePointingDirectionChanged;
            _isEmpty = Fuel.IsZero;
            ModeLow.SetEnabled(false);
            ModeHigh.SetEnabled(false);
            SetMode(ModeMedium);
            base._Ready();
        }

        private void HandlePointingDirectionChanged(Vector2 direction, Vector2 position)
        {
            SetPointing(direction);
        }

        public override void _Process(double delta)
        {
            if (_isDestroying || _lanternMode == null || IsEnabled == false)
            {
                return;
            }
            Fuel.Value -= (float)delta * _lanternMode.FuelBurnRate;
            if (!_isEmpty && Fuel.Value <= 0)
            {
                OnEmptyChange?.Invoke(true, this);
                SetMode(null);
                _isEmpty = true;
                ToolAnimation.Play(_pointing,"0");
                return;
            }
            if (_isEmpty && Fuel.Value > 0)
            {
                _isEmpty = false;
                OnEmptyChange?.Invoke(false, this);
                SetMode(ModeLow);
            }
            float fuelPercentage = Core.Range.Percentage(Fuel);
            int newFuelLevel = Mathf.CeilToInt(Core.Range.Percentage(Fuel) * 5);
            
            if (_visibleFuelLevel != newFuelLevel)
            {
               
                //_lanternMode.UpdateFuelLevel(_visibleFuelLevel,fuelPercentage);
                _visibleFuelLevel = newFuelLevel;
            }
            ToolAnimation.Play(_pointing,_visibleFuelLevel.ToString());
            base._Process(delta);
        }

        public void SetMode(LanternModeNode mode)
        {
            if (_lanternMode == mode)
            {
                return;
            }
            if (_lanternMode != null)
            {
                _lanternMode.SetEnabled(false);
                _lanternMode = null;
            }
            _lanternMode = mode;
            if (_lanternMode != null)
            {
                _lanternMode.SetEnabled(true);
                Pointing.Controlled = _lanternMode;
            }
        }

        public void Destroy()
        {
            _lanternMode = null;
            _isDestroying = true;
            ToolAnimation.PlayOneShot("Destroy", HandleDestroyAnimationComplete);
        }

        private void HandleDestroyAnimationComplete(StringName animName)
        {
            _isDestroying = false;
        }

        public void SetPointing(Vector2 direction)
        {
            if (IsEnabled == false)
            {
                return;
            }
            _pointing = direction;
        }

        public void Disable()
        {
            SetEnabled(false);
        }
        
        public void Enable()
        {
            SetEnabled(true);
        }

        public bool IsEnabled { get; private set; } = true;

        public void SetEnabled(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }
    }
}

