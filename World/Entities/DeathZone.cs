using Godot;
using SpookyBotanyGame.World.Entities.Properties;

namespace SpookyBotanyGame.World.Entities
{
    public partial class DeathZone : Node2D
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
            HitBox.BodyEntered += HandleBodyEntered;
        }

        private void HandleBodyEntered(Node2D body)
        {
            if (body.IsInGroup("EntityProvider") && body.HasMethod("GetEntity"))
            {
                GameEntity gameEntity = body.Call("GetEntity").As<GameEntity>();
                if (gameEntity != null && gameEntity.TryGetProperty(out Killable killable))
                {
                    Kill(killable);
                }
            }
        }
    }
}