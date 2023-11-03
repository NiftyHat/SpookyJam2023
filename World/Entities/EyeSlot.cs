using Godot;
using SpookyBotanyGame.Collectable;
using SpookyBotanyGame.World.Entities.Properties;

namespace SpookyBotanyGame.World.Entities;

[GlobalClass]
public partial class EyeSlot : GameEntity
{
    [Export] public AnimationPlayer SlotAnimation { get; set; }
    [Export] public Interactable Interactable { get; set; }
    [Export] public CollectableResource CollectableType { get; set; }

    private CollectableStackSlot<CollectableResource> _slot = new CollectableStackSlot<CollectableResource>();

    public override void _Ready()
    {
        base._Ready();
        Interactable.OnInteractionTriggered += HandleInteractionTriggered;
        _slot.OnChanged += HandleSlotChanged;
        SlotAnimation.HasAnimation("Added");
        SlotAnimation.HasAnimation("Removed");
    }

    public override string[] _GetConfigurationWarnings()
    {
        if (SlotAnimation == null)
        {
            return new[] { "Missing SlotAnimation" };
        }
        if (!SlotAnimation.HasAnimation("Added"))
        {
            return new[] { "Missing animation 'Added'" };
        }
        if (!SlotAnimation.HasAnimation("Removed"))
        {
            return new[] { "Missing animation 'Removed'" };
        }
        return base._GetConfigurationWarnings();
    }

    private void HandleSlotChanged(int newAmount, int oldAmount)
    {
        if (newAmount > 0 && oldAmount == 0)
        {
            SlotAnimation.Play("Added");
        }
        else if (newAmount == 0 && oldAmount > 0)
        {
            SlotAnimation.Play("Removed");
        }
    }

    private bool HandleInteractionTriggered(GameEntity other, GameEntity self)
    {
        if (!_slot.IsFull)
        {
            return false;
        }
        if (other is PlayerEntity player && player.Inventory != null && player.Inventory.TryGetSlot(CollectableType, out var slot) && slot.Amount > 0)
        {
            slot.Remove(1);
            _slot.Add(1);
            Interactable.OnInteractionTriggered -= HandleInteractionTriggered;
            return true;
        }
        return false;
    }
}