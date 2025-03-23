using System;

namespace WDL.DigitalLogic
{
    public interface ISignal
    {
        public bool IsHigh { get; }
        public bool IsLow { get; }
    }

    public struct Signal : ISignal
    {
        public const int MaxSignal = 16;

        private int value = 0;

        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = Math.Clamp(value, 0, MaxSignal);
            }
        }

        public bool IsHigh => Value > 0;

        public bool IsLow => !IsHigh;

        public Signal(int value)
        {
            Value = value;
        }

        public Signal(bool isMax)
        {
            Value = isMax ? MaxSignal : 0;
        }
    }
}
