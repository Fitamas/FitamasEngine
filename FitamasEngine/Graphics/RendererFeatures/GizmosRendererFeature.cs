using Fitamas.DebugTools;
using Fitamas.ECS;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Graphics.RendererFeatures
{
    public class GizmosRendererFeature : RendererFeature
    {
        private DrawGizmosExecutor executor;

        public GizmosRendererFeature(GraphicsDevice graphicsDevice, DrawGizmosExecutor executor) : base(graphicsDevice, true)
        {
            this.executor = executor;
        }

        public override void Draw(RenderContext context, RenderingData renderingData)
        {
            GraphicsDevice.SetRenderTarget(RenderTexture);
            GraphicsDevice.Clear(RenderTargetClearColor);

            Gizmos.Begin();
            executor.DrawGizmos();
            Gizmos.End();

            GraphicsDevice.SetRenderTarget(null);
        }
    }
}
