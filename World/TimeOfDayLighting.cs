using Godot;
using System;

public partial class TimeOfDayLighting : CanvasModulate
{
    private float _time;
    
    [Export]
    public float TimeScale { get; set; }
    
    public override void _Process(double delta)
    {
        /*
        base._Process(delta);
        _time += (float)delta * TimeScale;
        float c = MathF.Abs(MathF.Sin(_time));
        this.Color = new Color(c, c, c);*/
    }
}
