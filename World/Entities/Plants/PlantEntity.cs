using Godot;

namespace SpookyBotanyGame.World.Entities.Plants;

[GlobalClass]
public partial class PlantEntity : GameEntity
{
    [Export] public AnimationPlayer Animation { get; set; }
    
}