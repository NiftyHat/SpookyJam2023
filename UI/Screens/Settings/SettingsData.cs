using Godot;
using Godot.Collections;
using SpookyBotanyGame.UI.Screens.Settings.Audio;
using SpookyBotanyGame.UI.Screens.Settings.Video;
using SpookyBotanyGame.UI.Widgets.Volume;

namespace SpookyBotanyGame.UI.Screens.Settings
{
    [GlobalClass]
    public partial class SettingsData : Resource
    {
        [Export] public Array<AudioBusSettingsData> AudioBusList { get; set; }
        
        private ConfigFile _file;
        private string _path = "user://settings.cfg";
        private Error _loadError;
        private bool _isValid;
        private bool _isInitialized;

        public AudioSettings Audio { get; private set; }
        public VideoSettings Video { get; private set; }

        private System.Collections.Generic.Dictionary<string, SettingsSection> _sections;
        
        public void Init()
        {
            if (_isInitialized)
            {
                return;
            }
            _isInitialized = true;
            Audio = new AudioSettings();
            Video = new VideoSettings();
            _sections = new System.Collections.Generic.Dictionary<string, SettingsSection>()
            {
                { Audio.Name, Audio },
                { Video.Name, Video }
            };
            _file = new ConfigFile();
        }

        public void Save(string settingsKey, SettingsSection section)
        {
            foreach (var kvp in section.Items)
            {
                var valueKey = kvp.Key;
                var binding = kvp.Value;
                _file.SetValue(settingsKey,valueKey, binding.Value);
            }
        }
        
        public void Save()
        {
            foreach (var kvp in _sections)
            {
                var key = kvp.Key;
                var section = kvp.Value;
                GD.Print($"Saving [{key}]");
                Save(key, section);
            }
            _file.Save(_path);
        }

        public void Load()
        {
            if (!_isInitialized)
            {
                Init();
            }
            _loadError = _file.Load(_path);
            _isValid = _loadError == Error.Ok;
            if (_loadError != Error.Ok)
            {
                GD.PrintErr($"Failed to Load with error {_loadError}");
                return;
            }
            _file.Load(_path);
            var sectionKeyList = _file.GetSections();
            foreach (var sectionKey in sectionKeyList)
            {
                GD.Print($"Loading [{sectionKey}]");
                if (_sections.TryGetValue(sectionKey, out SettingsSection section))
                {
                    Dictionary<string, Variant> loadedValues = new Dictionary<string, Variant>();
                    string[] itemKeyList = _file.GetSectionKeys(sectionKey);
                    foreach (var key in itemKeyList)
                    {
                        var value = _file.GetValue(sectionKey, key);
                        
                        loadedValues.Add(key, value);
                        GD.Print($"Loaded KVP {key} = {value}");
                    }
                    section.Load(loadedValues);
                }
            }
        }
    }
}