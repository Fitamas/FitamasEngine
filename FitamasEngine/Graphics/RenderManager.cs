using Fitamas.Collections;
using Fitamas.Core;
using Fitamas.Graphics.PostProcessors;
using Fitamas.Graphics.RendererFeatures;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fitamas.Graphics
{
    //public enum RenderQueue
    //{
    //    Opaque,
    //    Transparent,
    //    All,
    //}

    public class RenderManager : IGameComponent
    {
        private GameEngine game;
        private List<RendererFeature> rendererFeatures;
        private List<RendererFeature> rendererFeaturesAfterPostProcessors;
        private List<PostProcessor> postProcessors;
        private GraphicsDevice graphicsDevice;
        private SpriteBatch spriteBatch;
        private Camera camera;

        private RenderingData renderingData;

        public Bag<Renderer> Renderers { get; }

        public RenderManager(GameEngine game)
        {
            this.game = game;
            this.graphicsDevice = game.GraphicsDevice;

            rendererFeatures = new List<RendererFeature>();
            rendererFeaturesAfterPostProcessors = new List<RendererFeature>();
            postProcessors = new List<PostProcessor>();
            spriteBatch = new SpriteBatch(graphicsDevice);
            Renderers = new Bag<Renderer>();

            renderingData = new RenderingData();
            renderingData.PostProcessingEnabled = true;

            game.Window.ClientSizeChanged += (s, a) =>
            {
                OnCameraSetup();
            };
        }

        public void Initialize()
        {
            OnCameraSetup();
        }

        public void AddRendererFeature(RendererFeature rendererFeature)
        {
            if (rendererFeature.RenderAfterPostProcessors)
            {
                rendererFeaturesAfterPostProcessors.Add(rendererFeature);
            }
            else
            {
                rendererFeatures.Add(rendererFeature);
            }

            rendererFeature.Create();
            rendererFeature.OnCameraSetup(ref renderingData);
        }

        public void AddPostProcessor(PostProcessor postProcessor)
        {
            postProcessors.Add(postProcessor);

            postProcessor.Create();
            postProcessor.OnCameraSetup(ref renderingData);
        }

        public void Draw(GameTime gameTime)
        {
            RenderContext context = new RenderContext(this);
            context.GameTime = gameTime;

            renderingData.Camera = Camera.Current;

            foreach (RendererFeature renderer in rendererFeatures)
            {
                renderer.Draw(context, renderingData);
            }

            foreach (RendererFeature renderer in rendererFeaturesAfterPostProcessors)
            {
                renderer.Draw(context, renderingData);
            }

            graphicsDevice.SetRenderTarget(renderingData.Source);
            graphicsDevice.Clear(Camera.Current.Color);

            foreach (RendererFeature renderer in rendererFeatures)
            {
                Material material = renderer.Material;
                Effect effect = material.Effect;

                spriteBatch.Begin(SpriteSortMode.Deferred, material.BlendState, material.SamplerState, material.DepthStencilState, RasterizerState.CullNone);
                spriteBatch.Draw(renderer.RenderTexture, renderingData.Viewport, Color.White);
                spriteBatch.End();
            }

            RenderTarget2D renderTarget = renderingData.Source;

            graphicsDevice.SetRenderTarget(null);
            foreach (PostProcessor postProcessor in postProcessors)
            {
                postProcessor.Process(context, renderingData);
                renderTarget = renderingData.Destination;
                renderingData.Destination = renderingData.Source;
                renderingData.Source = renderTarget;
            }
            graphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone);
            spriteBatch.Draw(renderTarget, renderingData.Viewport, Color.White);
            spriteBatch.End();

            foreach (RendererFeature renderer in rendererFeaturesAfterPostProcessors)
            {
                Material material = renderer.Material;
                Effect effect = material.Effect;

                spriteBatch.Begin(SpriteSortMode.Deferred, material.BlendState, material.SamplerState, material.DepthStencilState, RasterizerState.CullNone);
                spriteBatch.Draw(renderer.RenderTexture, renderingData.Viewport, Color.White);
                spriteBatch.End();
            }
        }

        public void Blit(Texture2D source, RenderTarget2D destination)
        {
            Blit(source, destination, Material.BlitMaterial);
        }

        public void Blit(Texture2D source, RenderTarget2D destination, Material material)
        {
            graphicsDevice.SetRenderTarget(destination);
            spriteBatch.Begin(SpriteSortMode.Deferred, material.BlendState, material.SamplerState, material.DepthStencilState, RasterizerState.CullNone, material.Effect);
            spriteBatch.Draw(source, source.Bounds, Color.White);
            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);
        }

        public void DrawRenderers(RenderContext context, RenderTarget2D renderTarget, Color color)
        {
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.Clear(color);

            foreach (Renderer renderer in Renderers)
            {
                renderer?.Draw(context.GameTime, spriteBatch);
            }

            graphicsDevice.SetRenderTarget(null);
        }

        public void DrawRenderers(RenderContext context, RenderTarget2D renderTarget, Color color, string tag)
        {
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.Clear(color);

            foreach (Renderer renderer in Renderers)
            {
                if (renderer != null && renderer.Has(tag))
                {
                    renderer.Draw(context.GameTime, spriteBatch);
                }
            }

            graphicsDevice.SetRenderTarget(null);
        }

        private void OnCameraSetup()
        {
            renderingData.ViewportAdapter = game.WindowViewportAdapter;
            renderingData.Viewport = game.WindowViewportAdapter.Viewport.Bounds;

            renderingData.Source = new RenderTarget2D(graphicsDevice, renderingData.Viewport.Width, renderingData.Viewport.Height);
            renderingData.Destination = new RenderTarget2D(graphicsDevice, renderingData.Viewport.Width, renderingData.Viewport.Height);

            foreach (RendererFeature renderer in rendererFeatures)
            {
                renderer.OnCameraSetup(ref renderingData);
            }

            foreach (PostProcessor postProcessor in postProcessors)
            {
                postProcessor.OnCameraSetup(ref renderingData);
            }

            foreach (RendererFeature postProcessor in rendererFeaturesAfterPostProcessors)
            {
                postProcessor.OnCameraSetup(ref renderingData);
            }
        }
    }
}
