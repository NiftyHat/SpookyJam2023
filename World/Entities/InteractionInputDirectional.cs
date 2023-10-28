using System.Text;
using Godot;
using Godot.Collections;
using SpookyBotanyGame.World.Entities.Properties;

namespace SpookyBotanyGame.World.Entities
{
    [GlobalClass]
    public partial class InteractionInputDirectional : Node2D
    {
        [Export] public Area2D HitBox { get; set; }
        [Export] public float Distance { get; set; }
        [Export] public GameEntity Entity { get; set; }

        [Export(PropertyHint.MultilineText)] 
        public string Debug { get; set; }

        private Dictionary<Node2D, Interactable> _trackedEntities = new Dictionary<Node2D, Interactable>();
        private Node2D _parent;
        private Vector2 _offset;
        private bool _isEnabled = true;

        public override void _Ready()
        {
            base._Ready();
            HitBox.Monitoring = true;
            HitBox.AreaEntered += HandleAreaEntered;
            HitBox.AreaExited += HandleAreaExited;
            HitBox.BodyEntered += HandleBodyEntered;
            HitBox.BodyExited += HandleBodyExited;
            Distance = (HitBox.Position).Length();
        }
        
        public void DoInteract()
        {
            foreach (var kvp in _trackedEntities)
            {
                var interactable = kvp.Value;
                interactable.DoInteraction(Entity);
            }
        }

        private void HandleAreaEntered(Area2D area)
        {
            
            
            if (GameEntity.TryGetProperty(area, out Interactable interactable, out GameEntity gameEntity))
            {
                Add(interactable, gameEntity);
                interactable.SetTargeted(true);
            }
        }
        
        private void HandleAreaExited(Area2D area)
        {
            if (GameEntity.TryGetProperty(area, out Interactable interactable, out GameEntity gameEntity))
            {
                interactable.SetTargeted(false);
                Remove(gameEntity);
            }
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            Vector2 angle = AngleToMouse();
            _offset = angle.Normalized() * Distance;
            HitBox.Position = _offset;
        }

        protected Vector2 AngleToMouse()
        {
            var mousePosition = GetLocalMousePosition();
            var pivotPosition = Vector2.Zero;
            return (mousePosition - pivotPosition);
        }
        
        private void UpdateDebug()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var kvp in _trackedEntities)
            {
                sb.AppendLine(kvp.Key.Name);
            }
        }

        private void Add(Interactable interactable, GameEntity gameEntity)
        {
            if (!_trackedEntities.ContainsKey(gameEntity))
            {
                _trackedEntities.Add(gameEntity, interactable);
                interactable.OnEnabledUpdate += HandleInteractableEnabled;
            }
        }

        private void HandleInteractableEnabled(bool obj)
        {
            
        }

        private void Remove(GameEntity gameEntity)
        {
            if (_trackedEntities.ContainsKey(gameEntity))
            {
                _trackedEntities.Remove(gameEntity);
            }
        }

        private void HandleBodyExited(Node2D body)
        {
            if (GameEntity.TryGetProperty(body, out Interactable interactable, out GameEntity gameEntity))
            {
                Add(interactable, gameEntity);
                interactable.SetTargeted(true);
            }
        }
        private void HandleBodyEntered(Node2D body)
        {
            if (GameEntity.TryGetProperty(body, out Interactable interactable, out GameEntity gameEntity))
            {
                interactable.SetTargeted(false);
                Remove(gameEntity);
            }
        }
        
        public void Enable()
        {
            _isEnabled = true;
        }

        public void Disable()
        {
            _isEnabled = false;
        }
    }
}