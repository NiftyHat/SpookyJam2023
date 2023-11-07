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
            _itemInPlot.OnMaxGrowthStateChanged += HandleMaxGrowthChanged;
        }

        private void HandleMaxGrowthChanged(bool isMax, IPlantable plant)
        {
            if (isMax)
            {
                _owner.Animation.Play("Tilled");
            }
            else
            {
                _owner.Animation.Play("Empty");
            }
        }

        private void HandlePlantDestroyed()
        {
            _itemInPlot.OnDestroyed -= HandlePlantDestroyed;
            _owner.Animation.Play("Empty");
            Exit(new EmptyState(_owner));
        }
    }
}