using Godot;

namespace SpookyBotanyGame.UI.Widgets
{
    [GlobalClass]
    public partial class InputUIAnimationViewData : Resource
    {
        [Export] 
        public ImageSequenceFrame[] PressedFrames
        {
            get;
            set;
        }
        
        [Export] 
        public Texture2D IdleFrame {
            get;
            set;
        }
    }
}
