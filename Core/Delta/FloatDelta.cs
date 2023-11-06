using System;

namespace SpookyBotanyGame.Core.Delta;

public struct FloatDelta
{
    public float Last;
    public float Delta;
    
    public event Action<float> OnChanged;

    public void Update(float current)
    {
        if (Math.Abs(Last - current) > 0.00001d)
        {
            Delta = Last - current;
            Last = current;
            OnChanged?.Invoke(current);
        }
    }
}