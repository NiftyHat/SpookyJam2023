using Godot;
using SpookyBotanyGame.Core;

namespace SpookyBotanyGame.World.Entities
{
    [GlobalClass]
    public partial class SpriteShaderEffect : Node, IEnabled
    {
        [Export] public Sprite2D Sprite { get; set; }
        [Export] public ShaderMaterial Material { get; set; }

        private Material _effectMaterial;
        private Material _defaultMaterial;

        [Export] public bool IsEnabled { get; private set; } = true;

        public override void _Ready()
        {
            if (Sprite != null)
            {
                _defaultMaterial = Sprite.Material;
            }
            if (IsEnabled)
            {
                SetEnabled(true);
            }
            base._Ready();
        }

        public void Enable()
        {
            SetEnabled(true);
        }

        public void Disable()
        {
            SetEnabled(false);
        }

        private void ApplyShader()
        {
            if (_effectMaterial == null)
            {
                _effectMaterial = Material.Duplicate() as ShaderMaterial;
            }
            Sprite.Material = _effectMaterial;
        }

        private void ClearShader()
        {
            Sprite.Material = _defaultMaterial;
        }

        public void SetEnabled(bool isEnabled)
        {
            if (IsEnabled != isEnabled)
            {
                IsEnabled = isEnabled;
                if (isEnabled)
                {
                    ApplyShader();
                }
                else
                {
                    ClearShader();
                }
            }
        }
    }
}