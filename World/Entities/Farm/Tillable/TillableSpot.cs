using Godot;
using SpookyBotanyGame.Core.StateMachines;
using SpookyBotanyGame.World.Entities.Properties;

namespace SpookyBotanyGame.World.Entities.Farm.Tillable
{
    public class TillableSpot : GameEntity
    {
        [Export] public Interactable Interaction;
    
        [Export] public StateMachine StateMachine;

        [Export] public AnimationPlayer Animation;
    }
}