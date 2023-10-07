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

        public string SetVerticalAnimationDirection(Vector2 inputDirection)
        {
            GD.Print(inputDirection);
            if (inputDirection.Y < 0)
            {
                _animationDirection = "north";
            }
            if (inputDirection.Y > 0)
            {
                _animationDirection = "south";
            }
            return _animationDirection;
        }
    
    
        public void Play(Vector2 inputDirection, string animationName)
        {
            SetVerticalAnimationDirection(inputDirection);
            SetFlipFromDirection(inputDirection);
            string directionalAnimationName = GetDirectionalAnimationName(animationName);
            GD.Print(directionalAnimationName);
            Animation.Play(directionalAnimationName);
        }
        
        public void Play(string animationName)
        {
            string directionalAnimationName = GetDirectionalAnimationName(animationName);
            Animation.Play(directionalAnimationName);
        }

        public void Reset()
        {
            Animation.Play("RESET");
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

        private string GetDirectionalAnimationName(string directionName, string animationName)
        {
            return animationName + "_" + directionName;
        }
        private string GetDirectionalAnimationName(string animationName)
        {
            return animationName + "_" + _animationDirection;
        }

        public void PlayOneShot(string animationName, AnimationPlayer.AnimationFinishedEventHandler callback)
        {
            string directionalAnimationName = GetDirectionalAnimationName(animationName);
            _oneShotAnimationHandler.Add(directionalAnimationName, callback);
            Animation.Play(directionalAnimationName);
        }
    }
}
