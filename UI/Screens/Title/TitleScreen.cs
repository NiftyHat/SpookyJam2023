using Godot;
using SpookyBotanyGame.UI;

[GlobalClass]
public partial class TitleScreen : Control
{
    [Export] public Button ButtonPlay { get; set; }
    [Export] public Button ButtonQuit { get; set; }

    protected UIController _controller;

    public override void _Ready()
    {
        base._Ready();
        ButtonPlay.ButtonUp += HandlePlayButtonUp;
        ButtonQuit.ButtonUp += HandleQuitButtonUp;
        _controller = GetNode<UIController>("/root/UIController");
    }

    private void HandleQuitButtonUp()
    {
        _controller.ApplicationQuit();
    }

    private void HandlePlayButtonUp()
    {
        _controller.ApplicationPlay();
    }
}
