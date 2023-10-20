using Godot;
using SpookyBotanyGame.World.Entities.Character;
using SpookyBotanyGame.World.Entities.Collision;
using SpookyBotanyGame.World.Entities.Properties;
using DiagonalAnimationPlayer = SpookyBotanyGame.World.Entities.Animation.DiagonalAnimationPlayer;
using PlayerInputControlled = SpookyBotanyGame.World.Entities.Properties.PlayerInputControlled;

namespace SpookyBotanyGame.World.Entities
{
    [GlobalClass]
    public partial class PlayerEntity : GameEntity
    {
        [Export] public Killable Killable { get; set; }
        [Export] public DiagonalAnimationPlayer Animation { get; set; }
        [Export] public PlayerInputControlled InputControlled { get; set; }
        [Export] public EntityCharacterBody2D Body { get; set; }
        [Export] public InteractionInputDirectional Interact { get; set; }

        private World.SpawnPoint _spawnPoint;

        public override void _Ready()
        {
            base._Ready();
            Killable.OnKilled += HandleKilled;
            Killable.OnRespawned += HandleRespawned;
            _properties.Add(Killable);
            
            //AddSpawnPointToParent();
        }

        private void AddSpawnPointToParent()
        {
            _spawnPoint = new SpawnPoint();
            _spawnPoint.Position = Body.Position;
            var parent = Body.GetParent();
            parent.AddChild(_spawnPoint);
        }

        private void HandleKilled()
        {
            InputControlled.Disable();
            Interact.Disable();
            Animation.PlayOneShot("death", HandleDeathAnimationComplete);
        }
        
        private void HandleRespawned(SpawnPoint spawnPoint)
        {
            InputControlled.Enable();
            Interact.Enable();
        }

        private void HandleDeathAnimationComplete(StringName animName)
        {
            _spawnPoint.Spawn(Body, Killable.Spawn);
            Animation.Reset();
        }
    }
}