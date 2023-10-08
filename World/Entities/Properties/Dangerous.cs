using Godot;

namespace SpookyBotanyGame.World.Entities.Properties
{
    [GlobalClass]
    public partial class Dangerous : EntityProperty
    {
        [Export] public Area2D HitBox { get; set; }
        public void Kill(Killable entity)
        {
            entity.Kill();
        }

        public override void _Ready()
        {
            base._Ready();
            HitBox.Monitoring = true;
            HitBox.AreaEntered += HandleAreaEntered;
            HitBox.BodyEntered += HandleBodyEntered;
        }

        private void HandleBodyEntered(Node2D body)
        {
            GD.Print($"Dangerous Entity intersected with {body}");
        }

        private void HandleAreaEntered(Area2D area)
        {
            GD.Print($"Dangerous Entity intersected with {area.GetParent().Name}");
        }
    }
}
