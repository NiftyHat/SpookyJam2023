using Godot;
using SpookyBotanyGame.Core.StateMachines;
using SpookyBotanyGame.World.Entities.Farm.Tillable.States;
using SpookyBotanyGame.World.Entities.Plants;
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

        [Export] public LightPlant StartingPlant;
        
        public override void _Ready()
        {
            base._Ready();
            if (StartingPlant != null)
            {
                GD.Print($"{Name} StartingPlant");
                var parent = StartingPlant.GetParent();
                if (parent != null)
                {
                    void MoveChild()
                    {
                        parent.RemoveChild(StartingPlant);
                        GrowSpot.AddChild(StartingPlant);
                        StartingPlant.Position = Vector2.Zero;
                        GetTree().ProcessFrame -= MoveChild;
                        StateMachine.SetState(new FilledState(this, StartingPlant));
                    }
                    GetTree().ProcessFrame += MoveChild;
                    return;
                }
            }
            StateMachine.SetState(new EmptyState(this));
        }
    }
}