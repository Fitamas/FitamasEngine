/*
    The MIT License (MIT)

    Copyright (c) 2015-2024:
    - Dylan Wilson (https://github.com/dylanwilson80)
    - Lucas Girouard-Stranks (https://github.com/lithiumtoast)
    - Christopher Whitley (https://github.com/aristurtledev)

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Fitamas.Graphics.ViewportAdapters;
using Fitamas.Input.Actions;

namespace Fitamas.Input.InputListeners
{
    public class MouseButtonEventArgs : EventArgs
    {
        public TimeSpan Time { get; }
        public MouseButton Button { get; }
        public ButtonState State { get; }
        public Point Position { get; }
        public Point Delta { get; }
        public int WheelValue { get; }
        public int WheelDelta { get; }

        public MouseButtonEventArgs(ViewportAdapter viewportAdapter, TimeSpan time, MouseState previousState,
            MouseState currentState, MouseButton button, ButtonState state) : base()
        {
            Position = viewportAdapter?.PointToScreen(currentState.X, currentState.Y) ?? new Point(currentState.X, currentState.Y);
            Delta = currentState.Position - previousState.Position;
            Button = button;
            State = state;
            WheelValue = currentState.ScrollWheelValue;
            WheelDelta = currentState.ScrollWheelValue - previousState.ScrollWheelValue;
            Time = time;
        }
    }

    public class MouseWheelEventArgs : EventArgs
    {
        public int WheelValue { get; }
        public int WheelDelta { get; }

        public MouseWheelEventArgs(int value, int delta)
        {
            WheelValue = value;
            WheelDelta = delta;
        }
    }

    public class MousePositionEventArgs : EventArgs
    {
        public Point Position { get; }
        public Point Delta { get; }

        public MousePositionEventArgs(Point position, Point delta)
        {
            Position = position;
            Delta = delta;
        }
    }
}