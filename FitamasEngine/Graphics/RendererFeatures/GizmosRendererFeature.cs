using Fitamas.DebugTools;
using Fitamas.ECS;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Fitamas.Core;
using System.Collections.Generic;

namespace Fitamas.Graphics.RendererFeatures
{
    public class GizmosRendererFeature : RendererFeature
    {
        private DrawGizmosExecutor executor;
        private IEnumerable<IDrawableGizmos> drawables;

        public GizmosRendererFeature(GameEngine game, DrawGizmosExecutor executor) : base(game.GraphicsDevice, true)
        {
            this.executor = executor;
            drawables = game.MainContainer.ResolveAll<IDrawableGizmos>();
        }

        public override void Draw(RenderContext context, RenderingData renderingData)
        {
            GraphicsDevice.SetRenderTarget(RenderTexture);
            GraphicsDevice.Clear(RenderTargetClearColor);

            Gizmos.Begin();
            executor.DrawGizmos();
            foreach (var drawable in drawables)
            {
                drawable.DrawGizmos();
            }
            Gizmos.End();

            GraphicsDevice.SetRenderTarget(null);
        }
    }
}
