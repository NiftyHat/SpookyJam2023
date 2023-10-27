using System.Collections.Generic;
using Godot;
using SpookyBotanyGame.Collectable;

namespace SpookyBotanyGame.World.Entities.Properties
{
    [GlobalClass]
    public partial class CollectableContainer : EntityProperty
    {
        //public CollectableStackSlot<ICollectableType> _slot;
        private readonly Dictionary<CollectableResource, CollectableStackSlot<ICollectableType>> _slots  = new();

        [Export]
        public CollectableResource[] ValidCollectableTypes
        {
            get;
            set;
        }

        public override void _Ready()
        {
            if (ValidCollectableTypes != null)
            {
                foreach (var item in ValidCollectableTypes)
                {
                    _slots[item] = new CollectableStackSlot<ICollectableType>();
                }
            }
            base._Ready();
        }

        public CollectableStackSlot<ICollectableType> GetSlot(CollectableResource type)
        {
            return _slots[type];
        }

        public void Add(CollectableResource type, int amount)
        {
            var slot = GetSlot(type);
            if (slot != null)
            {
                slot.Add(amount);
            }
        }
    }
}

