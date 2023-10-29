using Godot;

namespace SpookyBotanyGame.World.Entities.Animation;

[GlobalClass][Tool]
public partial class PolygonCollisionSequencePlayer : Node2D
{
    [Export] public CollisionPolygon2D[] PolygonList { get; set; }
    [Export] public int Frame
    {
        get => _frame;
        set => SetFrame(value);
    }

    private void SetFrame(int value)
    {
        if (value == -1)
        {
            return;
        }
        if (value != _frame)
        {
            if (_frame >= 0 && _frame < PolygonList.Length)
            {
                CollisionPolygon2D currentPoly = PolygonList[_frame];
                if (currentPoly != null)
                {
                    PolygonExitFrame(currentPoly);
                }
            }

            if (value >= 0 && value < PolygonList.Length)
            {
                CollisionPolygon2D nextPoly = PolygonList[value];
                if (nextPoly != null)
                {
                    PolygonEnterFrame(nextPoly);
                }
            }

            _frame = value;
        }
    }

    private int _frame;

    public override void _Ready()
    {
        base._Ready();
        foreach (var item in PolygonList)
        {
            item.Disabled = true;
            item.Visible = false;
        }
    }

    private void PolygonEnterFrame(CollisionPolygon2D polygon2D)
    {
        polygon2D.Disabled = false;
        polygon2D.Visible = true;
    }
    
    private void PolygonExitFrame(CollisionPolygon2D polygon2D)
    {
        polygon2D.Disabled = true;
        polygon2D.Visible = false;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }
}