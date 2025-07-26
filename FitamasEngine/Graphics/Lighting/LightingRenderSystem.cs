using Fitamas.Core;
using Fitamas.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Fitamas.Graphics.Lighting
{
    //class RenderingData : IDisposable
    //{
    //    /// <summary>
    //    /// Internally set to true when the given RenderTexture <see cref="Output"/> was not the good size regarding <see cref="viewPort"/> and needed to be recreated
    //    /// </summary>
    //    public bool SizeMissmatched;
    //    /// <summary>The stage that possess every object in your view</summary>
    //    //public Stage stage;
    //    /// <summary>Callback to update the Camera position. Only done in First phase.</summary>
    //    //public ICameraUpdater updater;
    //    /// <summary>Viewport size</summary>
    //    //public Rect viewPort;
    //    /// <summary>Render texture handling captured image</summary>
    //    public RenderTarget2D Output;

    //    private bool disposed = false;

    //    /// <summary>Dispose pattern</summary>
    //    public void Dispose()
    //    {
    //        if (disposed)
    //            return;
    //        disposed = true;

    //        //stage = null;
    //        //updater = null;
    //        Output?.Dispose();
    //        Output = null;
    //    }
    //}

    //public class RenderLayers
    //{
    //    private GraphicsDevice graphicsDevice;
    //    private List<RenderTarget2D> layers;

    //    public IReadOnlyList<RenderTarget2D> Layers => layers;

    //    public RenderLayers(GraphicsDevice graphicsDevice)
    //    {
    //        this.graphicsDevice = graphicsDevice;
    //        //graphicsDevice.
    //        layers = new List<RenderTarget2D>();
    //    }

    //    public RenderTarget2D Create()
    //    {
    //        RenderTarget2D layer = new RenderTarget2D(graphicsDevice, 0, 0);
    //        layers.Add(layer);
    //        return layer;
    //    }
    //}

    public class LightComponent : Component
    {
        public Color Color;
        public float Intensity;
        public Sprite Sprite;
    }

    public class LightingRenderSystem : EntityDrawSystem
    {
        //LightingEffect lighting;
        //Texture2D spot;
        //Texture2D texture;
        //Texture2D lightMaskTexture;
        LightingEffect.LightData[] lights;
        SpriteBatch spriteBatch;
        GraphicsDevice graphicsDevice;

        private RenderTarget2D colorRT;
        private RenderTarget2D distanceRT;
        private RenderTarget2D jumpFloodRT;
        private RenderTarget2D giRT;

        private Effect effect;
        private LightComponent lightComponent;

        public LightingRenderSystem(GameEngine game, Renderer renderer) : base(Aspect.All())
        {
            graphicsDevice = game.GraphicsDevice;
            spriteBatch = new SpriteBatch(graphicsDevice);

            //var effect = game.Content.Load<Effect>("Lighting");
            //lighting = new LightingEffect(effect, game.GraphicsDevice);

            effect = game.Content.Load<Effect>("RadianceCascades2D");

            //default
            //light
            //water


            //ui


            PresentationParameters parameters = graphicsDevice.PresentationParameters;
            colorRT = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height, false,
                                              parameters.BackBufferFormat, parameters.DepthStencilFormat);

            jumpFloodRT = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height, false,
                                  parameters.BackBufferFormat, parameters.DepthStencilFormat);

            distanceRT = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height, false,
                      parameters.BackBufferFormat, parameters.DepthStencilFormat);

            giRT = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height, false,
                      parameters.BackBufferFormat, parameters.DepthStencilFormat);


            //renderTarget = layers.Create();

            //lighting.AmbientColor = Color.White;// new Vector3(0.6f, 0.6f, 0.6f);
            //lighting.AmbientIntensity = 0.3f;

            //spot = game.Content.Load<Texture2D>("Spot");
            //texture = new Texture2D(graphicsDevice, 256, 256);
            //lighting.Effect.Parameters["MainTexture"].SetValue(texture);

            //texture = CreateNoiseTexture(256, 256);//new Texture2D(GameEngine.Instance.GraphicsDevice, 256, 256, false, SurfaceFormat.Color);
            //texture.SetData(new[] { Color.White });

            // Создаем текстуру маски (может быть шумом или градиентом)
            //lightMaskTexture = CreateNoiseTexture(256, 256);
            //lighting.SetLightMask(lightMaskTexture);

            // Настройка источников света
            //lights = new LightingEffect.LightData[0];
            //lights = new LightingEffect.LightData[2];
            //lights[0] = new LightingEffect.LightData
            //{
            //    Position = new Vector2(1000, 700),
            //    Color = new Vector3(1, 0.8f, 0.6f),
            //    Radius = 2,
            //    Intensity = 0.8f
            //};
            //lights[1] = new LightingEffect.LightData
            //{
            //    Position = new Vector2(200, 200),
            //    Color = new Vector3(0.6f, 0.8f, 1),
            //    Radius = 0.8f,
            //    Intensity = 0.8f
            //};

            lightComponent = new LightComponent()
            {
                Color = Color.White,
                Intensity = 1,
                Sprite = Sprite.Create("Pumpkin"),
            };
        }

        public override void Initialize(IComponentMapperService mapperService)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            Matrix view = Camera.Current.GetViewMatrix();
            Matrix projection = Camera.Current.GetProjectionMatrix();
            Matrix.Multiply(ref view, ref projection, out var result);
            //lighting.Effect.Parameters["WorldViewProj"]?.SetValue(result);
            effect.Parameters["WorldViewProj"]?.SetValue(projection);

            Transform transform = new Transform();

            //lights[0].Position = new Vector2(1, 1);
            //lights[1].Position = new Vector2(3, 3);
            //lighting.SetLights(lights, lights.Length);

            graphicsDevice.SetRenderTarget(colorRT);
            graphicsDevice.Clear(Color.Transparent); // Очищаем только RenderTarget
            //graphicsDevice.Clear(Camera.Current.Color);


            //сделать рендер и лайт рендер для освещения и постпроцессов

            //spriteBatch.Begin();

            //spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointWrap,
            //      DepthStencilState.Default, RasterizerState.CullNone, lighting.Effect, Camera.Current.TranslationVirtualMatrix);
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointWrap,
                            DepthStencilState.Default, RasterizerState.CullNone, null, Camera.Current.TranslationVirtualMatrix);

            //spriteBatch.Draw(spot, transform.Position, spot.Bounds, Color.White, 0,
            //                 Vector2.Zero, Vector2.One / 32, SpriteEffects.None, 0);

            spriteBatch.Draw(lightComponent.Sprite, transform.Position, lightComponent.Sprite.Bounds, lightComponent.Color, 0,
                 Vector2.Zero, Vector2.One / 32, SpriteEffects.None, 0);



            spriteBatch.End();

            //graphicsDevice.Clear(Color.CornflowerBlue);





            Vector2 screen = colorRT.Bounds.Size.ToVector2();
            effect.Parameters["_Aspect"].SetValue(screen / MathF.Max(screen.X, screen.Y));


            graphicsDevice.SetRenderTarget(jumpFloodRT);
            graphicsDevice.Clear(Color.Transparent);

            effect.Parameters["_MainTex"].SetValue(colorRT);
            effect.CurrentTechnique = effect.Techniques["ScreenUVDrawing"];
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointWrap,
                            DepthStencilState.Default, RasterizerState.CullNone, effect);
            spriteBatch.Draw(colorRT, colorRT.Bounds, Color.White);
            spriteBatch.End();





            //float max = MathF.Max(screen.X, screen.Y);
            //int steps = (int)MathF.Ceiling(MathF.Log(max));
            //float stepSize = 1;

            //graphicsDevice.SetRenderTarget(null);

            //effect.CurrentTechnique = effect.Techniques["JumpFloodDrawing"];
            //spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointWrap,
            //                DepthStencilState.Default, RasterizerState.CullNone, effect);


            //for (var n = 0; n < steps; n++)
            //{
            //    effect.Parameters["_MainTex"].SetValue(jumpFloodRT);

            //    //graphicsDevice.SetRenderTarget(jumpFloodRT);
            //    stepSize *= 0.5f;
            //    effect.Parameters["_StepSize"].SetValue(stepSize);
            //    //you might find setting this value as global is unecessary but for some reason when using for loops you can't set the value directly to the material with Material.SetFloat
            //    //The value don't get passed correctly. Why, I have no idea

            //    spriteBatch.Draw(jumpFloodRT, jumpFloodRT.Bounds, Color.White);

            //    //BlitJumpFloodRT(cmd, jumpFloodMat);//Chooses the source and destination texture based on "jumpFlood1IsFinal" boolean
            //}
            //spriteBatch.End();




            //            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointWrap,
            //DepthStencilState.Default, RasterizerState.CullNone);
            //            spriteBatch.Draw(distanceRT, distanceRT.Bounds, Color.White);
            //            spriteBatch.End();



            graphicsDevice.SetRenderTarget(distanceRT);
            graphicsDevice.Clear(Color.Transparent);

            effect.Parameters["_MainTex"].SetValue(jumpFloodRT);
            effect.CurrentTechnique = effect.Techniques["DistanceDrawing"];
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointWrap,
                DepthStencilState.Default, RasterizerState.CullNone, effect);
            spriteBatch.Draw(jumpFloodRT, jumpFloodRT.Bounds, Color.White);
            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);



    //        spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointWrap,
    //DepthStencilState.Default, RasterizerState.CullNone);
    //        spriteBatch.Draw(distanceRT, distanceRT.Bounds, Color.White);
    //        spriteBatch.End();


            int count = 6;

            effect.Parameters["_MainTex"].SetValue(colorRT);
            effect.Parameters["_ColorTex"].SetValue(colorRT);
            effect.Parameters["_DistanceTex"].SetValue(distanceRT);

            effect.Parameters["_CascadeCount"].SetValue(count);
            effect.Parameters["_CascadeResolution"].SetValue(colorRT.Bounds.Size.ToVector2());
            effect.Parameters["_RayRange"].SetValue(1f);
            effect.Parameters["_SkyRadiance"].SetValue(1f);
            effect.Parameters["_SkyColor"].SetValue(new Color(0.2f, 0.5f, 1f).ToVector3());
            effect.Parameters["_SunColor"].SetValue(new Color(1f, 0.7f, 0.1f).ToVector3());
            effect.Parameters["_SunAngle"].SetValue(2f);


            effect.CurrentTechnique = effect.Techniques["CascadesDrawing"];
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointWrap,
                            DepthStencilState.Default, RasterizerState.CullNone, effect);
            for (int i = 0; i < count; i++)
            {
                effect.Parameters["_CascadeLevel"].SetValue(i);

                spriteBatch.Draw(colorRT, colorRT.Bounds, Color.White);
            }
            spriteBatch.End();


            graphicsDevice.SetRenderTarget(null);
        }

        private Texture2D CreateNoiseTexture(int width, int height)
        {
            Texture2D texture = new Texture2D(graphicsDevice, width, height);
            Color[] data = new Color[width * height];
            Random rand = new Random();

            for (int i = 0; i < data.Length; i++)
            {
                byte value = (byte)rand.Next(0, 10);
                data[i] = new Color(value, value, value, value);
            }

            texture.SetData(data);
            return texture;
        }
    }
}
