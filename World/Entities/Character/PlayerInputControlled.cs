using Godot;

public partial class PlayerInputControlled : CharacterBody2D
{

    [Export] public float Speed { get; set; } = 400;
    [Export] private AnimationPlayer Animation { get; set; }

    private string _animationDirection = "down";

    public void GetInput()
    {
        Vector2 inputDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        if (inputDirection.X > 0)
        {
            _animationDirection = "right";
        }

        if (inputDirection.X < 0)
        {
            _animationDirection = "left";
        }

        if (inputDirection.Y > 0)
        {
            _animationDirection = "down";
        }

        if (inputDirection.Y < 0)
        {
            _animationDirection = "up";
        }
        Velocity = inputDirection * Speed;

        if (inputDirection == Vector2.Zero)
        {
            Animation.Play(_animationDirection + "_idle");
            Animation.Advance(0);
        }
        else
        {
            Animation.Play(_animationDirection + "_walk");
            Animation.Advance(0);
        }
    }
    
    public override void _PhysicsProcess(double delta)
    {
        GetInput();
        MoveAndSlide();
    }
}
