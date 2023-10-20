using Godot;

namespace SpookyBotanyGame.UI.Widgets
{
    public partial class ImageSequenceFrame : Resource
    {
        [Export] public float Time { get; set; } = 0.1f;
        [Export] public Texture2D Texture2D { get; set; }
    }
}