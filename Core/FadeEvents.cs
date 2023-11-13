using System;
using Godot;

namespace SpookyBotanyGame.Core;

public class FadeEvents
{
    public event Action Started;
    public event Action Completed;

    public Callable Start;
    public Callable Complete;

    public FadeEvents()
    {
        Start = Callable.From(InternalStart);
        Complete = Callable.From(InternalComplete);
    }

    public void InternalStart()
    {
        Started?.Invoke();
    }

    public void InternalComplete()
    {
        Completed?.Invoke();
    }
}