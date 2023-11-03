using Godot;
using SpookyBotanyGame.Core.StateMachines;
using SpookyBotanyGame.World.Entities.Plants.Planting;

namespace SpookyBotanyGame.World.Entities.Farm.Tillable.States
{
    public class FilledState : State<TillableSpot>
    {
        public FilledState(TillableSpot owner, IPlantable itemInPlot) : base(owner)
        {
            _owner.Interaction.SetEnabled(false);
            itemInPlot.OnDestroyed += HandlePlantDestroyed;
        }

        private void HandlePlantDestroyed()
        {
            Exit(new EmptyState(_owner));
        }
    }
}