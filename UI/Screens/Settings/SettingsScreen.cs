using Godot;

namespace SpookyBotanyGame.UI.Screens.Settings
{
    [GlobalClass]
    public partial class SettingsScreen : Control
    {
        private SettingsController _controller;
        [Export] public Button ButtonConfirm { get; set; }
        [Export] public Button ButtonCancel { get; set; }

        public override void _Ready()
        {
            base._Ready();
            _controller = GetNode<SettingsController>("/root/SettingsController");
            ButtonConfirm.ButtonUp += HandleConfirmButtonUp;
            ButtonCancel.ButtonUp += HandleCancelButtonUp;
        }

        private void HandleCancelButtonUp()
        {
            GetParent().RemoveChild(this);
        }

        private void HandleConfirmButtonUp()
        {
            _controller?.Save();
            GetParent().RemoveChild(this);
        }
    }
}