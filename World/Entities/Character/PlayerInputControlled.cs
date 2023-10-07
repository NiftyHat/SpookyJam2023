using Godot;

namespace SpookyBotanyGame.World.Entities.Character;

public partial class PlayerInputControlled : CharacterBody2D
{
    [Export] public float Speed { get; set; } = 400;
    [Export] private FourDirectionAnimationController Animation { get; set; }

    public void GetInput()
    {
        Vector2 inputDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        Velocity = inputDirection * Speed;
        
        if (inputDirection == Vector2.Zero)
        {
            Animation.Play(inputDirection,"idle");
        }
        else
        {
            Animation.Play(inputDirection, "walk");
        }
    }
    
    public override void _PhysicsProcess(double delta)
    {
        GetInput();
        MoveAndSlide();
    }
}