using Godot;
using SpookyBotanyGame.Collectable;

namespace SpookyBotanyGame.World.Entities.Plants.Planting
{
    [GlobalClass]
    public partial class PlantingMapEntryData : Resource
    {
        [Export] public CollectableResource Collectable { get; set; }
        [Export] public PackedScene Scene { get; set; }
    }
}