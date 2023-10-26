using Godot;
using SpookyBotanyGame.World.Entities.Character;
using DiagonalAnimationPlayer = SpookyBotanyGame.World.Entities.Animation.DiagonalAnimationPlayer;

namespace SpookyBotanyGame.World.Entities.Properties
{
    [GlobalClass]
    public partial class PlayerInputControlled : EntityProperty
    {
        [Export] public float Speed { get; set; } = 100;
        [Export] private DiagonalAnimationPlayer Animation { get; set; }
        [Export] private CharacterBody2D Body2D { get; set; }


        public static Vector2 GetMoveStickAxis()
        {
            return Input.GetVector("move_east", "move_west", "move_north", "move_south");
        }
        
        public static Vector2 GetLookStickAxis()
        {
            return Input.GetVector("look_east", "look_west", "look_north", "look_south");
        }
        public void GetInput()
        {
            var inputDirection = GetMoveStickAxis();
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

