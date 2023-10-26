using Godot;

namespace SpookyBotanyGame.UI.Widgets;

[GlobalClass]
public partial class InputUIKeyboardViewData : Resource
{
    [Export] public InputUIAnimationViewData Standard { get; set; }
    [Export] public InputUIAnimationViewData Wide { get; set; }
    [Export] public InputUIAnimationViewData Space { get; set; }
    
    [Export] public Font KeyLabelFont { get; set; }
}