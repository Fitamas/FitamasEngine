using Fitamas.Input;
using Fitamas.Input.InputListeners;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface.Input
{
    public class GUIMouseEventArgs : GUIEventArgs
    {
        private MouseEventArgs args;

        public Point Position => args.Position;
        public MouseButton Button => args.Button;
        public int ScrollWheelValue => args.ScrollWheelValue;
        public int ScrollWheelDelta => args.ScrollWheelDelta;
        public Point DistanceMoved => args.CurrentState.Position - args.PreviousState.Position;

        public GUIMouseEventArgs(MouseEventArgs args, RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {
            this.args = args;
        }
    }
}
