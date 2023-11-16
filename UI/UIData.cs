using Godot;

namespace SpookyBotanyGame.UI;

[GlobalClass]
public partial class UIData : Resource
{
    [Export] public UISceneData Scenes { get; set; }
}