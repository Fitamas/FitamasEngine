using Fitamas.Events;
using Fitamas.Input;
using Fitamas.Input.InputListeners;
using Fitamas.UserInterface.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Fitamas.UserInterface.Input
{
    public class GUIKeyboard
    {
        public static readonly RoutedEvent KeyDownEvent = new RoutedEvent();

        public static readonly RoutedEvent KeyEvent = new RoutedEvent();

        public static readonly RoutedEvent KeyUpEvent = new RoutedEvent();

        private KeyboardListener listener;

        public GUIComponent Focused { get; set; }

        public GUIKeyboard(KeyboardListener listener)
        {
            this.listener = listener;

            listener.KeyTyped += (s, a) =>
            {
                InvokeEvent(new GUIKeyboardEventArgs(a, KeyDownEvent, Focused));
            };
            listener.KeyPressed += (s, a) =>
            {
                InvokeEvent(new GUIKeyboardEventArgs(a, KeyEvent, Focused));
            };
            listener.KeyReleased += (s, a) =>
            {
                InvokeEvent(new GUIKeyboardEventArgs(a, KeyUpEvent, Focused));
            };
        }

        private void InvokeEvent(GUIKeyboardEventArgs args)
        {
            Focused?.RaiseEvent(args);
        }

        static GUIKeyboard()
        {
            GUIEventManager.Register(KeyDownEvent, new MonoEvent<GUIComponent, GUIKeyboardEventArgs>(OnKeyDown));
            GUIEventManager.Register(KeyEvent, new MonoEvent<GUIComponent, GUIKeyboardEventArgs>(OnKey));
            GUIEventManager.Register(KeyUpEvent, new MonoEvent<GUIComponent, GUIKeyboardEventArgs>(OnKeyUp));
        }

        private static void OnKeyDown(GUIComponent sender, GUIKeyboardEventArgs args)
        {
            if (sender is IKeyboardEvent keyboardEvent)
            {
                keyboardEvent.OnKeyDown(args);
            }
        }

        private static void OnKey(GUIComponent sender, GUIKeyboardEventArgs args)
        {
            if (sender is IKeyboardEvent keyboardEvent)
            {
                keyboardEvent.OnKey(args);
            }
        }

        private static void OnKeyUp(GUIComponent sender, GUIKeyboardEventArgs args)
        {
            if (sender is IKeyboardEvent keyboardEvent)
            {
                keyboardEvent.OnKeyUP(args);
            }
        }
    }
}
