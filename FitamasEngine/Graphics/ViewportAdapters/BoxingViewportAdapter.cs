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

// ReSharper disable once CheckNamespace

namespace MonoGame.Extended.ViewportAdapters
{
    public enum BoxingMode
    {
        None,
        Letterbox,
        Pillarbox
    }

    public class BoxingViewportAdapter : ScalingViewportAdapter
    {
        private readonly GameWindow _window;
        private readonly GraphicsDevice _graphicsDevice;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoxingViewportAdapter" />.
        /// </summary>
        public BoxingViewportAdapter(GameWindow window, GraphicsDevice graphicsDevice, int virtualWidth, int virtualHeight, int horizontalBleed = 0, int verticalBleed = 0)
            : base(graphicsDevice, virtualWidth, virtualHeight)
        {
            _window = window;
            _graphicsDevice = graphicsDevice;
            window.ClientSizeChanged += OnClientSizeChanged;
            HorizontalBleed = horizontalBleed;
            VerticalBleed = verticalBleed;
        }

        public override void Dispose()
        {
            _window.ClientSizeChanged -= OnClientSizeChanged;
            base.Dispose();
        }

        /// <summary>
        ///     Size of horizontal bleed areas (from left and right edges) which can be safely cut off
        /// </summary>
        public int HorizontalBleed { get; }

        /// <summary>
        ///     Size of vertical bleed areas (from top and bottom edges) which can be safely cut off
        /// </summary>
        public int VerticalBleed { get; }

        public BoxingMode BoxingMode { get; private set; }

        private void OnClientSizeChanged(object sender, EventArgs eventArgs)
        {
            var clientBounds = _window.ClientBounds;

            var worldScaleX = (float)clientBounds.Width / VirtualWidth;
            var worldScaleY = (float)clientBounds.Height / VirtualHeight;

            var safeScaleX = (float)clientBounds.Width / (VirtualWidth - HorizontalBleed);
            var safeScaleY = (float)clientBounds.Height / (VirtualHeight - VerticalBleed);

            var worldScale = MathHelper.Max(worldScaleX, worldScaleY);
            var safeScale = MathHelper.Min(safeScaleX, safeScaleY);
            var scale = MathHelper.Min(worldScale, safeScale);

            var width = (int)(scale * VirtualWidth + 0.5f);
            var height = (int)(scale * VirtualHeight + 0.5f);

            if (height >= clientBounds.Height && width < clientBounds.Width)
                BoxingMode = BoxingMode.Pillarbox;
            else
            {
                if (width >= clientBounds.Height && height <= clientBounds.Height)
                    BoxingMode = BoxingMode.Letterbox;
               else
                    BoxingMode = BoxingMode.None;
            }

            var x = clientBounds.Width / 2 - width / 2;
            var y = clientBounds.Height / 2 - height / 2;
            GraphicsDevice.Viewport = new Viewport(x, y, width, height);
        }

        public override void Reset()
        {
            base.Reset();
            OnClientSizeChanged(this, EventArgs.Empty);
        }

        public override Point PointToScreen(int x, int y)
        {
            var viewport = GraphicsDevice.Viewport;
            return base.PointToScreen(x - viewport.X, y - viewport.Y);
        }
    }
}