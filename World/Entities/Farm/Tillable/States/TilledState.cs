using SpookyBotanyGame.Collectable;
using SpookyBotanyGame.Core.StateMachines;

namespace SpookyBotanyGame.World.Entities.Farm.Tillable.States
{
    public class TilledState : State<TillableSpot>
    {
        private bool HandlePlayerInteracted(GameEntity other, GameEntity self)
        {
            if (other is PlayerEntity playerEntity)
            {
                if (playerEntity.CarriedSlot.Amount > 0)
                {
                    CollectableResource carriedItemType = playerEntity.CarriedSlot.CollectableType;
                    if (_owner.Planting != null && carriedItemType != null)
                    {
                        var plantScene = _owner.Planting.GetScene(carriedItemType);
                        if (plantScene != null)
                        {
                            var instance= plantScene.Instantiate();
                            _owner.GrowSpot.AddChild(instance);
                        }
                    }
                }
            }
            return false;
        }

        public TilledState(TillableSpot owner) : base(owner)
        {
            _owner.Animation.Play("Tilled");
            _owner.Interaction.OnInteractionTriggered += HandlePlayerInteracted;
        }
    }
}