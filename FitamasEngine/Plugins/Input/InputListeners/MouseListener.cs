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

namespace MonoGame.Extended.Input.InputListeners
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
        private MouseState _currentState;
        private bool _dragging;
        private GameTime _gameTime;
        private bool _hasDoubleClicked;
        private MouseEventArgs _mouseDownArgs;
        private MouseEventArgs _previousClickArgs;
        private MouseState _previousState;

        public MouseListener()
            : this(new MouseListenerSettings())
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

        public ViewportAdapter ViewportAdapter { get; set; }

        public int DoubleClickMilliseconds { get; }
        public int DragThreshold { get; }

        /// <summary>
        ///     Returns true if the mouse has moved between the current and previous frames.
        /// </summary>
        /// <value><c>true</c> if the mouse has moved; otherwise, <c>false</c>.</value>
        public bool HasMouseMoved => (_previousState.X != _currentState.X) || (_previousState.Y != _currentState.Y);

        public Point MousePosition
        {
            get
            {
                Point position = _currentState.Position;

                if (ViewportAdapter != null)
                {
                    return ViewportAdapter.PointToScreen(position);
                }

                return position;
            }
        }

        public event EventHandler<MouseEventArgs> MouseDown;
        public event EventHandler<MouseEventArgs> MouseUp;
        public event EventHandler<MouseEventArgs> MouseClicked;
        public event EventHandler<MouseEventArgs> MouseDoubleClicked;
        public event EventHandler<MouseEventArgs> MouseMoved;
        public event EventHandler<MouseEventArgs> MouseWheelMoved;
        public event EventHandler<MouseEventArgs> MouseDragStart;
        public event EventHandler<MouseEventArgs> MouseDrag;
        public event EventHandler<MouseEventArgs> MouseDragEnd;

        private void CheckButtonPressed(Func<MouseState, ButtonState> getButtonState, MouseButton button)
        {
            if ((getButtonState(_currentState) == ButtonState.Pressed) &&
                (getButtonState(_previousState) == ButtonState.Released))
            {
                var args = new MouseEventArgs(ViewportAdapter, _gameTime.TotalGameTime, _previousState, _currentState, button);

                MouseDown?.Invoke(this, args);
                _mouseDownArgs = args;

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
            if ((getButtonState(_currentState) == ButtonState.Released) &&
                (getButtonState(_previousState) == ButtonState.Pressed))
            {
                var args = new MouseEventArgs(ViewportAdapter, _gameTime.TotalGameTime, _previousState, _currentState, button);

                if (_mouseDownArgs.Button == args.Button)
                {
                    var clickMovement = DistanceBetween(args.Position, _mouseDownArgs.Position);

                    // If the mouse hasn't moved much between mouse down and mouse up
                    if (clickMovement < DragThreshold)
                    {
                        if (!_hasDoubleClicked)
                            MouseClicked?.Invoke(this, args);
                    }
                    else // If the mouse has moved between mouse down and mouse up
                    {
                        MouseDragEnd?.Invoke(this, args);
                        _dragging = false;
                    }
                }

                MouseUp?.Invoke(this, args);

                _hasDoubleClicked = false;
                _previousClickArgs = args;
            }
        }

        private void CheckMouseDragged(Func<MouseState, ButtonState> getButtonState, MouseButton button)
        {
            if ((getButtonState(_currentState) == ButtonState.Pressed) &&
                (getButtonState(_previousState) == ButtonState.Pressed))
            {
                var args = new MouseEventArgs(ViewportAdapter, _gameTime.TotalGameTime, _previousState, _currentState, button);

                if (_mouseDownArgs.Button == args.Button)
                {
                    if (_dragging)
                        MouseDrag?.Invoke(this, args);
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

        public override void Update(GameTime gameTime)
        {
            _gameTime = gameTime;
            _currentState = Mouse.GetState();

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

            // Check for any sort of mouse movement.
            if (HasMouseMoved)
            {
                MouseMoved?.Invoke(this,
                    new MouseEventArgs(ViewportAdapter, gameTime.TotalGameTime, _previousState, _currentState));

                CheckMouseDragged(s => s.LeftButton, MouseButton.Left);
                CheckMouseDragged(s => s.MiddleButton, MouseButton.Middle);
                CheckMouseDragged(s => s.RightButton, MouseButton.Right);
                CheckMouseDragged(s => s.XButton1, MouseButton.XButton1);
                CheckMouseDragged(s => s.XButton2, MouseButton.XButton2);
            }

            // Handle mouse wheel events.
            if (_previousState.ScrollWheelValue != _currentState.ScrollWheelValue)
            {
                MouseWheelMoved?.Invoke(this,
                    new MouseEventArgs(ViewportAdapter, gameTime.TotalGameTime, _previousState, _currentState));
            }

            _previousState = _currentState;
        }

        private static int DistanceBetween(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }
    }
}