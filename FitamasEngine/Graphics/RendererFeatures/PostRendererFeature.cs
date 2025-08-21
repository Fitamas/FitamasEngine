using Fitamas.ECS;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Graphics.RendererFeatures
{
    public class PostRendererFeature : RendererFeature
    {
        private DrawExecutor executor;

        public PostRendererFeature(GraphicsDevice graphicsDevice, DrawExecutor executor) : base(graphicsDevice, true)
        {
            this.executor = executor;
        }

        public override void Draw(RenderContext context, RenderingData renderingData)
        {
            GraphicsDevice.SetRenderTarget(RenderTexture);
            GraphicsDevice.Clear(RenderTargetClearColor);

            executor.Draw(context.GameTime);

            GraphicsDevice.SetRenderTarget(null);
        }
    }
}
