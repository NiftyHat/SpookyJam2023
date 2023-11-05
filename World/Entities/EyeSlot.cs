using System;
using Godot;
using SpookyBotanyGame.Collectable;
using SpookyBotanyGame.World.Entities.Properties;

namespace SpookyBotanyGame.World.Entities;

[GlobalClass]
public partial class EyeSlot : GameEntity
{
    [Export] public AnimationPlayer SlotAnimation { get; set; }
    [Export] public Interactable Interactable { get; set; }
    [Export] public CollectableResource StoredCollectable { get; set; }

    public bool IsFilled => _slot != null && _slot.Amount > 0;

    private CollectableStackSlot<CollectableResource> _slot = new CollectableStackSlot<CollectableResource>(0,1);

    public event Action<bool> OnFilledChanged;

    public override void _Ready()
    {
        base._Ready();
        Interactable.OnInteractionTriggered += HandleInteractionTriggered;
        _slot.OnChanged += HandleSlotChanged;
        SlotAnimation.HasAnimation("Added");
        SlotAnimation.HasAnimation("Removed");
        SlotAnimation.AnimationFinished += HandleAnimationFinished;
    }

    private void HandleAnimationFinished(StringName animationName)
    {
        switch (animationName)
        {
            case "Added":
                SlotAnimation.Play("Full");
                Interactable.SetEnabled(true);
                OnFilledChanged?.Invoke(true);
                break;
            case "Removed":
                SlotAnimation.Play("Empty");
                Interactable.SetEnabled(true);
                OnFilledChanged?.Invoke(false);
                break;
        }
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
            Interactable.SetEnabled(false);
        }
        else if (newAmount == 0 && oldAmount > 0)
        {
            SlotAnimation.Play("Removed");
            Interactable.SetEnabled(false);
        }
    }

    private bool HandleInteractionTriggered(GameEntity other, GameEntity self)
    {
        if (other is PlayerEntity player && player.CarriedSlot != null)
        {
            bool hasItem = _slot.Amount > 0;
            if (hasItem && player.CarriedSlot.Amount == 0)
            {
                player.CarriedSlot.Add(StoredCollectable);
                _slot.Remove(1);
                return true;
            }
            if (!hasItem && player.CarriedSlot.Has(StoredCollectable, 1))
            {
                player.CarriedSlot.Remove(1);
                _slot.Add(StoredCollectable);
                return true;
            }
        }

        return false;
    }
}