using Godot;
using SpookyBotanyGame.Core.StateMachines;

namespace SpookyBotanyGame.World.Entities.Plants.States.DarkStem;

public class IdleState : State, IUpdatableState
{
    protected DarkPlantStem _stem;
    public IdleState(DarkPlantStem stem)
    {
        stem.Animation.Play("Idle");
        _stem = stem;
        _stem.CanAttack = true;
    }

    public void Process(double delta)
    {
        GD.Print(_stem.LightSensor.MaxLightDelta.ThisFrame);
        if (_stem != null && _stem.LightSensor != null && _stem.LightSensor.MaxLightDelta.ThisFrame > 0.5f)
        {
            if (_stem.Health.Value > 0)
            {
                Exit(new HurtState(_stem, 1));
            }
        }
    }
}