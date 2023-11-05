using Godot;

[GlobalClass]
public partial class LanternPointLight : Node2D
{
    public struct Defaults
    {
        public Color Color;
        public float Scale;
        public float Energy;
    }
    [Export] private PointLight2D SpriteLight { get; set; }
    [Export] private PointLight2D ShadowLight { get; set; }

    public Defaults Default;

    public override void _Ready()
    {
        Default = new Defaults()
        {
            Color = SpriteLight.Color,
            Scale = SpriteLight.TextureScale,
            Energy = SpriteLight.Energy
        };
        base._Ready();
    }

    public void SetEnergy(float newEnergy)
    {
        ShadowLight.Energy = newEnergy;
        SpriteLight.Energy = newEnergy;
    }

    public void SetTexture(Texture2D texture2D)
    {
        ShadowLight.Texture = texture2D;
        SpriteLight.Texture = texture2D;
        SpriteLight.QueueRedraw();
        ShadowLight.QueueRedraw();
    }
    
    public void SetScale(float newScale)
    {
        ShadowLight.TextureScale = newScale;
        SpriteLight.TextureScale = newScale;
        SpriteLight.QueueRedraw();
        ShadowLight.QueueRedraw();
    }

    public void Reset()
    {
        SetScale(Default.Scale);
        SetEnergy(Default.Energy);
        SpriteLight.Color = Default.Color;
        ShadowLight.Color = Default.Color;
    }
}
