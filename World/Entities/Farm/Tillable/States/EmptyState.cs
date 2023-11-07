using Godot;
using SpookyBotanyGame.Collectable;
using SpookyBotanyGame.Core.StateMachines;
using SpookyBotanyGame.World.Entities.Plants.Planting;

namespace SpookyBotanyGame.World.Entities.Farm.Tillable.States;

public class EmptyState : State<TillableSpot>
{
    public EmptyState(TillableSpot owner) : base(owner)
    {
        _owner.Animation.Play("Empty");
        _owner.Interaction.SetEnabled(true);
        _owner.Interaction.CanInteractFilter = FilterHasSeeds;
        _owner.Interaction.OnInteractionTriggered += HandlePlayerInteracted;
    }

    private bool FilterHasSeeds(GameEntity other)
    {
        return other is PlayerEntity playerEntity && TryGetPlantPackedScene(playerEntity, out _, out _);
    }

    private bool TryGetPlantPackedScene(PlayerEntity playerEntity, out PackedScene plantScene, out CollectableStackSlot<CollectableResource> seedSlot)
    {
        seedSlot = playerEntity.GetFirstActiveSeedSlot();
        if (seedSlot != null) 
        {
            CollectableResource carriedItemType = seedSlot.CollectableType;
            if (_owner.Planting != null && carriedItemType != null)
            {
                plantScene = _owner.Planting.GetScene(carriedItemType);
                if (plantScene != null)
                {
                    return true;
                }
                return false;
            }
        }
        plantScene = null;
        return false;
    }

    protected override void Exit(State state = null)
    {
        base.Exit(state);
        _owner.Interaction.OnInteractionTriggered -= HandlePlayerInteracted;
    }

    private bool HandlePlayerInteracted(GameEntity other, GameEntity self)
    {
        if (other is PlayerEntity playerEntity && TryGetPlantPackedScene(playerEntity, out var plantScene, out var activeSeedSlot))
        {
            IPlantable instance= plantScene.Instantiate<IPlantable>();
            instance.SetSpot(_owner);
            activeSeedSlot.Remove(1);
            Exit(new FilledState(_owner, instance));
        }
        return false;
    }
}