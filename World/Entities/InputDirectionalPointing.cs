using System;
using Godot;
using SpookyBotanyGame.World.Entities.Properties;

namespace SpookyBotanyGame.World.Entities;

[GlobalClass]
public partial class InputDirectionalPointing : Node2D
{
    [Export] public float Distance { get; set; }
    [Export] public Node2D Controlled { get; set; }
    [Export] public RayCast2D RayCast { get; set; }

    public event Action<Vector2, Vector2> OnDirectionChanged;
    
    private Vector2 _relativePosition;
    
    private bool _isEnabled = true;
    private bool _useMouse = true;
    
    private Vector2 _angle;

    public override void _Ready()
    {
        base._Ready();
        Distance = (this.Position - Controlled.Position).Length();
    }
    
    public override void _Process(double delta)
    {
        base._Process(delta);
        if (_useMouse)
        {
            _angle = AngleToMouse();
        }
        var normalized = _angle.Normalized();
        _relativePosition = normalized  * Distance;
        if (RayCast != null)
        {
            RayCast.TargetPosition = _relativePosition;
            Vector2 collisionPoint = RayCast.GetCollisionPoint();
            Vector2 relativeCollisionPoint = this.GlobalPosition - collisionPoint;
            float distanceToCollision = relativeCollisionPoint.Length();
            if (distanceToCollision <= Distance)
            {
                _relativePosition = relativeCollisionPoint * 0.5f;
            }
        }
        Controlled.Position = _relativePosition;
        Controlled.Rotation = -normalized.AngleTo(Vector2.Right);
        OnDirectionChanged?.Invoke(normalized, _relativePosition);
    }
    
    protected Vector2 AngleToMouse()
    {
        var mousePosition = GetLocalMousePosition();
        var pivotPosition = Vector2.Zero;
        return (mousePosition - pivotPosition);
    }

    public override void _Input(InputEvent inputEvent)
    {
        switch (inputEvent)
        {
            case InputEventKey eventKey:
                _useMouse = true;
                break;
            case InputEventJoypadButton joyButton:
                _useMouse = false;
                break;
            case InputEventJoypadMotion joyMotion:
                _useMouse = false;
                Vector2 lookAxis = PlayerInputControlled.GetLookStickAxis();
                if (lookAxis != Vector2.Zero)
                {
                    _angle = PlayerInputControlled.GetLookStickAxis();
                }
                break;
        }
        base._Input(inputEvent);
    }
}