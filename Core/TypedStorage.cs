
using System;
using System.Collections.Generic;
using Godot;

namespace SpookyBotanyGame.Core
{
    public class TypedStorage<TBaseType>
    {
        public Dictionary<System.Type, TBaseType> _map = new Dictionary<Type, TBaseType>();

        public void Add<TType>(TType item) where TType : TBaseType
        {
            System.Type key = typeof(TType);
            if (_map.TryGetValue(key, out TBaseType storedItem))
            {
                if (!storedItem.Equals(item))
                {
                    GD.PushError($"{nameof(TypedStorage<TBaseType>)} can't add item {item} to key {key.Name} since it's already populated by {storedItem}");
                    return;
                }
            }
            _map.Add(key,item);
        }
        
        public TType Get<TType>() where TType : TBaseType
        {
            System.Type key = typeof(TType);
            if (_map.TryGetValue(key, out TBaseType storedItem))
            {
                return (TType)storedItem;
            }
            return default;
        }

        public bool TryGet<TType>(out TType item) where TType : TBaseType
        {
            System.Type key = typeof(TType);
            GD.Print($"TryGet {key.Name}");
            GD.Print($"Keys {string.Join(",",_map.Keys)}");
            if (_map.TryGetValue(key, out TBaseType storedItem))
            {
                item = (TType)storedItem;
                return true;
            }
            item = default;
            return false;
        }

        public void Remove<TType>() where TType : TBaseType
        {
            System.Type key = typeof(TType);
            _map.Remove(key);
        }
    }
}