using Godot;

namespace SpookyBotanyGame.World.Entities.Animation
{
    [GlobalClass]
    public partial class ToolAnimationPlayer : Node2D
    {
        private string _animationName = "1";
        
        private OneShotAnimationHandler _oneShotAnimationHandler = new OneShotAnimationHandler();
        
        [Export] private AnimationPlayer Animation { get; set; }
        [Export] private Sprite2D Sprite { get; set; }

        public const string CarryAnimationName = "Carry";

        public override void _Ready()
        {
            base._Ready();
            _oneShotAnimationHandler.Animation = Animation;
        }
        
        public void Play(Vector2 inputDirection, string animationName)
        {
            SetFlipFromDirection(inputDirection);
            _animationName = animationName;
            string carriedAnimationName = GetCarriedAnimationName(animationName);
            Animation.Play(carriedAnimationName);
            Animation.Advance(0);
        }
        
        public void Play(string animationName)
        {
            string carriedAnimationName = GetCarriedAnimationName(animationName);
            Animation.Play(carriedAnimationName);
        }

        public void Reset()
        {
            Animation.Play("RESET");
        }

        private void SetFlipFromDirection(Vector2 inputDirection)
        {
            if (inputDirection.X > 0)
            {
                Sprite.Position = Vector2.Right * 5;
                Sprite.FlipH = true;
            }
            if (inputDirection.X < 0)
            {
                Sprite.Position = Vector2.Left * 5;
                Sprite.FlipH = false;
            }
        }

        private string GetCarriedAnimationName(string animationName)
        {
            return CarryAnimationName + "-" + animationName;
        }

        public void PlayOneShot(string animationName, AnimationPlayer.AnimationFinishedEventHandler callback = null)
        {
            string carriedAnimationName = GetCarriedAnimationName(animationName);
            _oneShotAnimationHandler.Add(carriedAnimationName, callback);
            Animation.Play(carriedAnimationName);
        }
    }
}

