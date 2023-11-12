using Godot;

namespace SpookyBotanyGame.Core.Animation;

[GlobalClass]
public partial class ShaderParamBindingFloat : ShaderParamBinding
{
    [Export] private float Value;
    protected override Variant GetValue()
    {
        return Value;
    }
}