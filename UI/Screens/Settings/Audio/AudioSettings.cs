using Godot.Collections;
using SpookyBotanyGame.Core;
using SpookyBotanyGame.UI.Widgets.Volume;

namespace SpookyBotanyGame.UI.Screens.Settings.Audio
{
    public class AudioSettings : SettingsSection
    {
        public readonly string Name;

        public AudioSettings()
        {
            Name = "Audio";
            Items = new System.Collections.Generic.Dictionary<string, VariantBinding>();
        }

        public void Bind(Array<AudioBusSettingsData> audioBusList)
        {
            foreach (var audioBus in audioBusList)
            {
                if (!Items.ContainsKey(audioBus.BusName))
                {
                    Items.Add(audioBus.BusName, new VariantBinding<double>(audioBus.LinearVolume));
                }
            }
        }
    }
}