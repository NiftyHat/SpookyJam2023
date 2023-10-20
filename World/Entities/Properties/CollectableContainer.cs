using Godot;
using SpookyBotanyGame.Collectable;

namespace SpookyBotanyGame.World.Entities.Properties
{
    [GlobalClass]
    public partial class CollectableContainer : EntityProperty
    {
        public CollectableStackSlot<ICollectableType> _slot;

        [Export]
        private CollectableResource[] _collectable
        {
            get;
            set;
        }

        public void Add(ICollectableType type, int amount)
        {
             _slot.Add(amount);
        }
    }
}

