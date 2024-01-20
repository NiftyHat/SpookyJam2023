using Godot;
using SpookyBotanyGame.UI.Screens.Settings;
using SpookyBotanyGame.World.Systems;

namespace SpookyBotanyGame.UI.Screens.Pause;

[GlobalClass]
public partial class PauseScreen : Control
{
    private UIController _controller;
    [Export] public Button ResumeButton { get; set; }
    [Export] public Button OptionsButton { get; set; }
    [Export] public Button ExitButton { get; set; }

    public override void _Ready()
    {
        ResumeButton.Pressed += HandleResumeButtonPressed;
        OptionsButton.Pressed += HandleOptionsButtonPressed;
        ExitButton.Pressed += HandleExitButtonPressed;
        _controller = GetNode<UIController>("/root/UIController");
        base._Ready();
    }

    private void HandleExitButtonPressed()
    {
        _controller.ApplicationHome();
        GetParent().RemoveChild(this);
    }

    private void HandleOptionsButtonPressed()
    {
        _controller.Open<SettingsScreen>();
    }

    private void HandleResumeButtonPressed()
    {
        ResumeSim();
        GetParent().RemoveChild(this);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (Input.IsActionJustReleased("pause_menu"))
        {
            //ResumeSim();
        }
    }

    private void ResumeSim()
    {
        var sim = GetNode<SimSystem>("/root/SimSystem");
        if (sim != null && sim.IsPaused)
        {
            sim.Resume();
        }
    }
}