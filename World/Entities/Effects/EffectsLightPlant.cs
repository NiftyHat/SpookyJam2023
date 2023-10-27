using Godot;

namespace SpookyBotanyGame.World.Entities.Effects;

[GlobalClass]
public partial class EffectsLightPlant : Node2D
{
    [Export] public Light2D Light { get; set; }
    [Export] public double ScaleMax { get; set; }
    [Export] public double EnergyMax { get; set; }
    [Export(PropertyHint.Range, "0,1")] private float LitBoostPercentage { get; set; }

    private float _defaultScale;
    private float _defaultEnergy;

    private bool _isLit;
    private float _scale;
    private double _isLitBoost;

    public override void _Ready()
    {
        base._Ready();
        Light.Energy = 0;
        Light.Scale = Vector2.Zero;
    }

    public void SetIsLit(bool isLit)
    {
        _isLit = isLit;
    }

    public void SetScale(float percentage)
    {
        _scale = percentage;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (_isLit)
        {
            if (_isLitBoost < 1.0f)
            {
                _isLitBoost += delta * 0.5f;
            }
            else
            {
                _isLitBoost = 1;
            }
        }
        else
        {
            if (_isLitBoost > 0)
            {
                _isLitBoost -= delta;
            }
        }
        float totalScale = _scale + ((float)_isLitBoost * LitBoostPercentage);
        Light.Energy = (float)(EnergyMax * totalScale);
        Light.Scale = Vector2.One * totalScale;
    }
}