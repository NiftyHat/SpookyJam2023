using Godot;

namespace SpookyBotanyGame.Core.StateMachines
{
    [GlobalClass]
    public partial class StateMachine : Node
    {
        [Export] private bool Logging { get; set; }
        private State _currentState;
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
                GD.Print(_loggingName, $"Enter state {state.GetType()}");
            }
            _currentState = state;
            _currentState.OnExit += HandleStateExit;
            if (state is IUpdatableState updatableState)
            {
                _updatableState = updatableState;
            }
        }

        private void HandleStateExit(State state)
        {
            if (Logging)
            {
                GD.Print(_loggingName, $"Exit state {state.GetType()}");
            }
            _currentState = null;
            _updatableState = null;
            if (state != null)
            {
                SetState(state);
            }
        }

        public override void _Process(double delta)
        {
            _updatableState?.Process(delta);
            base._Process(delta);
        }
    }
}