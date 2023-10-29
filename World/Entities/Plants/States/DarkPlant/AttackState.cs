using SpookyBotanyGame.Core.StateMachines;

namespace SpookyBotanyGame.World.Entities.Plants.States.DarkPlant;

public abstract class AttackState : State
{
    public abstract string AnimStart { get; }
    public abstract string AnimLoop { get; }
    public abstract string AnimEnd { get; }
}