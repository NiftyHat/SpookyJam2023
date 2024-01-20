using System;
using System.Linq;

namespace SpookyBotanyGame.Core.Bindable
{
    public abstract class BindableStruct<TValue> : IBindable<TValue>
    {
        public event IBindable<TValue>.OnChanged OnChange;
        protected TValue _value;
        public TValue Value => _value;

        public BindableStruct()
        {
        }
        
        public BindableStruct(IBindable<TValue>.OnChanged onChanged)
        {
            OnChange += onChanged;
        }
    
        public BindableStruct(TValue value)
        {
            _value = value;
        }
        public virtual void Set(TValue value)
        {
            if (!value.Equals(_value))
            {
                _value = value;
                OnChange?.Invoke(_value);
            }
        }

        public virtual void Bind(IBindable<TValue>.OnChanged change)
        {
            OnChange += change;
            change?.Invoke(_value);
        }

        public virtual void BindOnce(IBindable<TValue>.OnChanged change)
        {
            if (OnChange != null && OnChange.GetInvocationList().Contains(change))
            {
                return;
            }
            OnChange += change;
            change?.Invoke(_value);
        }
    }

    public class BindableStruct<TValue1, TValue2> : IBindable<TValue1, TValue2> where TValue1 : IComparable<TValue1> where TValue2 : IComparable<TValue2>
    {
        public event IBindable<TValue1, TValue2>.OnChanged OnChange;
        protected TValue1 _value1;
        protected TValue2 _value2;

        public BindableStruct()
        {
        }
    
        public BindableStruct(TValue1 value)
        {
            _value1 = value;
        }
    
        public BindableStruct(TValue1 value1, TValue2 value2)
        {
            _value1 = value1;
            _value2 = value2;
        }
    
        public virtual void Set(TValue1 value1, TValue2 value2)
        {
            bool isChanged = false;
            if (!value1.Equals(_value1))
            {
                _value1 = value1;
                isChanged = true;
            }

            if (!value2.Equals(_value2))
            {
                _value2 = value2;
                isChanged = true;
            }

            if (isChanged)
            {
                OnChange?.Invoke(_value1, _value2);
            }
        }
    
        public virtual void Bind(IBindable<TValue1, TValue2>.OnChanged change)
        {
            OnChange += change;
            change?.Invoke(_value1, _value2);
        }
    }
}