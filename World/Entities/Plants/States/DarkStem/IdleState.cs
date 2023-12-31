using Godot;
using SpookyBotanyGame.Core.StateMachines;

namespace SpookyBotanyGame.World.Entities.Plants.States.DarkStem;

public class IdleState : State, IUpdatableState
{
    protected DarkPlantStem _stem;
    private bool _isDamagedByLight;
    public IdleState(DarkPlantStem stem)
    {
        stem.Animation.Play("Idle");
        _stem = stem;
        _stem.CanTriggerAttack = true;
        _stem.LightSensor.OnApply += HandleLightApply;
    }

    protected override void Exit(State state = null)
    {
        _stem.LightSensor.OnApply -= HandleLightApply;
        base.Exit(state);
    }

    public void Process(double delta)
    {
        if (_isDamagedByLight)
        {
            if (_stem.Health.Value > 0)
            {
                Exit(new HurtState(_stem, 1));
            }
            else
            {
                Exit(new DestroyState(_stem));
            }
        }
    }
    
    private void HandleLightApply(LightEmissionZone zone, float lightPower)
    {
        if (_isDamagedByLight)
        {
            return;
        }
        if (lightPower >= 0.9f)
        {
            _isDamagedByLight = true;
        }
        else
        {
            _isDamagedByLight = false;
        }
    }
}