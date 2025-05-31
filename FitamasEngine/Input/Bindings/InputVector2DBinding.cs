using Fitamas.Input.InputListeners;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Input.Bindings
{
    public class InputVector2DBinding : InputCompositeBinding<Vector2>
    {
        public static readonly string PositiveX = nameof(PositiveX);
        public static readonly string NegativeX = nameof(NegativeX);
        public static readonly string PositiveY = nameof(PositiveY);
        public static readonly string NegativeY = nameof(NegativeY);

        public InputVector2DBinding()
        {

        }

        protected override Vector2 ReadValue(InputListener listener)
        {
            return new Vector2(
                (bindingMap[PositiveX].Value ? 1 : 0) + (bindingMap[NegativeX].Value ? -1 : 0),
                (bindingMap[PositiveY].Value ? 1 : 0) + (bindingMap[NegativeY].Value ? -1 : 0));
        }
    }
}
