using SpookyBotanyGame.Core.StateMachines;

namespace SpookyBotanyGame.World.Entities.Farm.Tillable.States
{
    public class FilledState : State<TillableSpot>
    {
        public FilledState(TillableSpot owner) : base(owner)
        {
        }
    }
}