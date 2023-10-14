using SpookyBotanyGame.Core;

namespace SpookyBotanyGame.Collectable;

public class CollectableAmountSlot
{
    private int _count;
    
    public delegate void OnChanged(int newCount, int oldCount);

    public delegate void OnEmpty();
    public delegate void OnFull();

    public event OnChanged OnChange;

    public ICollectableType _type;

    public CollectableAmountSlot(ICollectableType collectableType)
    {
        _type = collectableType;
    }
    
    public void Add(int amount)
    {
        if (amount <= 0)
        {
            return;
        }
        int oldCount = _count;
        _count += amount;
        OnChange?.Invoke(_count, oldCount);
    }

    public void Remove(int amount)
    {
        if (amount <= 0)
        {
            return;
        }
        int oldCount = _count;
        _count -= amount;
        OnChange?.Invoke(_count, oldCount);
    }

    public void Change(int amount)
    {
        int oldCount = _count;
        _count += amount;
        OnChange?.Invoke(_count, oldCount);
    }
}