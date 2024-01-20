using Godot;
using SpookyBotanyGame.UI.Screens.Settings;
using SpookyBotanyGame.UI.Screens.Settings.Video;

namespace SpookyBotanyGame.Core
{
    [GlobalClass]
    public partial class GameWorldEnvironment : WorldEnvironment
    {
        private SettingsController _controller;
        private VideoSettings _videoSettings;

        public override void _Ready()
        {
            base._Ready();
            _controller = GetNode<SettingsController>("/root/SettingsController");
            _videoSettings = _controller.Settings.Video;
            _videoSettings.Brightness.Bind((value) =>
            {
                Environment.AdjustmentEnabled = true;
                Environment.AdjustmentBrightness = value;
            });
        }
    }
}