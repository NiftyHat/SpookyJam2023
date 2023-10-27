namespace SpookyBotanyGame.Core.StateMachines
{
    public interface IUpdatableState
    {
        void Process(double delta);
    }
}

