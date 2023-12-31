using System;
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
        [Export] public CollisionShape2D Collision { get; set; }
        [Export] public InteractionInputDirectional Interact { get; set; }
        [Export] public SimController Sim { get; set; }
        [Export] public CollectableContainer Inventory { get; set; }
        [Export] public LanternTool LanternTool { get; set; }
        [Export] public Sprite2D CarriedSlotIcon { get; set; }
        
        public CollectableStackSlot<CollectableResource> CarriedEyeSlot { get; set; } =  new CollectableStackSlot<CollectableResource>(0, 1);
        public CollectableStackSlot<CollectableResource> CarriedSeedSlot { get; set; } =  new CollectableStackSlot<CollectableResource>(0, 5);

        private SpawnPoint _spawnPoint;
        private bool _isSleeping;

        public override void _Ready()
        {
            base._Ready();
            Killable.OnKilled += HandleKilled;
            Killable.OnRespawned += HandleRespawned;
            CarriedEyeSlot.OnChanged += HandleCarriedChanged;
            CarriedSeedSlot.OnChanged += HandleCarriedChanged;
            _properties.Add(Killable);
            CallDeferred("AddSpawnPointToParent");
        }

        public CollectableStackSlot<CollectableResource> GetFirstActiveSeedSlot()
        {
            if (CarriedEyeSlot != null && CarriedEyeSlot.Amount > 0)
            {
                return CarriedEyeSlot;
            }

            if (CarriedSeedSlot != null && CarriedSeedSlot.Amount > 0)
            {
                return CarriedSeedSlot;
            }

            return null;
        }

        private void HandleCarriedChanged(int newValue, int oldValue)
        {
            var activeSlot = GetFirstActiveSeedSlot();
            if (activeSlot != null && activeSlot.CollectableType != null)
            {
                if (CarriedSlotIcon.Visible == false)
                {
                    CarriedSlotIcon.Visible = true;
                }
                CarriedSlotIcon.Texture = activeSlot.CollectableType.CarriedTexture; 
            }
            else
            {
                CarriedSlotIcon.Texture = null;
                CarriedSlotIcon.Visible = false;
            }
        }

        private void HandleLanternDirectionChange(Vector2 direction, Vector2 distance)
        {
            LanternTool.SetPointing(direction);
        }

        public override void _Process(double delta)
        {
            if (_isSleeping)
            {
                return;
            }
            if (Input.IsActionJustReleased("interact"))
            {
                Interact?.DoInteract();
            }

            if (!LanternTool.IsEmpty && LanternTool.IsEnabled)
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

            if (OS.IsDebugBuild())
            {
                if (Input.IsActionJustPressed("toggle_collision"))
                {
                    Collision.Disabled = true;
                    //ody
                }
                else if (Input.IsActionJustReleased("toggle_collision"))
                {
                    Collision.Disabled = false;
                    //ody
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
            Wake();
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

        public void Rest()
        {
            _isSleeping = true;
            InputControlled.Disable();
            Interact.Disable();
            LanternTool.SetMode(LanternTool.ModeLow);
            LanternTool.Disable();
            Animation.Play("rest");
            Sim.PlayerRest(this);
            Sim.DayStartEvents.Completed += Wake;
        }

        public void Wake()
        {
            Sim.DayStartEvents.Completed -= Wake;
            Animation.PlayOneShot("wake", HandleWakeComplete);
        }
        
        private void HandleWakeComplete(StringName animname)
        {
            InputControlled.Enable();
            Interact.Enable();
            LanternTool.Enable();
            _isSleeping = false;
        }
    }
}