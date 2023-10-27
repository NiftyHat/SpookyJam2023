using Godot;

namespace SpookyBotanyGame.UI.Widgets
{
    [GlobalClass]
    public partial class InputUIAnimationFrameData : Resource
    {
        [Export] public float Duration { get; set; } = 0.1f;
        [Export] public Vector2 LabelOffset { get; set; }

        public InputUIAnimationFrameData(float duration, Vector2 labelOffset)
        {
            Duration = duration;
            LabelOffset = labelOffset;
        }
    }
}