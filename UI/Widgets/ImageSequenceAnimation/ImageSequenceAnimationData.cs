using Godot;

namespace SpookyBotanyGame.UI.Widgets
{
    public partial class ImageSequenceAnimationData : Resource
    {
        
        [Export] 
        public ImageSequenceFrame[] Frames
        {
            get;
            set;
        }

        private Texture2D _frame;
        private double _current;
        private double _next;

        public Texture2D GetFrame(double elapsed)
        {
            if (elapsed < _next)
            {
                return _frame;
            }

            foreach (var item in Frames)
            {
                
            }

            return null;
        }
    }
}
