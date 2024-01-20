namespace SpookyBotanyGame.Core.Bindable;

public interface IBindable<TValue>
{
    public delegate void OnChanged(TValue value);

    public event OnChanged OnChange;
    
    public TValue Value { get; }
    
    public void Set(TValue value);
    public void Bind(OnChanged onChanged);
    public void BindOnce(OnChanged onChanged);
}

public interface IBindable<TValue1, TValue2> 
{
    public delegate void OnChanged(TValue1 value, TValue2 value2);

    public event OnChanged OnChange;
    
    public void Set(TValue1 value, TValue2 value2);
    public void Bind(OnChanged onChanged);
}