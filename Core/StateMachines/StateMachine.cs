using Godot;

namespace SpookyBotanyGame.Core.StateMachines
{
    [GlobalClass]
    public partial class StateMachine : Node
    {
        [Export] private bool Logging { get; set; }
        private State _currentState;
        public State CurrentState => _currentState;
        private IUpdatableState _updatableState;
        private string _loggingName;
        
        public override void _Ready()
        {
            if (Logging)
            {
                var parent = GetParent();
                if (parent != null)
                {
                    _loggingName = $"{parent.Name}.{this.Name}";
                }
                else
                {
                    _loggingName = $"{this.Name}";
                }
            }
            base._Ready();
        }

        public void SetState(State state)
        {
            if (Logging)
            {
                string currentStateName = _currentState != null ? _currentState.GetType().Name : "null";
                string nextStateName = state != null ? state.GetType().Name : "null";
                GD.Print(_loggingName, $"{currentStateName} -> {nextStateName}");
            }
            //if you try and set state with an active state force eject it so exit gets called.
            if (_currentState != null)
            {
                _currentState.ForceExit(this);
                _currentState = null;
                _updatableState = null;
            }
            _currentState = state;
            if (state != null)
            {
                _currentState.OnExit += HandleStateExit;
                if (state is IUpdatableState updatableState)
                {
                    _updatableState = updatableState;
                }
            }
        }

        private void HandleStateExit(State nextState)
        {
            if (Logging)
            {
                string currentStateName = _currentState != null ? _currentState.GetType().ToString() : "null";
                string nextStateName = nextState != null ? nextState.GetType().ToString() : "null";
                GD.Print(_loggingName, $"{currentStateName} -> {nextStateName}");
            }
            
            _currentState = null;
            _updatableState = null;
            if (nextState != null)
            {
                SetState(nextState);
            }
        }

        public override void _Process(double delta)
        {
            _updatableState?.Process(delta);
            base._Process(delta);
        }
    }
}