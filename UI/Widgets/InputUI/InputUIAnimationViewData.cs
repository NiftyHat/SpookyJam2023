using Godot;

namespace SpookyBotanyGame.UI.Widgets
{
    [GlobalClass]
    public partial class InputUIAnimationViewData : Resource
    {
        [Export] 
        public Texture2D[] PressedFrames
        {
            get;
            set;
        }
        
        [Export] 
        public Godot.Collections.Array<InputUIAnimationFrameData> PressedFrameData
        {
            get;
            set;
        }
        
        [Export] 
        public Texture2D IdleTexture 
        {
            get;
            set;
        }
    }
}
