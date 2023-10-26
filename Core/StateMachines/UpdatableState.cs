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