using System;

namespace Fitamas.Input.Processors
{
    public abstract class InputProcessor
    {
        public abstract object ProcessObject(object value); 
    }

    public abstract class InputProcessor<T> : InputProcessor
    {
        public override object ProcessObject(object value)
        {
            if (value.GetType() == typeof(T))
            {
                return Process((T)value);
            }

            throw new ArgumentException($"Different types {value.GetType()} and {typeof(T)}", nameof(value));
        }

        public abstract T Process(T value);
    }
}
