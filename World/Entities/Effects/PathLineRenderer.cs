using Godot;
using SpookyBotanyGame.Core;

namespace SpookyBotanyGame.World.Entities.Effects;

[GlobalClass][Tool]
public partial class PathLineRenderer : Path2D, IEnabled
{
    public bool IsEnabled { get; private set; } = true;
    [Export] public Node2D Target { get; set; }
    [Export] public Line2D Line { get; set; }

    public override void _Ready()
    {
        if (Target == null)
        {
            SetEnabled(false);
        }
        base._Ready();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (!IsEnabled)
        {
            return;
        }
        if (Curve == null || Curve.PointCount <= 1)
        {
            return;
        }
        
        
        if (Target != null && GetParent() != null)
        {
            Vector2 relativePosition = Target.GlobalPosition - GlobalPosition;
            int lastPointIndex = Curve.PointCount - 1;
            Vector2 dir = Vector2.FromAngle(Target.Rotation) * 30f;
            Curve.SetPointPosition(lastPointIndex,relativePosition);
            Curve.SetPointOut(lastPointIndex,dir);
        }
       
        if (Line == null)
        {
            GD.Print(nameof(_Process), "No Line");
            return;
        }
        Line.ClearPoints();
        foreach (var point in Curve.GetBakedPoints())
        {
            Line.AddPoint(point);
        }
    }

    public void SetTarget(Node2D target)
    {
        Target = target;
        if (Target == null)
        {
            SetEnabled(false);
        }
    }
    
    public void SetEnabled(bool isEnabled)
    {
        IsEnabled = isEnabled;
        Visible = isEnabled;
    }
}