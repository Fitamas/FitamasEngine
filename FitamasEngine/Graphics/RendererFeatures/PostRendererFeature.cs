using Fitamas.Core;
using Fitamas.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Fitamas.Graphics.RendererFeatures
{
    public class PostRendererFeature : RendererFeature
    {
        private DrawExecutor executor;
        private IEnumerable<Core.IDrawable> drawables;

        public PostRendererFeature(GameEngine game, DrawExecutor executor) : base(game.GraphicsDevice, true)
        {
            this.executor = executor;
            drawables = game.MainContainer.ResolveAll<Core.IDrawable>();
        }

        public override void Draw(RenderContext context, RenderingData renderingData)
        {
            GraphicsDevice.SetRenderTarget(RenderTexture);
            GraphicsDevice.Clear(RenderTargetClearColor);

            executor.Draw(context.GameTime);
            foreach (var drawable in drawables)
            {
                drawable.Draw(context.GameTime);
            }

            GraphicsDevice.SetRenderTarget(null);
        }
    }
}
