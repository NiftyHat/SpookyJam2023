using Godot;

namespace SpookyBotanyGame.UI.Screens.GameOver
{
    public partial class GameOverScreen : Control
    {
        [Export] public Button ButtonRestart { get; set; }
        [Export] public Button ButtonHome { get; set; }
        [Export] public Button ButtonQuit { get; set; }

        protected UIController _controller;

        public override void _Ready()
        {
            base._Ready();
            ButtonRestart.ButtonUp += HandlePlayButtonUp;
            ButtonQuit.ButtonUp += HandleQuitButtonUp;
            ButtonHome.ButtonUp += HandleHomeButtonUp;
            _controller = GetNode<UIController>("/root/UIController");
        }

        private void HandleHomeButtonUp()
        {
            _controller.ApplicationHome();
        }

        private void HandleQuitButtonUp()
        {
            _controller.ApplicationQuit();
        }

        private void HandlePlayButtonUp()
        {
            _controller.ApplicationPlay();
        }
    }
}