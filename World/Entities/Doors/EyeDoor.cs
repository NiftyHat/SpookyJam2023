using Godot;
using Godot.Collections;

namespace SpookyBotanyGame.World.Entities.Doors;
[GlobalClass]
public partial class EyeDoor : Node2D
{
    [Export] public Array<EyeSlot> OpeningEyeSlots { get; set; }
    [Export] public AnimationPlayer Animation { get; set; }

    public override void _Ready()
    {
        base._Ready();
        InitAnimationState();
    }

    private void InitAnimationState()
    {
        bool areAllFilled = true;
        foreach (var item in OpeningEyeSlots)
        {
            if (item.IsFilled == false)
            {
                areAllFilled = false;
            }
            item.OnFilledChanged += HandleFilledChanged;
        }

        if (areAllFilled)
        {
            Animation.Play("Opened");
        }
        else
        {
            Animation.Play("Closed");
        }
    }

    private void HandleFilledChanged(bool obj)
    {
        bool areAllFilled = true;
        foreach (var item in OpeningEyeSlots)
        {
            if (item.IsFilled == false)
            {
                areAllFilled = false;
            }
        }
        GD.Print(Name, $"all filled {areAllFilled}");
        if (areAllFilled)
        {
            Animation.Play("Open");
        }
        else
        {
            Animation.Play("Close");
        }
    }
}