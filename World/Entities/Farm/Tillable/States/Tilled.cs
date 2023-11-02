using SpookyBotanyGame.Core.StateMachines;

namespace SpookyBotanyGame.World.Entities.Farm.Tillable.States
{
    public class Tilled : State
    {
        private TillableSpot _spot;

        public Tilled(TillableSpot spot)
        {
            _spot = spot;
            _spot.Animation.Play("Tilled");
            _spot.Interaction.OnInteractionTriggered += HandlePlayerInteracted;
        }

        private bool HandlePlayerInteracted(GameEntity other, GameEntity self)
        {
            return false;
        }
    }
}