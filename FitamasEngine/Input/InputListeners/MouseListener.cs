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
using System.Collections.Generic;
using Fitamas.Input.Actions;

namespace Fitamas.Input.InputListeners
{
    /// <summary>
    ///     Handles mouse input.
    /// </summary>
    /// <remarks>
    ///     Due to nature of the listener, even when game is not in focus, listener will continue to be updated.
    ///     To avoid that, manual pause of Update() method is required whenever game loses focus.
    ///     To avoid having to do it manually, register listener to <see cref="InputListenerComponent" />
    /// </remarks>
    public class MouseListener : InputListener
    {
        private bool _dragging;
        private GameTime _gameTime;
        private bool _hasDoubleClicked;
        private MouseButtonEventArgs _mouseDownArgs;
        private MouseButtonEventArgs _previousClickArgs;
        private MouseState currentState;
        private MouseState previousState;

        public event EventHandler<MouseButtonEventArgs> MouseDown;
        public event EventHandler<MouseButtonEventArgs> MouseUp;
        public event EventHandler<MouseButtonEventArgs> MouseClicked;
        public event EventHandler<MouseButtonEventArgs> MouseDoubleClicked;
        public event EventHandler<MousePositionEventArgs> MouseMoved;
        public event EventHandler<MouseWheelEventArgs> MouseWheelMoved;
        public event EventHandler<MouseButtonEventArgs> MouseDragStart;
        public event EventHandler<MouseButtonEventArgs> MouseDrag;
        public event EventHandler<MouseButtonEventArgs> MouseDragEnd;

        public ViewportAdapter ViewportAdapter { get; set; }
        public int DoubleClickMilliseconds { get; }
        public int DragThreshold { get; }

        public override string Name => "Mouse";

        public bool HasMouseMoved { get; private set; }
        public bool HasMouseWheelMoved { get; private set; }

        public Point Position
        {
            get
            {
                Point position = currentState.Position;

                if (ViewportAdapter != null)
                {
                    return ViewportAdapter.PointToScreen(position);
                }

                return position;
            }
        }

        public Point Delta
        {
            get
            {
                return currentState.Position - previousState.Position;
            }
        }

        public int WheelDelta
        {
            get
            {
                return currentState.ScrollWheelValue - previousState.ScrollWheelValue;
            }
        }

        public MouseListener() : this(new MouseListenerSettings())
        {

        }

        public MouseListener(ViewportAdapter viewportAdapter) : this(new MouseListenerSettings())
        {
            ViewportAdapter = viewportAdapter;
        }

        public MouseListener(MouseListenerSettings settings)
        {
            ViewportAdapter = settings.ViewportAdapter;
            DoubleClickMilliseconds = settings.DoubleClickMilliseconds;
            DragThreshold = settings.DragThreshold;
        }

        private void CheckButtonPressed(Func<MouseState, ButtonState> getButtonState, MouseButton button)
        {
            if (getButtonState(currentState) == ButtonState.Pressed &&
                getButtonState(previousState) == ButtonState.Released)
            {
                var args = new MouseButtonEventArgs(ViewportAdapter, _gameTime.TotalGameTime, previousState, currentState, button, ButtonState.Pressed);

                MouseDown?.Invoke(this, args);
                _mouseDownArgs = args;

                AddEvent(button.ToString(), true);

                if (_previousClickArgs != null)
                {
                    // If the last click was recent
                    var clickMilliseconds = (args.Time - _previousClickArgs.Time).TotalMilliseconds;

                    if (clickMilliseconds <= DoubleClickMilliseconds)
                    {
                        MouseDoubleClicked?.Invoke(this, args);
                        _hasDoubleClicked = true;
                    }

                    _previousClickArgs = null;
                }
            }
        }

        private void CheckButtonReleased(Func<MouseState, ButtonState> getButtonState, MouseButton button)
        {
            if (getButtonState(currentState) == ButtonState.Released &&
                getButtonState(previousState) == ButtonState.Pressed)
            {
                var args = new MouseButtonEventArgs(ViewportAdapter, _gameTime.TotalGameTime, previousState, currentState, button, ButtonState.Released);

                if (_mouseDownArgs.Button == args.Button)
                {
                    var clickMovement = DistanceBetween(args.Position, _mouseDownArgs.Position);

                    // If the mouse hasn't moved much between mouse down and mouse up
                    if (clickMovement < DragThreshold)
                    {
                        if (!_hasDoubleClicked)
                        {
                            MouseClicked?.Invoke(this, args);
                        }
                    }
                    else // If the mouse has moved between mouse down and mouse up
                    {
                        MouseDragEnd?.Invoke(this, args);
                        _dragging = false;
                    }
                }

                MouseUp?.Invoke(this, args);
                AddEvent(button.ToString(), false);

                _hasDoubleClicked = false;
                _previousClickArgs = args;
            }
        }

        private void CheckMouseDragged(Func<MouseState, ButtonState> getButtonState, MouseButton button)
        {
            if (getButtonState(currentState) == ButtonState.Pressed &&
                getButtonState(previousState) == ButtonState.Pressed)
            {
                var args = new MouseButtonEventArgs(ViewportAdapter, _gameTime.TotalGameTime, previousState, currentState, button, ButtonState.Pressed);

                if (_mouseDownArgs.Button == args.Button)
                {
                    if (_dragging)
                    {
                        MouseDrag?.Invoke(this, args);
                    }
                    else
                    {
                        // Only start to drag based on DragThreshold
                        var clickMovement = DistanceBetween(args.Position, _mouseDownArgs.Position);

                        if (clickMovement > DragThreshold)
                        {
                            _dragging = true;
                            MouseDragStart?.Invoke(this, args);
                        }
                    }
                }
            }
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            _gameTime = gameTime;
            currentState = Mouse.GetState();

            CheckButtonPressed(s => s.LeftButton, MouseButton.Left);
            CheckButtonPressed(s => s.MiddleButton, MouseButton.Middle);
            CheckButtonPressed(s => s.RightButton, MouseButton.Right);
            CheckButtonPressed(s => s.XButton1, MouseButton.XButton1);
            CheckButtonPressed(s => s.XButton2, MouseButton.XButton2);

            CheckButtonReleased(s => s.LeftButton, MouseButton.Left);
            CheckButtonReleased(s => s.MiddleButton, MouseButton.Middle);
            CheckButtonReleased(s => s.RightButton, MouseButton.Right);
            CheckButtonReleased(s => s.XButton1, MouseButton.XButton1);
            CheckButtonReleased(s => s.XButton2, MouseButton.XButton2);

            HasMouseMoved = (currentState.Position - previousState.Position) != Point.Zero;
            MousePositionEventArgs args0 = new MousePositionEventArgs(Position, Delta);
            if (HasMouseMoved)
            {
                MouseMoved?.Invoke(this, args0);   

                CheckMouseDragged(s => s.LeftButton, MouseButton.Left);
                CheckMouseDragged(s => s.MiddleButton, MouseButton.Middle);
                CheckMouseDragged(s => s.RightButton, MouseButton.Right);
                CheckMouseDragged(s => s.XButton1, MouseButton.XButton1);
                CheckMouseDragged(s => s.XButton2, MouseButton.XButton2);
            }
            AddEvent("Delta", Delta);
            AddEvent("Position", Position);

            HasMouseWheelMoved = (currentState.ScrollWheelValue - previousState.ScrollWheelValue) != 0;
            MouseWheelEventArgs args1 = new MouseWheelEventArgs(currentState.ScrollWheelValue, WheelDelta);
            if (HasMouseWheelMoved)
            {
                MouseWheelMoved?.Invoke(this, args1);
            }
            AddEvent("WheelDelta", WheelDelta);

            previousState = currentState;
        }

        private static int DistanceBetween(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }
    }
}