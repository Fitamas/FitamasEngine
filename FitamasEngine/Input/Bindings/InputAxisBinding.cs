using Fitamas.Input.InputListeners;
using System;

namespace Fitamas.Input.Bindings
{
    public class InputAxisBinding : InputCompositeBinding<int>
    {
        public static readonly string Positive = nameof(Positive);
        public static readonly string Negative = nameof(Negative);

        public InputAxisBinding()
        {

        }

        protected override int ReadValue(InputListener listener)
        {
            return (bindingMap[Positive].Value ? 1 : 0) + (bindingMap[Negative].Value ? -1 : 0);
        }
    }
}
