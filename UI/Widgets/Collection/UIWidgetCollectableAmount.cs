using Godot;
using SpookyBotanyGame.Collectable;

namespace SpookyBotanyGame.UI.Widgets.Collection
{
    [GlobalClass]
    public partial class UIWidgetCollectableAmount : Container
    {
        [Export] public CollectableResource _collectableType { get; set; }
        [Export] public Texture2D _icon { get; set; }
        [Export] public Label _labelAmount { get; set; }
        [Export] public TextureProgressBar _progressBar { get; set; }

        private int _max;
        private int _value;
        private CollectableStackSlot<ICollectableType> _slot;

        public void Set(CollectableStackSlot<ICollectableType> slot)
        {
            _slot = slot;
            slot.OnChanged += HandleSlotAmountChanged;
            slot.OnMaxChanged += HandleSlotMaxChanged;
        }

        private void HandleSlotMaxChanged(int newValue, int oldValue)
        {
            _max = newValue;
            _labelAmount.Text = $"{_value}/{_slot.Max}";
            _progressBar.Value = _max;
        }

        private void HandleSlotAmountChanged(int newValue, int oldValue)
        {
            _value = newValue;
            _labelAmount.Text = $"{_value}/{_slot.Max}";
            _progressBar.Value = _value;
        }
    }
}

