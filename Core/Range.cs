using System;

namespace SpookyBotanyGame.Core
{

    public static class Range
    {
        public static float Percentage(Range<float> range)
        {
            if (range.Min == 0 && range.Max == 0)
            {
                return 1.0f;
            }
            if (range.Min == 0)
            {
                return 1.0f / (range.Max - range.Min) * range.Value;
            }
            return 0;
        }
        
        public static double Percentage(Range<double> range)
        {
            if (range.Min == 0 && range.Max == 0)
            {
                return 1.0f;
            }
            if (range.Min == 0)
            {
                return 1.0d / (range.Max - range.Min) * range.Value;
            }
            return 0;
        }
    }
    public class Range<TValue> where TValue : IComparable<TValue>
    {
        public TValue Min { get; private set; }
        public TValue Max { get; private set; }
    
        public bool IsMin => _isMin;
        public bool IsMax => _isMax;

        public bool IsZero => _value.Equals(0);
    
        private bool _isMin;
        private bool _isMax;
    
        public delegate void OnHitThreshold(TValue value);
    
        public event OnHitThreshold OnMin;
        public event OnHitThreshold OnMax;
    
        public TValue Value
        {
            get => _value;
            set => SetValue(value);
        }
        private TValue _value;
        
        public Range(TValue max, TValue min = default, TValue initial = default)
        {
            if (max.CompareTo(min) < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(min), $"Range ctor {nameof(max)} of {max} cannot be less than {nameof(min)} value of {min}");
            }
            Min = min;
            Max = max;
            if (_value.CompareTo(max) > 0)
            {
                _value = max;
            }
            if (_value.CompareTo(min) < 0)
            {
                _value = min;
            }
            _value = initial;
        }
    
        public void SetValue(TValue value)
        {
            _value = value;
            if (_value.Equals(Min) || _value.CompareTo(Min) < 0)
            {
                _value = Min;
                if (!_isMin)
                {
                    _isMin = true;
                    OnMin?.Invoke(_value);
                }
            }
            else
            {
                _isMin = false;
            }
    
            if (_value.Equals(Max) || _value.CompareTo(Max) > 0)
            {
                _value = Max;
                if (!_isMax)
                {
                    OnMax?.Invoke(_value);
                    _isMax = true;
                }
            }
            else
            {
                _isMax = false;
            }
        }


    }
}

