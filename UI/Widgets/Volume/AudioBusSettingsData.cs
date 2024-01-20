using Godot;
using SpookyBotanyGame.Core.Bindable;

namespace SpookyBotanyGame.UI.Widgets.Volume
{
    [GlobalClass]
    public partial class AudioBusSettingsData : Resource
    {
        public class AudioBusReference
        {
            private readonly int _index;
            public readonly string Name;
            
            public AudioBusReference(int index, string name)
            {
                _index = index;
                Name = name;
            }

            public void SetVolumeLinear(float linearValue)
            {
                float db = Mathf.LinearToDb(linearValue);
                AudioServer.SetBusVolumeDb(_index, db);
            }

            public float GetVolumeLinear()
            {
                float db = AudioServer.GetBusVolumeDb(_index);
                return Mathf.DbToLinear(db);
            }
        }
        
        [Export] public string FriendlyName { get; set; }
        [Export] public string BusName { get; set; }

        public AudioBusReference Bus => GetBus();
        protected AudioBusReference _bus;

        public readonly IBindable<double> LinearVolume = new BindableDouble();

        public AudioBusSettingsData()
        {
            LinearVolume.BindOnce(HandleLinearVolumeChanged);
        }

        private void HandleLinearVolumeChanged(double value)
        {
            if (_bus == null)
            {
                _bus = GetBus();
            }
            _bus?.SetVolumeLinear((float)value);
        }

        private AudioBusReference GetBus()
        {
            if (_bus == null)
            {
                int index = AudioServer.GetBusIndex(BusName);
                if (index != -1)
                {
                    _bus = new AudioBusReference(index, BusName);
                }
            }
            return _bus;
        }

        public float GetVolume()
        {
            if (_bus == null)
            {
                _bus = GetBus();
            }
            return _bus.GetVolumeLinear();
        }
    }
}