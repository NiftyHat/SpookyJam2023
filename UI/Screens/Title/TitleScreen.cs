using Godot;
using SpookyBotanyGame.UI.Screens.Settings;

namespace SpookyBotanyGame.UI.Screens.Title
{
    [GlobalClass]
    public partial class TitleScreen : Control
    {
        [Export] public Button ButtonPlay { get; set; }
        [Export] public Button ButtonQuit { get; set; }
        [Export] public Button ButtonSettings { get; set; }

        protected UIController _controller;

        public override void _Ready()
        {
            base._Ready();
            ButtonPlay.ButtonUp += HandlePlayButtonUp;
            ButtonQuit.ButtonUp += HandleQuitButtonUp;
            ButtonSettings.ButtonUp += HandleSettingsButtonUp;
            _controller = GetNode<UIController>("/root/UIController");
        }

        private void HandleSettingsButtonUp()
        {
            _controller.Open<SettingsScreen>();
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