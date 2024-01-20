using Godot;

namespace SpookyBotanyGame.UI;

[GlobalClass]
public partial class UIData : Resource
{
    [Export] public PackedScene UIRoot { get; set; }
    [Export] public UISceneData Scenes { get; set; }
}