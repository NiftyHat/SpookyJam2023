using Godot;

namespace SpookyBotanyGame.Core.InputMapping
{
    public partial class ControllerInputButtonResource : Godot.Resource
    {
        [Export] 
        public Texture2D[] Frames
        {
            get;
            set;
        }
        
        [Export] private string Glyph { get; set; }
        
        public ControllerInputButtonResource(Texture2D[] frames, string glyph)
        {
            Frames = frames;
        }
    }
}