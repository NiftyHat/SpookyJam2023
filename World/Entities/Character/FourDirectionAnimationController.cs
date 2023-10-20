using Godot;

namespace SpookyBotanyGame.World.Entities.Character
{
    public partial class FourDirectionAnimationController : Node2D
    {
        public override void _Ready()
        {
            GD.Print(this.Name);
            GD.Print(this.GetParent().Name);
            base._Ready();
        }
    }
}

