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

        [Export] private Sprite2D HolderSprite { get; set; }

        public const string CarryAnimationName = "Carry-";

        public override void _Ready()
        {
            base._Ready();
            _oneShotAnimationHandler.Animation = Animation;
        }

        public override void _Process(double delta)
        {
            if (Sprite.GlobalPosition.Y > HolderSprite.GlobalPosition.Y)
            {
                Sprite.ZIndex =  HolderSprite.ZIndex -1;
            }
            else
            {
                Sprite.ZIndex = HolderSprite.ZIndex;
            }
            base._Process(delta);
        }

        public Direction SetVerticalAnimationDirection(Vector2 inputDirection)
        { 
            if (inputDirection.Y < 0)
            {
                _holdDirection = Direction.North;
            }
            if (inputDirection.Y > 0)
            {
                _holdDirection = Direction.South;
            }
            return _holdDirection;
        }
    
        public void Play(Vector2 inputDirection, string animationName)
        {
            SetVerticalAnimationDirection(inputDirection);
            SetFlipFromDirection(inputDirection);
            string carriedAnimationName = GetCarriedAnimationName(_animationName);
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
                Sprite.FlipH = true;
            }
            if (inputDirection.X < 0)
            {
                Sprite.FlipH = false;
            }
        }

        private string GetCarriedAnimationName(string animationName)
        {
            return CarryAnimationName + "_" + animationName;
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

