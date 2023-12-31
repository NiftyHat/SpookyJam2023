using Godot;

namespace SpookyBotanyGame.World.Entities.Properties
{
    [GlobalClass,Icon("res://World/Entities/icon-entity-property.svg")]
    public partial class Killable : EntityProperty
    {
        [Export] public CollisionObject2D HurtBox { get; set; }
        
        public delegate void Killed();
        public delegate void Respawned(SpawnPoint spawnPoint);

        public event Killed OnKilled;
        public event Respawned OnRespawned;
        private bool _isAlive = true;


        public override void _Ready()
        {
            _entity?.AddProperty(this);
            base._Ready();
        }

        public void Kill()
        {
            if (!IsEnabled)
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