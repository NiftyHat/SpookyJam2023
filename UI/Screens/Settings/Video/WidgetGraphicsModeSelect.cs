using Godot;

namespace SpookyBotanyGame.UI.Screens.Settings.Video
{
    [GlobalClass]
    public partial class WidgetGraphicsModeSelect : BoxContainer
    {
        [Export] public Button ButtonWindowed { get; set; }
        [Export] public Button ButtonFullScreen { get; set; }
        [Export] public Button ButtonExclusiveFullscreen { get; set; }

        private SettingsController _controller;
        private VideoSettings _videoSettings;

        public override void _Ready()
        {
            base._Ready();
            _controller = GetNode<SettingsController>("/root/SettingsController");
            _videoSettings = _controller.Settings.Video;
            ButtonWindowed.ButtonUp += SetWindowed;
            ButtonFullScreen.ButtonUp += SetFullScreen;
            ButtonExclusiveFullscreen.ButtonUp += SetExclusiveFullscreen;
        }
        
        protected void SetWindowed()
        {
            _videoSettings.WindowMode.Set(DisplayServer.WindowMode.Windowed);
        }

        protected void SetFullScreen()
        {
            _videoSettings.WindowMode.Set(DisplayServer.WindowMode.Fullscreen);
        }
        
        protected void SetExclusiveFullscreen()
        {
            _videoSettings.WindowMode.Set(DisplayServer.WindowMode.Fullscreen);
        }
    }
}