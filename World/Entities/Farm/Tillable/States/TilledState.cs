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
                var activeSeedSlot = playerEntity.GetFirstActiveSeedSlot();
                if (activeSeedSlot != null) 
                {
                    CollectableResource carriedItemType = activeSeedSlot.CollectableType;
                    if (_owner.Planting != null && carriedItemType != null)
                    {
                        var plantScene = _owner.Planting.GetScene(carriedItemType);
                        if (plantScene != null)
                        {
                            IPlantable instance= plantScene.Instantiate<IPlantable>();
                            instance.SetSpot(_owner);
                            activeSeedSlot.Remove(1);
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
            _owner.Interaction.SetEnabled(true);
        }

        protected override void Exit(State state = null)
        {
            base.Exit(state);
            _owner.Interaction.OnInteractionTriggered -= HandlePlayerInteracted;
            _owner.Interaction.SetEnabled(false);
        }
    }
}