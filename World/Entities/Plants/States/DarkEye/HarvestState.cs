using Godot;
using SpookyBotanyGame.Collectable;
using SpookyBotanyGame.Core.StateMachines;

namespace SpookyBotanyGame.World.Entities.Plants.States.DarkEye;

public class HarvestState : State
{
    protected DarkPlantEye _plant;
    public const string HarvestAnimationName = "Harvest";

    public HarvestState(DarkPlantEye plant)
    {
        _plant = plant;
        _plant.OnDayTick += HandleDayAdvance;
        PlayHarvestAnimation(_plant.Animation);
    }

    private void PlayHarvestAnimation(AnimationPlayer animationPlayer)
    {
        animationPlayer.Play(HarvestAnimationName);
    }

    private void HandleDayAdvance(int amount)
    {
        Exit(new GrowingState(_plant, 0));
    }
}