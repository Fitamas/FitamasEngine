using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Graphics
{
    public struct RenderContext
    {
        private RenderManager renderManager;

        public GameTime GameTime;

        public RenderContext(RenderManager renderManager)
        {
            this.renderManager = renderManager;
        }

        public void Blit(Texture2D source, RenderTarget2D destination)
        {
            renderManager.Blit(source, destination);
        }

        public void Blit(Texture2D source, RenderTarget2D destination, Material material)
        {
            renderManager.Blit(source, destination, material);
        }

        public void DrawTexture(Texture2D source)
        {
            renderManager.DrawTexture(source);
        }

        public void DrawTexture(Texture2D source, Material material)
        {
            renderManager.DrawTexture(source, material);
        }

        public void DrawRenderers(RenderContext context, RenderTarget2D renderTarget, Color color)
        {
            renderManager.DrawRenderers(context, renderTarget, color);
        }

        public void DrawRenderers(RenderContext context, RenderTarget2D renderTarget, Color color, string tag)
        {
            renderManager.DrawRenderers(context, renderTarget, color, tag);
        }
    }
}
