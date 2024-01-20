using System.Collections.Generic;
using Godot;
using SpookyBotanyGame.Core;
using SpookyBotanyGame.Core.Bindable;

namespace SpookyBotanyGame.UI.Screens.Settings.Video;

public class VideoSettings : SettingsSection
{
    public readonly string Name;
    public BindableFloat Brightness = new (1.0f);
    public BindableWindowMode WindowMode = new ();
    
    public class BindableWindowMode : BindableEnum<DisplayServer.WindowMode>
    {
    }
    
    public VideoSettings()
    {
        Name = "Video";
        Items = new Dictionary<string, VariantBinding>();
        Items.Add("Brightness", new VariantBinding<float>(Brightness));
        Items.Add("WindowMode", new VariantBinding<DisplayServer.WindowMode>(WindowMode));
    }
}