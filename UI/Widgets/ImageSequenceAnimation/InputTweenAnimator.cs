using System.Collections.Generic;
using Godot;

namespace SpookyBotanyGame.UI.Widgets
{
    public partial class InputTweenAnimator : Node
    {
        [Export] 
        public InputUIAnimationViewData ViewData
        {
            get;
            set;
        }

        public TextureRect TextureRect
        {
            get;
            set;
        }
        
        private int _frame = 0;
        double _elapsed;
        private Tween _tweenPressed;

        public override void _Ready()
        {
            base._Ready();
            if (TextureRect != null)
            {
                TextureRect.Texture = ViewData.IdleFrame;
            }
        }

        public override void _Process(double delta)
        {
            
        }
        
        public Tween GetTweenPressed(ImageSequenceFrame frames)
        {
            Tween tween = GetTree().CreateTween();
            tween.TweenProperty(TextureRect, "Texture",  )
        }
        
        /*
        public Animation GetAnimation(ImageSequenceAnimationData data)
        {
            if (data == null)
            {
                return null;
            }
            if (data.Frames == null || data.Frames.Length == 0)
            {
                return null;
            }
            var animation = new Animation();
            int frameCount = data.Frames.Length;
            int idleTrackIndex = animation.AddTrack(Animation.TrackType.Method);
            animation.TrackSetPath(idleTrackIndex, "TextureRect:Texture");
            animation.TrackInsertKey(idleTrackIndex, 0, data.Frames[0].Texture2D);
            if (frameCount > 1)
            {
                for (int i = 2; i < frameCount; i++)
                {
                    int pressedTrackIndex = animation.AddTrack(Animation.TrackType.Method);
                    animation.TrackSetPath(pressedTrackIndex, "TextureRect:Texture");
                    animation.TrackInsertKey(idleTrackIndex, 0, data.Frames[i].Texture2D);
                }
            }

        }*/
    }
}

