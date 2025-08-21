using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Graphics.RendererFeatures
{
    public class WorldRendererFeature : RendererFeature
    {
        public WorldRendererFeature(GraphicsDevice graphicsDevice, bool renderAfterPostProcessors = false) : base(graphicsDevice, renderAfterPostProcessors)
        {

        }

        public override void Draw(RenderContext context, RenderingData renderingData)
        {
            context.DrawRenderers(context, RenderTexture, RenderTargetClearColor);
        }
    }
}
