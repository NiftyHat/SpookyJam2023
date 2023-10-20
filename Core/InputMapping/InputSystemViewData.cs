using Godot;
using Godot.Collections;

namespace SpookyBotanyGame.Core.InputMapping
{
    public partial class InputSystemViewData : Resource
    {
        [Export] private Dictionary<int, ControllerInputButtonResource> _buttons;
    }
}

