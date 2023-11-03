using Godot;
using Godot.Collections;
using SpookyBotanyGame.Collectable;

namespace SpookyBotanyGame.World.Entities.Plants.Planting
{
    [GlobalClass]
    public partial class PlantingMapData : Resource
    {
        [Export] public Array<PlantingMapEntryData> Items { get; set; }

        private Dictionary<CollectableResource, PackedScene> _map;

        public void Init()
        {
            foreach (var item in Items)
            {
                _map[item.Collectable] = item.Scene;
            }
        }

        public PackedScene GetScene(CollectableResource collectable)
        {
            if (_map == null)
            {
                Init();
            }

            if (_map.TryGetValue(collectable, out var packedScene))
            {
                return packedScene;
            }

            return null;
        }
    }
}