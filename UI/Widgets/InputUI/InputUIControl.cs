using System.Linq;
using Godot;
using Godot.Collections;

namespace SpookyBotanyGame.UI.Widgets
{
   
    [GlobalClass]
    public partial class InputUIControl : Control
    {
        [Export] public InputUITweenAnimator Animator { get; set; }
        [Export] public string ActionName { get; set; }
        [Export] public Label LabelKey { get; set; }
        
        [Export] public InputUIKeyboardViewData KeyboardViewData { get; set; }
        [Export] public InputUIJoypadViewData XBox { get; set; }
        [Export] public InputUIJoypadViewData Playstation { get; set; }
        [Export] public InputUIJoypadViewData Switch { get; set; }

        private long _currentDeviceId = 0;
        private string _joypadName;
        private Array<InputEvent> _events;

        private InputUIJoypadViewData _joypadViewData;
        private InputUIKeyboardViewData _keyboardViewData;

        public override void _Ready()
        {
            base._Ready();
            _keyboardViewData = KeyboardViewData;
            _events = InputMap.ActionGetEvents(ActionName);
            if (Input.GetConnectedJoypads().Count > 0)
            {
                var connectedJoypad = Input.GetConnectedJoypads().First();
                SetDeviceId(connectedJoypad);
            }
            else
            {
                SetDeviceId(-1);
            }
            Input.JoyConnectionChanged += HandleJoyConnectionChanged;
        }

        public override void _Input(InputEvent inputEvent)
        {
            base._Input(inputEvent);
            if (inputEvent is InputEventKey key)
            {
                SetDeviceId(-1);
            }
            else if (inputEvent is InputEventJoypadButton joyButton)
            {
                SetDeviceId(inputEvent.Device);
            }
            if (inputEvent.IsAction(ActionName))
            {
                Animator.Pressed();
            }
        }

        private void HandleJoyConnectionChanged(long device, bool connected)
        {
            if (connected)
            {
                SetDeviceId(device);
            }
            else
            {
                SetDeviceId(-1);
            }
        }

        private void SetDeviceId(long deviceId)
        {
            _currentDeviceId = deviceId;
            bool isKeyboard = _currentDeviceId == -1;
            foreach (var item in _events)
            {
                if (item is InputEventKey key)
                {
                    LabelKey.Text = OS.GetKeycodeString(key.PhysicalKeycode);
                    Animator.Set(_keyboardViewData.Standard);
                    LabelKey.Visible = true;
                }

                if (item is InputEventJoypadButton joypadButton && !isKeyboard)
                {
                    _joypadName = Input.GetJoyName((int)_currentDeviceId);
                    _joypadViewData = GetJoypadViewData(_joypadName);
                    var button = _joypadViewData.GetButton(joypadButton.ButtonIndex);
                    if (button != null)
                    {
                        Animator.Set(button);
                    }
                    LabelKey.Visible = false;
                }
            }
        }

        private InputUIJoypadViewData GetJoypadViewData(string joyName)
        {
            switch (joyName)
            {
                case "XInput Gamepad":
                    return XBox;
                default:
                    return null;
            }
        }
    }
}

