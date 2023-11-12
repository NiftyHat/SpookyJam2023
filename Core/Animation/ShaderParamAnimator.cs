using Godot;
using Godot.Collections;

namespace SpookyBotanyGame.Core.Animation
{
    [Tool] [GlobalClass]
    public partial class ShaderParamAnimator : Node2D
    {
        [Export] public Array<ShaderParamBinding> Bindings { get; set; }
    }
}