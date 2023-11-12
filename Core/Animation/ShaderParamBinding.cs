using Godot;

namespace SpookyBotanyGame.Core.Animation;

[GlobalClass]
public abstract partial class ShaderParamBinding : Resource
{
    [Export] public string Name { get; set; }

    private ShaderMaterial _material;

    public void Bind(ShaderMaterial shader)
    {
        _material = shader;
    }

    public void Apply(ShaderMaterial shaderMaterial)
    {
        var value = GetValue();
        shaderMaterial.SetShaderParameter(Name, value);
    }

    protected abstract Variant GetValue();
}