using Godot;

namespace SpookyBotanyGame.UI.Widgets.Volume
{
    [GlobalClass]
    public partial class WidgetVolumeSlider : VBoxContainer
    {
        [Export] public AudioBusSettingsData BusSettings { get; set; }
        [Export] public AudioStream Stream { get; set; }
        [Export] public Slider Slider { get; set; }
        [Export] public Label LabelName { get; set; }
        [Export] public Label ValueLabel { get; set; }
        [Export] public AudioStreamPlayer AudioPlayer { get; set; }

        public override void _Ready()
        {
            base._Ready();
            Slider.SetValueNoSignal(BusSettings.LinearVolume.Value);
            SetLabel(Slider.Value);
            LabelName.Text = BusSettings.FriendlyName;
            Slider.ValueChanged += HandleSliderChanged;
            Slider.FocusExited += HandleSliderFocusExit;
            AudioPlayer.Bus = BusSettings.BusName;
            AudioPlayer.Stream = Stream;
        }

        private void HandleSliderFocusExit()
        {
            AudioPlayer.Stop();
        }

        private void SetLabel(double value)
        {
            ValueLabel.Text = $"{value:N1}";
        }

        private void HandleSliderChanged(double value)
        {
            BusSettings.LinearVolume.Set(value);
            BusSettings.Bus.SetVolumeLinear((float)value);
            SetLabel(Slider.Value);
            if (Stream != null && !AudioPlayer.Playing)
            {
                AudioPlayer.Play();
            }
        }
    }
}