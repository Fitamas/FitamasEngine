using Fitamas.Graphics.ViewportAdapters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Fitamas.Graphics
{
    public struct RenderingData
    {
        public Camera Camera;
        public ViewportAdapter ViewportAdapter;
        public RenderTarget2D Source;
        public RenderTarget2D Destination;

        public bool NeedCameraSetup;
        public bool PostProcessingEnabled;
        public Rectangle Viewport;

        public RenderingData()
        {
            NeedCameraSetup = false;
            PostProcessingEnabled = true;
            Viewport = new Rectangle(Point.Zero, new Point(1, 1));
        }
    }
}
