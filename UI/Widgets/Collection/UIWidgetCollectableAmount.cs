using Godot;
using SpookyBotanyGame.Collectable;
using Container = System.ComponentModel.Container;

namespace SpookyBotanyGame.UI.Widgets.Collection;

public class UIWidgetCollectableAmount : Container
{
    [Export] public CollectableResource _collectableType { get; set; }

    [Export] public Texture2D _icon { get; set; }

    public void Set(CollectableAmountSlot slot)
    {
        
    }
}