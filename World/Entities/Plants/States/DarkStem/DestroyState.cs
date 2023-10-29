using SpookyBotanyGame.Core.StateMachines;

namespace SpookyBotanyGame.World.Entities.Plants.States.DarkStem;

public class DestroyState : State
{
    protected DarkPlantStem _stem;
    
    public DestroyState(DarkPlantStem stem)
    {
        stem.Animation.Play("Destroy");
        _stem = stem;
        _stem.Sim.OnDayTick += HandleDayTick;
        _stem.CanAttack = false;
    }

    protected override void Exit(State state = null)
    {
        _stem.Sim.OnDayTick -= HandleDayTick;
        base.Exit(state);
    }

    private void HandleDayTick(int dayCount)
    {
        _stem.Destroy();
        Exit(null);
    }
}