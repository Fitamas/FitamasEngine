using System;
using System.Collections.Generic;
using Fitamas.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fitamas.Graphics
{
    public abstract class EntityDrawSystem : EntitySystem, IDrawSystem
    {
        protected EntityDrawSystem(AspectBuilder aspect) : base(aspect)
        {

        }

        public abstract void Draw(GameTime gameTime);
    }

    //public class RenderingData
    //{
    //    /// <summary>The stage that possess every object in your view</summary>
    //    //public Stage stage;
    //    /// <summary>Callback to update the Camera position. Only done in First phase.</summary>
    //    //public ICameraUpdater updater;
    //    /// <summary>Viewport size</summary>
    //    public Rectangle ViewPort;
    //    /// <summary>Render texture handling captured image</summary>
    //    public RenderTarget2D Output;
    //}

    public class RenderTargetCache
    {
        private GraphicsDevice graphicsDevice;
        private RenderTarget2D output;

        public RenderTarget2D Output => output;

        public RenderTargetCache(GraphicsDevice graphicsDevice, int width, int height)
        {
            this.graphicsDevice = graphicsDevice;

            UpdateSize(width, height);
        }

        public void UpdateSize(int width, int height)
        {
            PresentationParameters parameters = graphicsDevice.PresentationParameters;
            output = new RenderTarget2D(graphicsDevice, width, height, false, parameters.BackBufferFormat, parameters.DepthStencilFormat);
        }
    }

    public class Renderer
    {
        private GraphicsDevice graphicsDevice;
        private RenderTargetCache cache;

        public RenderTargetCache Cache => cache;

        public Renderer(GraphicsDevice graphicsDevice, int width, int height)
        {
            this.graphicsDevice = graphicsDevice;

            cache = new RenderTargetCache(graphicsDevice, width, height);
        }

        public void UpdateSize(int width, int height)
        {
            cache.UpdateSize(width, height);
        }

        public void Begin()
        {
            graphicsDevice.SetRenderTarget(cache.Output);
            graphicsDevice.Clear(Color.Transparent);
        }

        public void End()
        {
            graphicsDevice.SetRenderTarget(null);
        }
    }

    public class RenderPipeline
    {
        private GraphicsDevice graphicsDevice;
        private GameWindow window;
        private List<Renderer> renderers;
        private RenderTargetCache cache;
        private SpriteBatch spriteBatch;

        public IReadOnlyList<Renderer> Renderers => renderers;

        public RenderPipeline(GraphicsDevice graphicsDevice, GameWindow window)
        {
            this.graphicsDevice = graphicsDevice;
            this.window = window;

            renderers = new List<Renderer>();
            spriteBatch = new SpriteBatch(graphicsDevice);

            cache = new RenderTargetCache(graphicsDevice, window.ClientBounds.Width, window.ClientBounds.Height);

            window.ClientSizeChanged += OnClientSizeChanged;
        }

        public Renderer Create()
        {
            Renderer renderer = new Renderer(graphicsDevice, window.ClientBounds.Width, window.ClientBounds.Height);
            renderers.Add(renderer);

            return renderer;
        }

        public void Render()
        {
            //graphicsDevice.Clear(Camera.Current.Color);

            //graphicsDevice.SetRenderTarget(cache.Output);
            //graphicsDevice.Clear(Camera.Current.Color);
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
            ////spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointWrap,
            ////      DepthStencilState.Default, RasterizerState.CullNone, null, Camera.Current.TranslationVirtualMatrix);
            ////foreach (Renderer renderer in renderers)
            ////{
            ////    graphicsDevice.Clear(Camera.Current.Color);
            ////    spriteBatch.Draw(renderer.Cache.Output, graphicsDevice.Viewport.Bounds, Color.White);
            ////}

            //graphicsDevice.Clear(Camera.Current.Color);
            spriteBatch.Draw(renderers[0].Cache.Output, graphicsDevice.Viewport.Bounds, Color.White);
            spriteBatch.End();
            //graphicsDevice.SetRenderTarget(null);

            //graphicsDevice.Clear(Camera.Current.Color);
            //spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointWrap,
            //      DepthStencilState.Default, RasterizerState.CullNone, null, Camera.Current.TranslationVirtualMatrix);
            //spriteBatch.Begin();
            //spriteBatch.Draw(cache.Output, graphicsDevice.Viewport.Bounds, Color.White);
            //spriteBatch.End();
        }

        private void OnClientSizeChanged(object sender, EventArgs eventArgs)
        {
            int x = window.ClientBounds.Width;
            int y = window.ClientBounds.Height;

            cache.UpdateSize(x, y);

            foreach (Renderer renderer in renderers)
            {
                renderer.UpdateSize(x, y);
            }
        }
    }
}
