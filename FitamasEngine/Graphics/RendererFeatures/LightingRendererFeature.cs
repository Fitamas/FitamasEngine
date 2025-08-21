using Fitamas.ECS;
using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Fitamas.Graphics.RendererFeatures
{
    public class LightingRendererFeature : RendererFeature
    {
        SpriteBatch SpriteBatch;

        private RenderTarget2D colorRT;
        private RenderTarget2D distanceRT;
        private TwoPassRenderTarget2D jumpFloodRT;
        private TwoPassRenderTarget2D giRT;

        private Material RadianceCascadesMaterial;
        private Material blitMaterial;
        private Vector2 cascadeResolution;

        public float RenderScale = 1;
        public int CascadeCount = 4;

        public LightingRendererFeature(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {

            SpriteBatch = new SpriteBatch(graphicsDevice);
        }

        public override void OnCameraSetup(ref RenderingData renderingData)
        {
            base.OnCameraSetup(ref renderingData);

            Point screen = new Point(renderingData.Viewport.Width, renderingData.Viewport.Height);

            colorRT = new RenderTarget2D(GraphicsDevice, screen.X, screen.Y, false, SurfaceFormat.Color, DepthFormat.None);
            jumpFloodRT = new TwoPassRenderTarget2D(GraphicsDevice, screen.X, screen.Y, false, SurfaceFormat.Vector2);
            distanceRT = new RenderTarget2D(GraphicsDevice, screen.X, screen.Y, false, SurfaceFormat.Single, DepthFormat.None);

            int cascadeWidth = (int)MathF.Ceiling((screen.X * RenderScale) / MathF.Pow(2, CascadeCount)) * (int)MathF.Pow(2, CascadeCount);
            int cascadeHeight = (int)MathF.Ceiling((screen.Y * RenderScale) / MathF.Pow(2, CascadeCount)) * (int)MathF.Pow(2, CascadeCount);
            cascadeResolution = new Vector2(cascadeWidth, cascadeHeight);

            giRT = new TwoPassRenderTarget2D(GraphicsDevice, cascadeWidth, cascadeHeight, false, SurfaceFormat.Color);

            Effect effect = Resources.Load<Effect>("RadianceCascades2D");
            RadianceCascadesMaterial = new Material(effect);
            RadianceCascadesMaterial.BlendState = BlendState.Opaque;
            //RadianceCascadesMaterial.SamplerState = SamplerState.LinearClamp;

            blitMaterial = Material.BlitMaterial.Clone();
            blitMaterial.BlendState = BlendState.AlphaBlend;
            blitMaterial.SamplerState = SamplerState.LinearClamp;

        }

        public override void Draw(RenderContext context, RenderingData renderingData)
        {
            Effect effect = RadianceCascadesMaterial.Effect;
            Matrix view = Camera.Current.GetViewMatrix();
            Matrix projection = Camera.Current.GetProjectionMatrix();
            Matrix.Multiply(ref view, ref projection, out var result);
            effect.Parameters["WorldViewProj"]?.SetValue(projection);

            context.DrawRenderers(context, colorRT, Color.Transparent, Tags.Light);

            Vector2 screen = new Vector2(renderingData.Viewport.Width, renderingData.Viewport.Height);

            effect.Parameters["_Aspect"].SetValue(screen / MathF.Max(screen.X, screen.Y));
            effect.Parameters["_MainTex"].SetValue(colorRT);
            effect.CurrentTechnique = effect.Techniques["ScreenUVDrawing"];

            context.Blit(colorRT, jumpFloodRT.GetResult(), RadianceCascadesMaterial);


            float max = MathF.Max(screen.X, screen.Y);
            int steps = (int)MathF.Ceiling(MathF.Log(max));
            float stepSize = 1;

            effect.CurrentTechnique = effect.Techniques["JumpFloodDrawing"];

            for (var n = 0; n < steps; n++)
            {
                stepSize *= 0.5f;
                effect.Parameters["_StepSize"].SetValue(stepSize);
                effect.Parameters["_MainTex"].SetValue(jumpFloodRT.GetResult());

                jumpFloodRT.ApplyEffect(context, RadianceCascadesMaterial);
            }

            effect.Parameters["_MainTex"].SetValue(jumpFloodRT.GetResult());
            effect.CurrentTechnique = effect.Techniques["DistanceDrawing"];

            context.Blit(jumpFloodRT.GetResult(), distanceRT, RadianceCascadesMaterial);


            effect.Parameters["_ColorTex"].SetValue(colorRT);
            effect.Parameters["_DistanceTex"].SetValue(distanceRT);

            effect.Parameters["_CascadeCount"].SetValue(CascadeCount);
            effect.Parameters["_CascadeResolution"].SetValue(cascadeResolution);
            effect.Parameters["_RayRange"].SetValue(1.5f);
            effect.Parameters["_SkyRadiance"].SetValue(1f);
            effect.Parameters["_SkyColor"].SetValue(new Vector3(0.2f, 0.5f, 1f));
            effect.Parameters["_SunColor"].SetValue(new Vector3(8, 5, 4));
            effect.Parameters["_SunAngle"].SetValue(5);

            effect.CurrentTechnique = effect.Techniques["CascadesDrawing"];

            for (int i = CascadeCount - 1; i >= 0; i--)
            {
                effect.Parameters["_CascadeLevel"].SetValue(i);
                effect.Parameters["_MainTex"].SetValue(giRT.GetResult());

                giRT.ApplyEffect(context, RadianceCascadesMaterial);
            }

            context.Blit(giRT.GetResult(), RenderTexture, blitMaterial);
        }
    }
}
