using Godot;

namespace SpookyBotanyGame.Core.StateMachines
{
    [GlobalClass]
    public partial class StateMachine : Node
    {
        private State _currentState;
        private IUpdatableState _updatableState;
        
        public override void _Ready()
        {
            base._Ready();
        }

        public void SetState(State state)
        {
            _currentState = state;
            _currentState.OnExit += HandleUpdatableStateExit;
        }

        private void HandleUpdatableStateExit(State state)
        {
            _currentState = null;
            if (_currentState != null)
            {
                SetState(state);
            }
        }

        public override void _Process(double delta)
        {
            _updatableState?.Process(delta);
            base._Process(delta);
        }

        /*
        public override void _PhysicsProcess(double delta)
        {
            _updatableState.PhysicsProcess(delta);
            base._PhysicsProcess(delta);
        }*/
    }
}