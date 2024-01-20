using Godot;
using SpookyBotanyGame.Core;

namespace SpookyBotanyGame.UI.Screens.Settings
{
    public partial class SettingsController : Node
    {
        public SettingsData Settings { get; set; }
        
        public static SettingsData LoadData()
        {
            string settingsDataPath = "res://UI/Screens/Settings/SettingsData.tres";
            var data = (SettingsData)GD.Load(settingsDataPath);
            if (data == null)
            {
                GD.PushError($"Missing resource '{settingsDataPath}'");
                return null;
            }
            data.Init();
            return data;
        }

        public override void _EnterTree()
        {
            Settings = LoadData();
            Settings.Load();
            Settings.Video.Brightness.Bind(OnBrightnessChanged);
            Settings.Video.WindowMode.Bind(OnWindowModeChange);
            Settings.Audio.Bind(Settings.AudioBusList);
            base._EnterTree();
        }

        public override void _Ready()
        {

        }

        public void Save()
        {
            Settings.Save();
        }

        private void OnWindowModeChange(DisplayServer.WindowMode value)
        {
            DisplayServer.WindowSetMode(value);
        }

        private void OnBrightnessChanged(float value)
        {
            var viewport = GetTree().Root;
            var worldEnvironment = viewport.FindFirstChild<WorldEnvironment>();
            if (worldEnvironment != null)
            {
                worldEnvironment.Environment.AdjustmentBrightness = value;
            }
        }
    }
}