using Godot;
using SpookyBotanyGame.Core;

namespace SpookyBotanyGame.World.Entities.Animation;

public partial class LookDirectionAnimationPlayer : Node2D, IEnabled
{
    [Export] public AnimationPlayer Animation { get; set; }
    
    private Vector2? _targetPosition;
    private bool _isEnabled;

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (Animation == null || !_isEnabled)
        {
            return;
        }
        if (_targetPosition.HasValue)
        {
            var lookDirection = GetAnimationDirection(_targetPosition.Value);
            if (lookDirection != null)
            {
                Animation.Play("Look-" + lookDirection.Name);
            }
            else
            {
                Animation.Play("Look-None");
            }
        }
        else
        {
            Animation.Play("Look-None");
        }
    }

    private Direction GetAnimationDirection(Vector2 targetPosition)
    {
        var directionRadians = (this.GlobalPosition - targetPosition).Angle();
        var segmentSize = Mathf.Pi / 4;
        var offset = directionRadians + (segmentSize / 8);
        int segment = Mathf.FloorToInt(offset / segmentSize);
        switch (segment)
        {
            case -4:
                return Direction.East;
            case -3:
                return Direction.SouthEast;
            case -2:
                return Direction.South;
            case -1:
                return Direction.SouthWest;
            case 0:
                return Direction.West;
            case 1:
                return Direction.NorthWest;
            case 2:
                return Direction.North;
            case 3:
                return Direction.NorthEast;
            case 4:
                return Direction.East;
        }
        /*
        if (offset <= segmentSize)
            return Direction.East;
        if (offset <= segmentSize * 2)
            return Direction.SouthEast;
        if (offset <= segmentSize * 3)
            return Direction.South;
        if (offset <= segmentSize * 4)
            return Direction.SouthWest;
        if (offset <= segmentSize * 5)
            return Direction.West;
        if (offset <= segmentSize * 6)
            return Direction.NorthWest;
        if (offset <= segmentSize * 7)
            return Direction.North;
        if (offset <= segmentSize * 8)
            return Direction.NorthEast;*/
        return null;
    }

    public void SetTarget(Vector2 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    public void ClearTarget()
    {
        _targetPosition = null;
    }

    public bool IsEnabled => _isEnabled;

    public void SetEnabled(bool isEnabled)
    {
        _isEnabled = isEnabled;
    }
}