using SpookyBotanyGame.Collectable;
using SpookyBotanyGame.Core.StateMachines;
using SpookyBotanyGame.World.Entities.Plants.Planting;

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
                            IPlantable instance= plantScene.Instantiate<IPlantable>();
                            instance.SetSpot(_owner);
                            playerEntity.CarriedSlot.Remove(1);
                            Exit(new FilledState(_owner, instance));
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