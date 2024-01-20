using System;
using Godot;
using Godot.Collections;

namespace SpookyBotanyGame.UI.Screens.Settings;

public partial class WidgetControlToggleButtons : BoxContainer
{
    [Export] public Array<Control> Controls { get; set; }
    [Export] public Node ButtonContainer { get; set; }

    private Control _lastSelectedControl;

    public override void _Ready()
    {
        base._Ready();
        if (ButtonContainer != null)
        {
            var containerChildren = ButtonContainer?.GetChildren(false);
            int len = Controls.Count;
            for (int i = 0; i < len; i++)
            {
                var control = Controls[i];
                var child = containerChildren[i];
                if (_lastSelectedControl == null && control != null)
                {
                    _lastSelectedControl = control;
                    _lastSelectedControl.Visible = true;
                }
                if (child is Button button)
                {
                    button.Pressed += () => { HandleSelectControl(control);};
                }
            }
        }
    }

    private void HandleSelectControl(Control selectedControl)
    {
        if (_lastSelectedControl != selectedControl && _lastSelectedControl != null)
        {
            _lastSelectedControl.Visible = false;
        }
        if (selectedControl != null)
        {
            _lastSelectedControl = selectedControl;
            selectedControl.Visible = true;
        }
    }
}