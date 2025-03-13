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
using Microsoft.Xna.Framework.Graphics;

namespace Fitamas.Graphics.ViewportAdapters
{
    public class WindowViewportAdapter : ViewportAdapter
    {
        protected readonly GameWindow Window;

        public WindowViewportAdapter(GameWindow window, GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            Window = window;
            window.ClientSizeChanged += OnClientSizeChanged;
        }

        public override int ViewportWidth => Window.ClientBounds.Width;
        public override int ViewportHeight => Window.ClientBounds.Height;
        public override int VirtualWidth => Window.ClientBounds.Width;
        public override int VirtualHeight => Window.ClientBounds.Height;

        public override Matrix GetScaleMatrix()
        {
            return Matrix.Identity;
        }

        public override Point PointToScreen(int x, int y)
        {
            var scaleMatrix = GetScaleMatrix();
            var invertedMatrix = Matrix.Invert(scaleMatrix);
            return Vector2.Transform(new Vector2(x, y), invertedMatrix).ToPoint();
        }

        private void OnClientSizeChanged(object sender, EventArgs eventArgs)
        {
            var x = Window.ClientBounds.Width;
            var y = Window.ClientBounds.Height;

            GraphicsDevice.Viewport = new Viewport(0, 0, x, y);
        }
    }
}