using Godot;

namespace SpookyBotanyGame.UI.Screens.Settings.Audio;

public partial class ButtonAudioData : Resource
{
    [Export] public AudioStream Hover { get; set; }
    [Export] public AudioStream Pressed { get; set; }
}