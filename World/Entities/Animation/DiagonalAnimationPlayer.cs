using Godot;
using System.Collections.Generic;

namespace SpookyBotanyGame.World.Entities.Character
{
    [GlobalClass]
    public partial class DiagonalAnimationPlayer : Node2D
    {
        private string _animationDirection = "south";
        private string _animationName = "idle";
        private OneShotAnimationHandler _oneShotAnimationHandler = new OneShotAnimationHandler();

        [Export] private AnimationPlayer Animation { get; set; }
        [Export] private Sprite2D Sprite { get; set; }

        public override void _Ready()
        {
            base._Ready();
            _oneShotAnimationHandler.Animation = Animation;
        }

        public string GetVerticalAnimationDirection(Vector2 inputDirection)
        {
            string animationDirectionName = _animationDirection;
            if (inputDirection.Y >= 0)
            {
                animationDirectionName = "south";
            }
            if (inputDirection.Y < 0)
            {
                animationDirectionName = "north";
            }
            return animationDirectionName;
        }
    
    
        public void Play(Vector2 inputDirection, string animationName)
        {
            string directionalAnimationName = GetDirectionalAnimationName(inputDirection, animationName);
            SetFlipFromDirection(inputDirection);
            Animation.Play(directionalAnimationName);
        }
        
        public void Play(string animationName)
        {
            string directionalAnimationName = GetDirectionalAnimationName(animationName);
            Animation.Play(directionalAnimationName);
        }

        private void SetFlipFromDirection(Vector2 inputDirection)
        {
            if (inputDirection.X > 0)
            {
                Sprite.FlipH = true;
            }
            if (inputDirection.X < 0)
            {
                Sprite.FlipH = false;
            }
        }

        private string GetDirectionalAnimationName(Vector2 inputDirection, string animationName)
        {
            _animationDirection = GetVerticalAnimationDirection(inputDirection);
            return animationName + "_" + _animationDirection;
        }
        
        private string GetDirectionalAnimationName(string animationName)
        {
            return animationName + "_" + _animationDirection;
        }

        public void PlayOneShot(string animationName, AnimationPlayer.AnimationFinishedEventHandler callback)
        {
            string directionalAnimationName = GetDirectionalAnimationName(animationName);
            _oneShotAnimationHandler.Add(directionalAnimationName, callback);
        }
    }
}
