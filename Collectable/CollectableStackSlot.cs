using System;
using System.Collections.Generic;

namespace SpookyBotanyGame.Collectable
{
    public class CollectableStackSlot<TStacked>
    {
        public const int DEFAULT_MAX_VALUE = 256;

        public delegate void OnEmpty();

        public delegate void OnFull();

        public delegate void OnChange(int newValue, int oldValue);

        public delegate void OnOverflow(IEnumerable<CollectableStackSlot<TStacked>> overflow);

        public event OnEmpty OnEmptied;
        public event OnFull OnFilled;
        public event OnChange OnChanged;
        public event OnChange OnMaxChanged;

        /// <summary>
        /// If you assign an overflow function it is invoked whenever a stack slot can't handle the change that is
        /// requested. It sends out a new stack with the remained after the combination.
        /// </summary>
        public event OnOverflow OnOverflowed;

        private int _amount = 0;
        private int _max = DEFAULT_MAX_VALUE;
        private bool _isOverflowing;

        /// <summary>
        /// If you don't assign an overflow function when the number of items in the slot is more than it's max this
        /// will be true.
        /// </summary>
        public bool IsOverflowing => _isOverflowing;

        public int Space => _max - _amount;

        public int Max => _max;

        public CollectableStackSlot()
        {
            _amount = 0;
            _max = DEFAULT_MAX_VALUE;
        }

        public CollectableStackSlot(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount),
                    $"{nameof(CollectableStackSlot<TStacked>)}() ctor {nameof(amount)} of {amount} cannot be less than 0");
            }

            SetAmount(amount);
        }

        public CollectableStackSlot(int amount, int max)
        {
            if (max < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(max),
                    $"{nameof(SetAmount)}() set {nameof(max)} of {max} cannot be less than 0");
            }

            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount),
                    $"{nameof(CollectableStackSlot<TStacked>)}() ctor {nameof(amount)} of {amount} cannot be less than 0");
            }

            if (amount > max)
            {
                throw new ArgumentOutOfRangeException(nameof(amount),
                    $"{nameof(CollectableStackSlot<TStacked>)}() ctor {nameof(amount)} of {amount} cannot be greater than {nameof(max)} of {max}");
            }

            SetAmount(amount);
            SetMax(max);
        }

        public CollectableStackSlot(int amount, int max, OnOverflow onOverflow)
        {
            if (max < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(max),
                    $"{nameof(SetAmount)}() set {nameof(max)} of {max} cannot be less than 0");
            }

            if (max == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(max),
                    $"{nameof(SetAmount)}() set {nameof(max)} of {max} cannot be less than 0");
            }

            OnOverflowed += onOverflow;
            SetAmount(amount);
            SetMax(max);
        }

        public void SetMax(int value)
        {
            int oldValue = _max;
            _max = value;
            if (_amount > _max)
            {
                Overflow(_amount - _max);
            }

            OnMaxChanged?.Invoke(_max,oldValue);
        }

        /// <summary>
        /// Whenever the stack size is changed more than the current stack can handle, generate an overflow stack with
        /// whatever the remainder is and clamp the stack back down.
        /// </summary>
        /// <param name="amount"></param>
        protected void Overflow(int amount)
        {
            if (OnOverflowed != null)
            {
                List<CollectableStackSlot<TStacked>> allOverflowStacks = new List<CollectableStackSlot<TStacked>>();
                for (int i = _max; i < amount; i += _max)
                {
                    //deal with the final stack somehow
                    CollectableStackSlot<TStacked> overflowStack = new CollectableStackSlot<TStacked>(_max);
                    allOverflowStacks.Add(overflowStack);
                }

                OnOverflowed.Invoke(allOverflowStacks);
                _amount -= amount;
            }
        }


        public int Amount
        {
            get => _amount;
            set => SetAmount(value);
        }

        private void SetAmount(int value)
        {
            if (_amount == value)
            {
                return;
            }

            if (_max == 0)
            {
                return;
            }

            if (value > _max)
            {
                Overflow(value - _max);
            }

            int oldCount = _amount;
            _amount = value;
            OnChanged?.Invoke(0, oldCount);
            if (_amount == 0)
            {
                OnEmptied?.Invoke();
            }

            if (_amount == _max)
            {
                OnFilled?.Invoke();
            }

            if (_amount > _max)
            {
                if (!_isOverflowing)
                {
                    _isOverflowing = true;
                    OnFilled?.Invoke();
                }
            }

            if (_amount <= _max)
            {
                _isOverflowing = false;
            }
        }

        public void Add(int value)
        {
            SetAmount(_amount + value);
        }

        public void Add(TStacked item)
        {
            SetAmount(_amount + 1);
        }

        public void Add(CollectableStackSlot<TStacked> stackSlot)
        {
            SetAmount(stackSlot._amount);
            stackSlot.Empty();
        }

        public CollectableStackSlot<TStacked> Remove(int count)
        {
            SetAmount(_amount - count);
            return new CollectableStackSlot<TStacked>(count);
        }

        public void Empty()
        {
            if (_amount != 0)
            {
                int oldCount = _amount;
                _amount = 0;
                _isOverflowing = false;
                OnChanged?.Invoke(0, oldCount);
                OnEmptied?.Invoke();
            }
        }

        public void Fill()
        {
            if (_amount < _max)
            {
                int oldCount = _amount;
                _amount = _max;
                _isOverflowing = false;
                OnChanged?.Invoke(_max, oldCount);
                OnFilled?.Invoke();
            }
        }
    }
}