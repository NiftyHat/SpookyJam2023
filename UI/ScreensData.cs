using Godot;

namespace SpookyBotanyGame.UI;

[GlobalClass]
public partial class ScreensData : Resource
{
    [Export] public PackedScene Title { get; set; }
    [Export] public PackedScene Game { get; set; }
    [Export] public PackedScene GameOver { get; set; }
    [Export] public PackedScene Options { get; set; }
}