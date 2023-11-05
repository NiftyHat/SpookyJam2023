using Godot;
using SpookyBotanyGame.World.Entities;

namespace SpookyBotanyGame.World
{
    public partial class SpawnPoint : Node2D
    {
        public delegate void SpawnCompleted(SpawnPoint spawnPoint);
        [Export] public EntityDetectionZone SpawnActivationZone { get; set; }
        [Export] public AudioStreamPlayer2D SafeZoneMusic { get; set; }
        protected PackedScene _spawnedObject { get; set; }

        public override void _Ready()
        {
            base._Ready();
            if (SpawnActivationZone != null)
            {
                var playerDetector = new EntityDetectionZone.Detector<PlayerEntity>();
                playerDetector.OnEnter = HandlePlayerEnterZone;
                playerDetector.OnExit = HandlePlayerExitZone;
                SpawnActivationZone.Set(playerDetector);
            }
        }

        private void HandlePlayerExitZone(PlayerEntity player)
        {
        }

        private void HandlePlayerEnterZone(PlayerEntity player)
        {
            player.SetSpawn(this);
        }

        public void Spawn(Node2D node, SpawnCompleted onComplete)
        {
            node.Position = this.Position;
            onComplete?.Invoke(this);
        }
    }
}