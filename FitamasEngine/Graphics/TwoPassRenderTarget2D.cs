using Microsoft.Xna.Framework.Graphics;
using System;

namespace Fitamas.Graphics
{
    public class TwoPassRenderTarget2D
    {
        private GraphicsDevice graphicsDevice;
        private RenderTarget2D rt1;
        private RenderTarget2D rt2;
        private SurfaceFormat surfaceFormat;
        private bool mipMap;
        private bool rt1IsFinal;

        public TwoPassRenderTarget2D(GraphicsDevice graphicsDevice, int width, int height, bool mipMap, SurfaceFormat surfaceFormat)
        {
            this.graphicsDevice = graphicsDevice;
            this.mipMap = mipMap;
            this.surfaceFormat = surfaceFormat;
            UpdateSize(width, height);
        }

        private void UpdateSize(int width, int height)
        {
            rt1 = new RenderTarget2D(graphicsDevice, width, height, mipMap, surfaceFormat, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);

            rt2 = new RenderTarget2D(graphicsDevice, width, height, mipMap, surfaceFormat, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
        }

        public void ApplyEffect(RenderContext context, Material material, int passes)
        {
            for (int i = 0; i < passes; i++)
            {
                ApplyEffect(context, material);
            }
        }

        public void ApplyEffect(RenderContext context, Material material)
        {
            var source = rt1IsFinal ? rt1 : rt2;
            var target = rt1IsFinal ? rt2 : rt1;

            context.Blit(source, target, material);

            rt1IsFinal = !rt1IsFinal;
        }

        public RenderTarget2D GetResult()
        {
            return rt1IsFinal ? rt1 : rt2;
        }

        public void Dispose()
        {
            rt1?.Dispose();
            rt2?.Dispose();
        }
    }
}
