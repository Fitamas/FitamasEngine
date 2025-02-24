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

namespace Fitamas.Input.InputListeners
{
    public class MouseEventArgs : EventArgs
    {
        public MouseEventArgs(ViewportAdapter viewportAdapter, TimeSpan time, MouseState previousState,
            MouseState currentState, MouseButton button = MouseButton.None)
        {
            PreviousState = previousState;
            CurrentState = currentState;
            Position = viewportAdapter?.PointToScreen(currentState.X, currentState.Y)
                       ?? new Point(currentState.X, currentState.Y);
            Button = button;
            ScrollWheelValue = currentState.ScrollWheelValue;
            ScrollWheelDelta = currentState.ScrollWheelValue - previousState.ScrollWheelValue;
            Time = time;
        }

        public TimeSpan Time { get; }

        public MouseState PreviousState { get; }
        public MouseState CurrentState { get; }
        public Point Position { get; }
        public MouseButton Button { get; }
        public int ScrollWheelValue { get; }
        public int ScrollWheelDelta { get; }

        public Vector2 DistanceMoved => CurrentState.Position.ToVector2() - PreviousState.Position.ToVector2();
    }
}