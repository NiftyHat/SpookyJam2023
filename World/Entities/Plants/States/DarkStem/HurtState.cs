using SpookyBotanyGame.Core.StateMachines;

namespace SpookyBotanyGame.World.Entities.Plants.States.DarkStem;

public class HurtState : State
{
    protected readonly DarkPlantStem _stem;
    public HurtState(DarkPlantStem stem, int damage)
    {
        _stem = stem;
        _stem.Health.Value -= damage;
        _stem.OnAnimationFinished += HandleAnimationFinished;
        _stem.Animation.Play("Hurt");
    }

    private void HandleAnimationFinished(string animationName)
    {
        if (animationName == "Hurt")
        {
            if (_stem.Health.Value <= 0)
            {
                Exit(new DestroyState(_stem));
            }
            else
            {
                Exit(new IdleState(_stem));
            }
        }
    }
}