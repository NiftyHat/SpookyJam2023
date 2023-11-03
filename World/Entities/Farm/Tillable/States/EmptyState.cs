using SpookyBotanyGame.Core.StateMachines;

namespace SpookyBotanyGame.World.Entities.Farm.Tillable.States;

public class EmptyState : State<TillableSpot>
{
    public EmptyState(TillableSpot owner) : base(owner)
    {
        _owner.Animation.Play("Empty");
        _owner.Interaction.OnInteractionTriggered += HandlePlayerInteracted;
    }

    private bool HandlePlayerInteracted(GameEntity other, GameEntity self)
    {
        if (self == _owner)
        {
            Exit(new TilledState(_owner));
            return true;
        }
        return false;
    }
}