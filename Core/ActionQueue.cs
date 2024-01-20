using System;
using System.Collections.Concurrent;
using Godot;

namespace SpookyBotanyGame.Core;

public class ActionQueue : Node
{
    public delegate void ActionExecution();
    
    private readonly ConcurrentQueue<ActionExecution> _actionQueue = new ConcurrentQueue<ActionExecution>();
    
    public void Add(ActionExecution action)
    {
        _actionQueue.Enqueue(action);
    }
    

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (!_actionQueue.IsEmpty)
        {
            while (_actionQueue.TryDequeue(out var action))
            {
                action?.Invoke();
            }
        }
    }
}