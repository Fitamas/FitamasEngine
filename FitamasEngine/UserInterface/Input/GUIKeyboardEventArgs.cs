using Fitamas.Input.InputListeners;
using Microsoft.Xna.Framework.Input;
using System;

namespace Fitamas.UserInterface.Input
{
    public class GUIKeyboardEventArgs : GUIEventArgs
    {
        private KeyboardEventArgs args;

        public KeyboardModifiers Modifiers => args.Modifiers;
        public Keys Key => args.Key;
        public char? Character => args.Character;

        public GUIKeyboardEventArgs(KeyboardEventArgs args, RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {
            this.args = args;
        }
    }
}
