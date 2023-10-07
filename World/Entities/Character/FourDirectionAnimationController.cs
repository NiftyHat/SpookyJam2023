using Godot;

namespace SpookyBotanyGame.World.Entities.Character;

[GlobalClass]
public partial class FourDirectionAnimationController : Node2D
{
    private string _animationDirection = "down";
    private string _animationName = "idle";

    [Export] private AnimationPlayer Animation { get; set; }
    public string GetDirectionAnimationName(Vector2 inputDirection)
    {
        string animationDirectionName = _animationDirection;
        if (inputDirection.X > 0)
        {
            animationDirectionName = "right";
        }
        if (inputDirection.X < 0)
        {
            animationDirectionName = "left";
        }
        if (inputDirection.Y > 0)
        {
            animationDirectionName = "down";
        }

        if (inputDirection.Y < 0)
        {
            animationDirectionName = "up";
        }
        return animationDirectionName;
    }
    
    
    public void Play(Vector2 inputDirection, string animationName)
    {
        _animationDirection = GetDirectionAnimationName(inputDirection);
        Animation.Play(_animationDirection + "_" + animationName);
    }
    
}