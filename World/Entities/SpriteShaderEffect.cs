using System;
using Godot;
using SpookyBotanyGame.Core;

namespace SpookyBotanyGame.World.Entities
{
    [GlobalClass]
    public partial class SpriteShaderEffect : Node, IEnabled
    {
        [Export] public Sprite2D Sprite { get; set; }
        [Export] public ShaderMaterial Material { get; set; }

        private ShaderMaterial _effectMaterial;
        private Material _defaultMaterial;

        public Func<ShaderMaterial, Tween> TweenIn;
        public Func<ShaderMaterial, Tween> TweenOut;
        //public Func<Tween> TweenIn;
        //public Func<Tween> TweenOut;

        private Tween _tween;

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
                    if (TweenIn != null)
                    {
                        _tween?.Kill();
                        _tween = TweenIn(_effectMaterial);
                    }
                }
                else
                {
                    if (TweenOut != null)
                    {
                        _tween?.Kill();
                        _tween = TweenOut(_effectMaterial);
                        _tween.TweenCallback(Callable.From(ClearShader));
                    }
                    else
                    {
                        ClearShader();
                    }
                }
            }
        }
    }
}