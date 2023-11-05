using Godot;
using SpookyBotanyGame.Collectable;
using SpookyBotanyGame.World.Entities.Animation;
using SpookyBotanyGame.World.Entities.Collision;
using SpookyBotanyGame.World.Entities.Properties;
using SpookyBotanyGame.World.Tools.Lantern;

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
        [Export] public SimController Sim { get; set; }
        [Export] public CollectableContainer Inventory { get; set; }
        [Export] public LanternTool LanternTool { get; set; }
        [Export] public Sprite2D CarriedSlotIcon { get; set; }
        
        public CollectableStackSlot<CollectableResource> CarriedSlot { get; set; } =  new CollectableStackSlot<CollectableResource>(0, 1);

        private SpawnPoint _spawnPoint;

        public override void _Ready()
        {
            base._Ready();
            Killable.OnKilled += HandleKilled;
            Killable.OnRespawned += HandleRespawned;
            CarriedSlot.OnChanged += HandleCarriedChanged;
            _properties.Add(Killable);
            CallDeferred("AddSpawnPointToParent");
        }

        private void HandleCarriedChanged(int newValue, int oldValue)
        {
            if (newValue > 0 && CarriedSlot.CollectableType != null)
            {
                if (CarriedSlotIcon.Visible == false)
                {
                    CarriedSlotIcon.Visible = true;
                }
                CarriedSlotIcon.Texture = CarriedSlot.CollectableType.CarriedTexture; 
            }
            else if (newValue == 0 || CarriedSlot.CollectableType == null)
            {
                CarriedSlotIcon.Texture = null;
                CarriedSlotIcon.Visible = false;
            }
        }

        private void HandleLanternEmptyChanged(bool isEmpty, LanternTool tool)
        {
            if (isEmpty)
            {
                
            }
        }

        private void HandleLanternDirectionChange(Vector2 direction, Vector2 distance)
        {
            LanternTool.SetPointing(direction);
        }

        public override void _Process(double delta)
        {
            if (Input.IsActionJustReleased("interact"))
            {
                Interact?.DoInteract();
            }

            if (!LanternTool.IsEmpty)
            {
                if (Input.IsActionPressed("lantern_primary"))
                {
                    LanternTool.SetMode(LanternTool.ModeHigh);
                }
                else if (Input.IsActionPressed("lantern_secondary"))
                {
                    LanternTool.SetMode(LanternTool.ModeLow);
                }
                else 
                {
                    LanternTool.SetMode(LanternTool.ModeMedium);
                }
            }
            
            
            base._Process(delta);
        }
        
        private void AddSpawnPointToParent()
        {
            _spawnPoint = new SpawnPoint();
            _spawnPoint.Position = Body.Position;
            var parent = Body.GetParent();
            if (parent != null)
            {
                parent.AddChild(_spawnPoint);
            }
        }

        private void HandleKilled()
        {
            InputControlled.Disable();
            Interact.Disable();
            LanternTool.Destroy();
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
            Sim.PlayerRespawn(this);
            Animation.Reset();
        }

        public void SetSpawn(SpawnPoint spawnPoint)
        {
            _spawnPoint = spawnPoint;
        }
    }
}