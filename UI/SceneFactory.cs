using System;
using System.Collections.Generic;
using Godot;

namespace SpookyBotanyGame.UI;

public class SceneFactory
{
    protected Dictionary<System.Type, PackedScene> _map = new Dictionary<Type, PackedScene>();
    
    public void Add<TBaseType>(PackedScene scene) where TBaseType : Node
    {
        _map.Add(typeof(TBaseType), scene);
    }

    public TBaseType Instantiate<TBaseType>() where TBaseType : Node
    {
        if (_map.TryGetValue(typeof(TBaseType), out PackedScene packedScene))
        {
            return packedScene.Instantiate<TBaseType>();
        }
        return null;
    }
}