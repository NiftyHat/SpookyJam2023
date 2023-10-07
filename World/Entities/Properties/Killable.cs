using Godot;

namespace SpookyBotanyGame.World.Entities.Properties
{
    [GlobalClass]
    public partial class Killable : EntityProperty
    {
        [Export] public CollisionObject2D HurtBox { get; set; }
        
        public delegate void Killed();
        public delegate void Respawned(SpawnPoint spawnPoint);

        public event Killed OnKilled;
        public event Respawned OnRespawned;
        private bool _isAlive = true;
        
        public void Kill()
        {
            if (!_isEnabled)
            {
                return;
            }
            if (!_isAlive)
            {
                return;
            }
            OnKilled?.Invoke();
            _isAlive = false;
        }

        public void Spawn()
        {
            _isAlive = true;
        }
        
        public void Spawn(SpawnPoint spawnPoint)
        {
            _isAlive = true;
            OnRespawned?.Invoke(spawnPoint);
        }
    }
}