using Godot;
using SpookyBotanyGame.Core.StateMachines;
using SpookyBotanyGame.World.Entities.Plants.Planting;

namespace SpookyBotanyGame.World.Entities.Farm.Tillable.States
{
    public class FilledState : State<TillableSpot>
    {
        private IPlantable _itemInPlot;
        public FilledState(TillableSpot owner, IPlantable itemInPlot) : base(owner)
        {
            _owner.Interaction.SetEnabled(false);
            _itemInPlot = itemInPlot;
            _itemInPlot.OnDestroyed += HandlePlantDestroyed;
        }

        private void HandlePlantDestroyed()
        {
            _itemInPlot.OnDestroyed -= HandlePlantDestroyed;
            Exit(new EmptyState(_owner));
        }
    }
}