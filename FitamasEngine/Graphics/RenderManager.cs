using Fitamas.Collections;
using Fitamas.Core;
using Fitamas.Graphics.PostProcessors;
using Fitamas.Graphics.RendererFeatures;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using R3;

namespace Fitamas.Graphics
{
    //public enum RenderQueue
    //{
    //    Opaque,
    //    Transparent,
    //    All,
    //}

    public class RenderManager : IInitializable
    {
        private GameEngine game;
        private List<RendererFeature> rendererFeatures;
        private List<RendererFeature> rendererFeaturesAfterPostProcessors;
        private List<PostProcessor> postProcessors;
        private GraphicsDevice graphicsDevice;
        private SpriteBatch spriteBatch;
        private RenderTarget2D finalRenderTarget;

        private RenderTarget2D renderTarget1;
        private RenderTarget2D renderTarget2;
        private Camera camera;

        private RenderingData renderingData;

        private bool isInitialized;
        private RenderContext context;

        public Bag<Renderer> Renderers { get; }
        public IFinalRender FinalRender;

        public RenderManager(GameEngine game)
        {
            this.game = game;
            this.graphicsDevice = game.GraphicsDevice;

            rendererFeatures = new List<RendererFeature>();
            rendererFeaturesAfterPostProcessors = new List<RendererFeature>();
            postProcessors = new List<PostProcessor>();
            spriteBatch = new SpriteBatch(graphicsDevice);
            Renderers = new Bag<Renderer>();

            //renderingData = new RenderingData();
            context = new RenderContext(this);
            //renderingData.PostProcessingEnabled = true;
        }

        public void Initialize()
        {
            if (!isInitialized)
            {
                isInitialized = true;
            }
            renderingData.ViewportAdapter = game.ViewportAdapterProperty.Value;
            SetupCamera();
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

            if (isInitialized)
            {
                rendererFeature.OnCameraSetup(ref renderingData);
            }
        }

        public void AddPostProcessor(PostProcessor postProcessor)
        {
            postProcessors.Add(postProcessor);

            postProcessor.Create();

            if (isInitialized)
            {
                postProcessor.OnCameraSetup(ref renderingData);
            }
        }

        public void Render(GameTime gameTime)
        {
            SetupCameraIfNeed();

            context.GameTime = gameTime;

            renderingData.Source = renderTarget1;
            renderingData.Destination = renderTarget2;
            renderingData.Camera = camera;

            foreach (RendererFeature renderer in rendererFeatures)
            {
                renderer.Draw(context, renderingData);
            }

            foreach (RendererFeature renderer in rendererFeaturesAfterPostProcessors)
            {
                renderer.Draw(context, renderingData);
            }

            graphicsDevice.SetRenderTarget(renderingData.Source);
            graphicsDevice.Clear(camera.Color);

            foreach (RendererFeature renderer in rendererFeatures)
            {
                Material material = renderer.Material;
                Effect effect = material.Effect;

                spriteBatch.Begin(SpriteSortMode.Deferred, material.BlendState, material.SamplerState, material.DepthStencilState, RasterizerState.CullNone);
                spriteBatch.Draw(renderer.RenderTexture, graphicsDevice.Viewport.Bounds, Color.White);
                spriteBatch.End();
            }

            graphicsDevice.SetRenderTarget(null);
            foreach (PostProcessor postProcessor in postProcessors)
            {
                postProcessor.Process(context, renderingData);
                RenderTarget2D renderTarget = renderingData.Destination;
                renderingData.Destination = renderingData.Source;
                renderingData.Source = renderTarget;
            }

            graphicsDevice.SetRenderTarget(renderingData.Destination);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone);
            spriteBatch.Draw(renderingData.Source, graphicsDevice.Viewport.Bounds, Color.White);
            spriteBatch.End();

            foreach (RendererFeature renderer in rendererFeaturesAfterPostProcessors)
            {
                Material material = renderer.Material;
                Effect effect = material.Effect;

                spriteBatch.Begin(SpriteSortMode.Deferred, material.BlendState, material.SamplerState, material.DepthStencilState, RasterizerState.CullNone);
                spriteBatch.Draw(renderer.RenderTexture, graphicsDevice.Viewport.Bounds, Color.White);
                spriteBatch.End();
            }

            if (FinalRender != null)
            {
                renderingData.Source = renderingData.Destination;
                renderingData.Destination = finalRenderTarget;
                graphicsDevice.SetRenderTarget(null);
                FinalRender.Draw(context, renderingData);
            }

            graphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone);
            spriteBatch.Draw(renderingData.Destination, graphicsDevice.Viewport.Bounds, Color.White);
            spriteBatch.End();
        }

        public void Render(Camera camera, RenderTarget2D renderTarget)
        {
            //TODO
        }

        public void Blit(Texture2D source, RenderTarget2D destination)
        {
            Blit(source, destination, Material.BlitMaterial);
        }

        public void Blit(Texture2D source, RenderTarget2D destination, Material material)
        {
            graphicsDevice.SetRenderTarget(destination);
            DrawTexture(source, material);
            graphicsDevice.SetRenderTarget(null);
        }

        public void DrawTexture(Texture2D source)
        {
            DrawTexture(source, Material.BlitMaterial);
        }

        public void DrawTexture(Texture2D source, Material material)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, material.BlendState, material.SamplerState, material.DepthStencilState, RasterizerState.CullNone, material.Effect);
            spriteBatch.Draw(source, source.Bounds, Color.White);
            spriteBatch.End();
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

        private void SetupCameraIfNeed()
        {
            camera = Camera.Main;


            renderingData.Camera = camera; //TODO
            renderingData.ViewportAdapter = game.ViewportAdapterProperty.Value;

            if (game.ViewportAdapterProperty.Value != renderingData.ViewportAdapter)
            {
                renderingData.NeedCameraSetup = true;
            }

            int wigth = renderingData.ViewportAdapter.ViewportWidth;
            int height = renderingData.ViewportAdapter.ViewportHeight;
            Rectangle viewport = new Rectangle(0, 0, wigth, height);
            if (viewport != renderingData.Viewport)
            {
                renderingData.NeedCameraSetup = true;
            }

            //if (wigth > 0 && height > 0)
            //{

            //}

            if (renderingData.NeedCameraSetup)
            {
                SetupCamera();
                renderingData.NeedCameraSetup = false;
            }
        }

        private void SetupCamera()
        {
            Point screen = game.Window.ClientBounds.Size;
            int wigth = renderingData.ViewportAdapter.ViewportWidth;
            int height = renderingData.ViewportAdapter.ViewportHeight;
            if (wigth == 0) wigth = 1;
            if (height == 0) height = 1;
            //renderingData.ViewportAdapter = camera.ViewportAdapter;
            renderingData.Viewport = new Rectangle(0, 0, wigth, height);
            renderTarget1 = new RenderTarget2D(graphicsDevice, wigth, height);
            renderTarget2 = new RenderTarget2D(graphicsDevice, wigth, height);
            finalRenderTarget = new RenderTarget2D(graphicsDevice, screen.X, screen.Y);

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
