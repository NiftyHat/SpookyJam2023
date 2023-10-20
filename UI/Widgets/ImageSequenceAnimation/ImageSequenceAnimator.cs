using System.Collections.Generic;
using Godot;

namespace SpookyBotanyGame.UI.Widgets
{
    public partial class ImageSequenceAnimator : Node
    {
        [Export] 
        public Dictionary<string, ImageSequenceAnimationData> Animations
        {
            get;
            set;
        }

        public Texture2D Texture
        {
            get;
            set;
        }

        private ImageSequenceAnimationData _animationData;
        private int _frame = 0;
        double _elapsed;

        public void Play(string animationName)
        {
            if (Animations.TryGetValue(animationName, out _animationData))
            {
                _elapsed = 0;
            }
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            if (_animationData != null)
            {
                _elapsed += delta;
                _animationData.GetFrame(_elapsed);
            }
        }
    }
}

