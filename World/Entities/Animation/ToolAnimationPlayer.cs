using Godot;

namespace SpookyBotanyGame.World.Entities.Animation
{
    [GlobalClass]
    public partial class ToolAnimationPlayer : Node2D
    {
        private string _animationName = "1";
        
        private OneShotAnimationHandler _oneShotAnimationHandler = new OneShotAnimationHandler();
        private Direction _holdDirection = Direction.North;

        [Export] private AnimationPlayer Animation { get; set; }
        [Export] private Sprite2D Sprite { get; set; }

        public const string CarryAnimationName = "Carry";

        public override void _Ready()
        {
            base._Ready();
            _oneShotAnimationHandler.Animation = Animation;
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
        }

        public Direction SetVerticalAnimationDirection(Vector2 inputDirection)
        { 
            if (inputDirection.Y < 0)
            {
                _holdDirection = Direction.North;
                Sprite.ZIndex = 0;
            }
            if (inputDirection.Y > 0)
            {
                _holdDirection = Direction.South;
                Sprite.ZIndex = 3;
            }
            return _holdDirection;
        }
    
        public void Play(Vector2 inputDirection, string animationName)
        {
            SetVerticalAnimationDirection(inputDirection);
            SetFlipFromDirection(inputDirection);
            _animationName = animationName;
            string carriedAnimationName = GetCarriedAnimationName(animationName);
            Animation.Play(carriedAnimationName);
            Animation.Advance(0);
        }
        
        public void Play(string animationName)
        {
            string carriedAnimationName = GetCarriedAnimationName(animationName);
            GD.Print(carriedAnimationName);
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

        public void PlayOneShot(string animationName, AnimationPlayer.AnimationFinishedEventHandler callback)
        {
            string carriedAnimationName = GetCarriedAnimationName(animationName);
            _oneShotAnimationHandler.Add(carriedAnimationName, callback);
            Animation.Play(carriedAnimationName);
        }

        public void SetIdleState(int visibleFuelLevel)
        {
            throw new System.NotImplementedException();
        }
    }
}
