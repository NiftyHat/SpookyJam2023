using Godot;

namespace SpookyBotanyGame.UI.Screens.Settings.Video;

[GlobalClass]
public partial class WidgetBrightnessSlider : HBoxContainer
{
    private SettingsController _controller;
    [Export] public Slider Slider { get; set; }
    [Export] public Label ValueLabel { get; set; }
    [Export] public Button ButtonReset { get; set; }

    public override void _Ready()
    {
        base._Ready();
        _controller = GetNode<SettingsController>("/root/SettingsController");
        Slider.Value = _controller.Settings.Video.Brightness.Value;
        SetLabel(Slider.Value);
        Slider.ValueChanged += HandleSliderChanged;
        ButtonReset.Pressed += HandleButtonResetPressed;
    }

    private void HandleButtonResetPressed()
    {
        Slider.Value = 1.0;
    }

    private void SetLabel(double value)
    {
        ValueLabel.Text = $"{value:N1}";
    }

    private void HandleSliderChanged(double value)
    {
        float brightnessValue = (float)value;
        SetLabel(brightnessValue);
        _controller.Settings.Video.Brightness.Set(brightnessValue);
        //Environment.AdjustmentBrightness = (float)value;
    }
}