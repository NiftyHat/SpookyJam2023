using Godot;

namespace SpookyBotanyGame.World.Entities
{
    [GlobalClass]
    public partial class Area2DEntityReference : Area2D, IEntityProvider
    {
        [Export] private GameEntity Entity { get; set; }

        public override void _Ready()
        {
            base._Ready();
            if (Entity == null)
            {
                var parent =  GetParent<GameEntity>();
                if (parent != null)
                {
                    GD.PushWarning($"EntityReference '{Name}' missing serialized value attempting to get from parent '{parent.Name}'");
                }
                else
                {
                    GD.PushError($"EntityReference {Name} missing serialized value and has no parent with valid value.");
                }
            }
        }

        public GameEntity GetEntity()
        {
            return Entity;
        }
    }
}