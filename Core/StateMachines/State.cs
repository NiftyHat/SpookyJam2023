namespace SpookyBotanyGame.Core.StateMachines;

public abstract class State
{
    public delegate void Exited(State state);
    public event Exited OnExit;

    protected virtual void Exit(State state = null)
    {
        OnExit?.Invoke(state);
        OnExit = null;
    }

    public void ForceExit(StateMachine stateMachine, State nextState = null)
    {
        if (stateMachine.CurrentState == this)
        {
            Exit(nextState);
        }
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