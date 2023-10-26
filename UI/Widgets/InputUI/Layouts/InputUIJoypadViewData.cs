using Godot;

namespace SpookyBotanyGame.UI.Widgets;

[GlobalClass]
public partial class InputUIJoypadViewData : Resource
{
    [Export] public InputUIAnimationViewData Button_East { get; set; }
    [Export] public InputUIAnimationViewData Button_North { get; set; }
    [Export] public InputUIAnimationViewData Button_South { get; set; }
    [Export] public InputUIAnimationViewData Button_West { get; set; }

    [Export] public InputUIAnimationViewData Dpad_East { get; set; }
    [Export] public InputUIAnimationViewData Dpad_North { get; set; }
    [Export] public InputUIAnimationViewData Dpad_South { get; set; }
    [Export] public InputUIAnimationViewData Dpad_West { get; set; }
    
    [Export] public InputUIAnimationViewData Button_Start { get; set; }
    [Export] public InputUIAnimationViewData Button_Back { get; set; }
    [Export] public InputUIAnimationViewData Button_Guide { get; set; }
    
    [Export] public InputUIAnimationViewData Button_LeftStick { get; set; }
    [Export] public InputUIAnimationViewData Button_RightStick { get; set; }
    
    [Export] public InputUIAnimationViewData Shoulder_L1 { get; set; }
    [Export] public InputUIAnimationViewData Shoulder_L2 { get; set; }
    [Export] public InputUIAnimationViewData Shoulder_R1 { get; set; }
    [Export] public InputUIAnimationViewData Shoulder_R2 { get; set; }
    

    public InputUIAnimationViewData GetButton(JoyButton button)
    {
        GD.Print("Get button " + button);
        switch (button)
        {
            case JoyButton.A:
                return Button_South;
            case JoyButton.B:
                return Button_West;
            case JoyButton.X:
                return Button_East;
            case JoyButton.Y:
                return Button_South;
            case JoyButton.Back:
                return Button_Back;
            case JoyButton.Guide:
                return Button_Guide;
            case JoyButton.Start:
                return Button_Start;
            case JoyButton.DpadDown:
                return Dpad_South;
            case JoyButton.DpadUp:
                return Dpad_North;
            case JoyButton.DpadLeft:
                return Dpad_West;
            case JoyButton.DpadRight:
                return Dpad_East;
            case JoyButton.RightShoulder:
                return Shoulder_R1;
            case JoyButton.LeftShoulder:
                return Shoulder_L1;
            case JoyButton.LeftStick:
                return Button_LeftStick;
            case JoyButton.RightStick:
                return Button_RightStick;
        }
        return null;
    }

    public InputUIAnimationViewData GetAxis(JoyAxis axis)
    {
        switch (axis)
        {
            case JoyAxis.TriggerLeft:
                return Shoulder_L2;
            case JoyAxis.TriggerRight:
                return Shoulder_R2;
        }

        return default;
    }
}