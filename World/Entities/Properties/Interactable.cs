using Godot;
namespace SpookyBotanyGame.World.Entities.Properties
{
    [GlobalClass,Icon("res://World/Entities/icon-entity-property.svg")]
    public partial class Interactable : EntityProperty
    {
        [Export] private InteractionHandler Handler { get; set; }
        
        public override void _Ready()
        {
            base._Ready();
            _entity?.AddProperty<Interactable>(this);
        }
        
        public void SetTargeted(bool isTargeted)
        {
            if (isTargeted)
            {
                Handler.OutlineShaderShow();
            }
            else
            {
                Handler.OutlineShaderHide();
            }
        }

        public void TriggerInteraction(GameEntity entity)
        {
            //throw new System.NotImplementedException();
        }
    }
}