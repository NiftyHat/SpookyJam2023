using Godot;
using System;

public partial class TweenLightEnergy : Sprite2D
{
    [Export] public float Min { get; set; }
    [Export] public float Max { get; set; }

    public override void _Ready()
    {
        base._Ready();
        Tween tween = CreateTween();
        foreach (Node light in GetChildren())
        {
            tween.TweenProperty(light, "energy", 0.4f, 0.1f);
            tween.TweenProperty(light, "energy", 0.5f, 1f);
        }

        tween.SetLoops();
        tween.Play();
            
    }
}
