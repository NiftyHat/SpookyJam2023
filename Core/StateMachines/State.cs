namespace SpookyBotanyGame.Core.StateMachines;

public abstract class State
{
    public delegate void Exited(State state);
    public event Exited OnExit;

    protected virtual void Exit(State state = null)
    {
        OnExit?.Invoke(state);
    }
}

public abstract class State<TOwner> : State where TOwner : class
{
    protected TOwner _owner;
    public State(TOwner owner)
    {
        _owner = owner;
    }

    protected override void Exit(State state = null)
    {
        base.Exit(state);
        _owner = null;
    }
}