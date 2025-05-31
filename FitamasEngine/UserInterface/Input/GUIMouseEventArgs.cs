using Fitamas.Input.InputListeners;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Fitamas.UserInterface.Input
{
    public class GUIMouseEventArgs : GUIEventArgs
    {
        private MouseButtonEventArgs args;

        public Point Position => args.Position;
        public Point Delta => args.Delta;
        public MouseButton Button => args.Button;
        public int ScrollWheelValue => args.WheelValue;
        public int ScrollWheelDelta => args.WheelDelta;

        public GUIMouseEventArgs(MouseButtonEventArgs args, RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {
            this.args = args;
        }
    }

    public class GUIMouseWheelEventArgs : GUIEventArgs
    {
        private MouseWheelEventArgs args;

        public int ScrollWheelValue => args.WheelValue;
        public int ScrollWheelDelta => args.WheelDelta;

        public GUIMouseWheelEventArgs(MouseWheelEventArgs args, RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {
            this.args = args;
        }
    }

    public class GUIMousePositionEventArgs : GUIEventArgs
    {
        private MousePositionEventArgs args;

        public Point Position => args.Position;
        public Point Delta => args.Delta;

        public GUIMousePositionEventArgs(MousePositionEventArgs args, RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {
            this.args = args;
        }
    }
}
