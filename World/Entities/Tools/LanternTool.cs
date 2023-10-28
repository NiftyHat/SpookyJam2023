using Godot;
using SpookyBotanyGame.Core;
using SpookyBotanyGame.World.Entities.Animation;

namespace SpookyBotanyGame.World.Entities.Tools
{
    [GlobalClass]
    public partial class LanternTool : Node2D
    {
        [Export] public ToolAnimationPlayer Animation;

        [Export] public float FuelDrainPerSecond { get; set; } = 1;

        private Core.Range<float> _fuelLevel = new Range<float>(100,0, 50);
        private int _visibleFuelLevel;
        
        public override void _Process(double delta)
        {
            _fuelLevel.Value -= (float)delta * FuelDrainPerSecond;
            int newFuelLevel = Mathf.CeilToInt(Core.Range.Percentage(_fuelLevel) * 5);
            if (newFuelLevel != _visibleFuelLevel)
            {
                _visibleFuelLevel = newFuelLevel;
                Animation.Play(_visibleFuelLevel.ToString());
            }
            base._Process(delta);
        }

        public void Destroy()
        {
            Animation.PlayOneShot("Destroy", HandleDestroyAnimationComplete);
        }

        private void HandleDestroyAnimationComplete(StringName animName)
        {
            this.QueueFree();
        }
    }
}

