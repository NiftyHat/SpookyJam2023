using Godot;
using SpookyBotanyGame.Core.Bindable;

namespace SpookyBotanyGame.Core;

public class VariantBinding
{
    public Variant Value { get; protected set; }

    public VariantBinding()
    {
        
    }
    public VariantBinding(Variant variant)
    {
        Value = variant;
    }

    public virtual void Set(Variant variant)
    {
        if (!Value.Equals(variant))
        {
            Value = variant;
        }
    }
    
    public void Add<[MustBeVariant] TValue>(IBindable<TValue> bindable)
    {
        bindable.Set(Value.As<TValue>());
        bindable.OnChange += HandleBindableChanged;
    }

    private void HandleBindableChanged<[MustBeVariant] TValue>(TValue value)
    {
        var variant = Variant.From(value);
        Set(variant);
    }
}

public class VariantBinding<[MustBeVariant] TValue> : VariantBinding
{
    public VariantBinding(IBindable<TValue> bindable)
    {
        Add(bindable);
    }
}