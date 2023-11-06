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

        private Tween _audioTween;
        private float _defaultVolDB;

        public override void _Ready()
        {
            base._Ready();
            if (SpawnActivationZone != null)
            {
                if (SafeZoneMusic != null)
                {
                    _defaultVolDB = SafeZoneMusic.VolumeDb;
                }
               
                var playerDetector = new EntityDetectionZone.Detector<PlayerEntity>();
                playerDetector.OnEnter = HandlePlayerEnterZone;
                playerDetector.OnExit = HandlePlayerExitZone;
                SpawnActivationZone.Set(playerDetector);
            }
        }

        private void HandlePlayerExitZone(PlayerEntity player)
        {
            _audioTween?.Kill();
            if (SafeZoneMusic != null)
            {
                _audioTween = CreateTween();
                _audioTween.TweenProperty(SafeZoneMusic, "volume_db", -80, 3f);
                _audioTween.TweenCallback( Callable.From(() => { SafeZoneMusic.Stop();}));//.SetDelay(3.0f);
            }
        }

        private void HandlePlayerEnterZone(PlayerEntity player)
        {
            _audioTween?.Kill();
            if (SafeZoneMusic != null)
            {
                
                _audioTween = CreateTween();
                _audioTween.TweenProperty(SafeZoneMusic, "volume_db", _defaultVolDB, 0.5f);
                SafeZoneMusic.Play();
            }
            player.SetSpawn(this);
        }

        public void Spawn(Node2D node, SpawnCompleted onComplete)
        {
            node.Position = this.Position;
            onComplete?.Invoke(this);
        }
    }
}