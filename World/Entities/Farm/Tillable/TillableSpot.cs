using Godot;
using SpookyBotanyGame.Core.StateMachines;
using SpookyBotanyGame.World.Entities.Farm.Tillable.States;
using SpookyBotanyGame.World.Entities.Plants.Planting;
using SpookyBotanyGame.World.Entities.Properties;

namespace SpookyBotanyGame.World.Entities.Farm.Tillable
{
    [GlobalClass]
    public partial class TillableSpot : GameEntity
    {
        [Export] public Interactable Interaction;
    
        [Export] public StateMachine StateMachine;

        [Export] public AnimationPlayer Animation;

        [Export] public Node2D GrowSpot;

        [Export] public PlantingMapData Planting;
        
        public override void _Ready()
        {
            base._Ready();
            StateMachine.SetState(new EmptyState(this));
        }
    }
}