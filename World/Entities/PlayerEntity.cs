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

        private World.SpawnPoint _spawnPoint;

        public override void _Ready()
        {
            base._Ready();
            Killable.OnKilled += HandleKilled;
            _properties.Add(Killable);
        }

        public override void _EnterTree()
        {
            base._EnterTree();
            AddSpawnPointToParent();
        }

        private void AddSpawnPointToParent()
        {
            _spawnPoint = new World.SpawnPoint();
            _spawnPoint.Position = this.Position;
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

        private void HandleRespawn(SpawnPoint spawnPoint)
        {
            Killable.Spawn();
        }
    }
}