using Godot;
using SpookyBotanyGame.World.Entities.Properties;

namespace SpookyBotanyGame.World.Entities;

[GlobalClass]
public partial class InputDirectionalPointing : Node2D
{
    [Export] public float Distance { get; set; }
    [Export] public Node2D Controlled { get; set; }
    
    private Vector2 _offset;
    
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
        _offset = _angle.Normalized() * Distance;
        Controlled.Position = _offset;
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