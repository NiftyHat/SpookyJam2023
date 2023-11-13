using Godot;

namespace SpookyBotanyGame.World.Entities.Effects.Audio
{
    [GlobalClass]
    public partial class SineWaveGenerator : Node2D
    {
        [Export] public AudioStreamPlayer Player { get; set; }

        private float _pulseHz = 440.0f; // The frequency of the sound wave.
        private float _sampleHz;
        private AudioStreamGeneratorPlayback _playback; //Will hold the AudioStreamGeneratorPlayback.

        public override void _Ready()
        {
            base._Ready();
            if (Player.Stream is AudioStreamGenerator generator) //need to type as a generator so you can access MixRate
            {
                _sampleHz = generator.MixRate;
                Player.Play();
                _playback = (AudioStreamGeneratorPlayback)Player.GetStreamPlayback();
                FillBuffer();
            }
        }

        public void FillBuffer()
        {
            double phase = 0.0;
            float increment = _pulseHz / _sampleHz;
            int framesAvailable = _playback.GetFramesAvailable();

            for (int i = 0; i < framesAvailable; i++)
            {
                _playback.PushFrame(Vector2.One * (float)Mathf.Sin(phase * Mathf.Tau));
                phase = Mathf.PosMod(phase + increment, 1.0);
            }
        }
    }
}