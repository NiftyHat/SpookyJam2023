using Godot;
using SpookyBotanyGame.World.Entities.Properties;

namespace SpookyBotanyGame.World.Entities
{
    [GlobalClass]
    public partial class DeathZone : Area2D
    {
        public bool IsEnabled { get; private set; } = true;

        public void SetEnabled(bool isEnabled)
        {
            if (IsEnabled != isEnabled)
            {
                IsEnabled = isEnabled;
            }
            //TODO - disable monitoring when enabled is false.
            Monitoring = IsEnabled;
        }
        
        public void Kill(Killable entity)
        {
            entity.Kill();
        }

        public override void _Ready()
        {
            base._Ready();
            Monitoring = true;
            BodyEntered += HandleBodyEntered;
        }

        private void HandleBodyEntered(Node2D body)
        {
            if (!IsEnabled)
            {
                return;
            }
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