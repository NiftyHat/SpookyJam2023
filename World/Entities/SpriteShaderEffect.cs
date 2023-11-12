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
        [Export] public AnimationPlayer Animation { get; set; }

        private ShaderMaterial _effectMaterial;
        private Material _defaultMaterial;

        public Func<ShaderMaterial, Tween> TweenIn;
        public Func<ShaderMaterial, Tween> TweenOut;

        private Tween _tween;

        [Export] public bool IsEnabled { get; private set; } = true;

        public override void _Ready()
        {
            if (Animation != null)
            {
                GD.PrintErr(Name, "Can't use Tweens and Animation at the same time");
                TweenIn = null;
                TweenOut = null;
            }
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
            GD.Print("ClearShader");
            Sprite.Material = _defaultMaterial;
        }

        public void AnimateIn()
        {
            if (Animation.HasAnimation("In"))
            {
                Animation.Play("In");
                if (Animation.HasAnimation("Loop") && Animation.GetAnimation("In").LoopMode == Godot.Animation.LoopModeEnum.None)
                {
                    Animation.Queue("Loop");
                }
            }
            else if (Animation.HasAnimation("Loop"))
            {
                Animation.Queue("Loop");
            }
            else
            {
                if (this.IsInsideTree())
                {
                    GD.PrintErr($" {this.GetPath()} Missing animation name Loop");
                }
                else
                {
                    GD.PrintErr($" {this.Name} Missing animation name Loop");
                }
            }
        }
        
        public void AnimateOut()
        {
            Animation.Play("Out");

            void ClearOnAnimationFinished(StringName animationName)
            {
                Animation.AnimationFinished -= ClearOnAnimationFinished;
                if (animationName == "Out")
                {
                    ClearShader();
                }
            }
            Animation.AnimationFinished += ClearOnAnimationFinished;
        }

        public void SetEnabled(bool isEnabled)
        {
            if (IsEnabled != isEnabled)
            {
                IsEnabled = isEnabled;
                if (isEnabled)
                {
                    ApplyShader();
                    if (Animation != null)
                    {
                        AnimateIn();
                    }
                    else if (TweenIn != null)
                    {
                        _tween?.Kill();
                        _tween = TweenIn(_effectMaterial);
                    }
                }
                else
                {
                    if (Animation != null)
                    {
                        AnimateOut();
                    }
                    else if (TweenOut != null)
                    {
                        _tween?.Kill();
                        _tween = TweenOut(_effectMaterial);
                        _tween.TweenCallback(Callable.From(ClearShader)).SetDelay(1.0f);
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