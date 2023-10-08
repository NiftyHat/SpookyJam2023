using Godot;
using SpookyBotanyGame.World.Entities.Character;
using SpookyBotanyGame.World.Entities.Properties;
using PlayerInputControlled = SpookyBotanyGame.World.Entities.Properties.PlayerInputControlled;

namespace SpookyBotanyGame.World.Entities
{
    [GlobalClass]
    public partial class PlayerEntity : GameEntity
    {
        [Export] public Killable Killable { get; set; }
        [Export] public DiagonalAnimationPlayer Animation { get; set; }
        [Export] public PlayerInputControlled InputControlled { get; set; }

        private SpawnPoint _spawnPoint;

        public override void _Ready()
        {
            base._Ready();
            Killable.OnKilled += HandleKilled;
            AddSpawnPointToParent();
        }

        private void AddSpawnPointToParent()
        {
            _spawnPoint = new SpawnPoint();
            _spawnPoint.Position = this.Position;
            GetParent().AddChild(_spawnPoint);
        }

        private void HandleKilled()
        {
            InputControlled.Disable();
            Animation.PlayOneShot("death", HandleDeathAnimationComplete);
        }

        private void HandleDeathAnimationComplete(StringName animName)
        {
            _spawnPoint.Spawn(this, HandleRespawn);
            Animation.Play("RESET");
        }

        private void HandleRespawn(SpawnPoint spawnpoint)
        {
            Killable.Spawn();
        }
    }
}