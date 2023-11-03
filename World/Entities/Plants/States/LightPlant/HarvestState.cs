using Godot;
using SpookyBotanyGame.Collectable;
using SpookyBotanyGame.Core;
using SpookyBotanyGame.Core.StateMachines;

namespace SpookyBotanyGame.World.Entities.Plants.States
{
    public class HarvestState : State
    {
        protected LightPlant _plant;
        protected CollectableResource _resource;
        protected readonly Core.Range<int> _collectableAmount;
        
        public const string HarvestAnimationName = "Harvest";

        public HarvestState(LightPlant lightPlant, int outputAmount, CollectableResource resource)
        {
            _collectableAmount = new Range<int>(outputAmount, 0, outputAmount);
            _plant = lightPlant;
            _resource = resource;
            
            lightPlant.Sim.OnDayTick += HandleDayAdvance;
            lightPlant.Interactable.SetEnabled(true);
            lightPlant.Interactable.OnInteractionTriggered += HandleInteractionTriggered;
            PauseOnFirstFrame(lightPlant.Animation);
        }

        private void PauseOnFirstFrame(AnimationPlayer animationPlayer)
        {
            animationPlayer.Play(HarvestAnimationName);
            animationPlayer.Advance(0);
            animationPlayer.Pause();
        }

        private void HandleDayAdvance(int amount)
        {
            GD.Print("HarvestState", "HandleDayAdvance");
            if (_collectableAmount.IsMin)
            {
                Exit(new GrowingState(_plant, 1));
            }
        }

        private bool HandleInteractionTriggered(GameEntity other, GameEntity self)
        {
            if (self == null)
            {
                GD.PushError($"Failed ${nameof(HandleInteractionTriggered)} on '{_plant.Name}' sending entity was null");
                return false;
            }

            if (self != _plant)
            {
                GD.PushError($"Failed ${nameof(HandleInteractionTriggered)} on '{_plant.Name}' as sending entity {self.Name} didn't match plant");
                return false;
            }

            if (other == null)
            {
                GD.PushError($"Failed ${nameof(HandleInteractionTriggered)} on '{_plant.Name}' as other entity was null");
                return false;
            }

            if (_collectableAmount.IsMin)
            {
                GD.PushError($"Failed ${nameof(HandleInteractionTriggered)} on '{_plant.Name}' as collectable amount was 0");
                return false;
            }

            if (other is PlayerEntity playerEntity)
            {
                if (playerEntity.LanternTool != null)
                {
                    var amountToHarvest = _collectableAmount.Value;
                    playerEntity.LanternTool.Fuel.Value += amountToHarvest * 3;
                    _plant.Animation.Play("Harvest");
                    _plant.Interactable.SetEnabled(false);
                    _collectableAmount.Value = 0;
                }

                if (playerEntity.CarriedSlot != null)
                {
                    playerEntity.CarriedSlot.Add(_plant.Output);
                }
                //playerEntity.LanternTool
                /*
                var slot = playerEntity.Inventory.GetSlot(_resource);
                var amountToHarvest = _collectableAmount.Value;
                if (slot.Space < amountToHarvest)
                {
                    amountToHarvest = slot.Space;
                }

                if (amountToHarvest > 0)
                {
                    _collectableAmount.SetValue(_collectableAmount.Value - amountToHarvest);
                    slot.Add(amountToHarvest);
                    if (_collectableAmount.Value <= 0)
                    {
                        _plant.Animation.Play("Harvest");
                        _plant.Interactable.SetEnabled(false);
                    }
                    return true;
                }
                else
                {
                    GD.PushWarning($"Failed ${nameof(HandleInteractionTriggered)} on '{_plant.Name}' because {amountToHarvest} was less or equal to 0");
                }*/
            }
            else
            {
                GD.PushWarning($"Failed ${nameof(HandleInteractionTriggered)} on '{_plant.Name}' because {other} was not type {nameof(PlayerEntity)}");
            }
            
            return false;
        }
    }
}