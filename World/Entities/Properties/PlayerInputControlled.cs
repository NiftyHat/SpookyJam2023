using Godot;
using SpookyBotanyGame.World.Entities.Character;

namespace SpookyBotanyGame.World.Entities.Properties
{
    public partial class PlayerInputControlled : EntityProperty
    {
        [Export] public float Speed { get; set; } = 100;
        [Export] private DiagonalAnimationPlayer Animation { get; set; }
        
        [Export] private CharacterBody2D Body2D { get; set; }

        public void GetInput()
        {
            Vector2 inputDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");
            Body2D.Velocity = inputDirection * Speed;
        
            if (inputDirection == Vector2.Zero)
            {
                Animation.Play(inputDirection,"idle");
            }
            else
            {
                Animation.Play(inputDirection, "walk");
            }
        }

        public void Enable()
        {
            SetPhysicsProcess(true);
        }

        public void Disable()
        {
            SetPhysicsProcess(false);
        }
    
        public override void _PhysicsProcess(double delta)
        {
            GetInput();
            Body2D.MoveAndSlide();
        }
    }
}

