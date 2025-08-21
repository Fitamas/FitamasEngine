using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Graphics.RendererFeatures
{
    public abstract class RendererFeature
    {
        private bool renderAfterPostProcessors;

        public Material Material = Material.DefaultMaterial;
        public int RenderOrder = 0;
        public RenderTarget2D RenderTexture;
        public Color RenderTargetClearColor = Color.Transparent;

        public bool RenderAfterPostProcessors => renderAfterPostProcessors;

        public GraphicsDevice GraphicsDevice { get; }

        public RendererFeature(GraphicsDevice graphicsDevice, bool renderAfterPostProcessors = false)
        {
            GraphicsDevice = graphicsDevice;
            this.renderAfterPostProcessors = renderAfterPostProcessors;
        }

        public virtual void Create()
        {

        }

        public virtual void OnCameraSetup(ref RenderingData renderingData)
        {
            Point screen = renderingData.ViewportAdapter.Viewport.Bounds.Size;

            RenderTexture = new RenderTarget2D(GraphicsDevice, screen.X, screen.Y);
        }

        public abstract void Draw(RenderContext context, RenderingData renderingData);
    }
}
