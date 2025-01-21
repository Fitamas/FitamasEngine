using Fitamas.Graphics;
using FitamasEngine.UserInterface.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Threading.Tasks;

namespace Fitamas.UserInterface
{
    public class GUIRenderBatch
    {
        private const float k = 1000f;

        private GraphicsDevice graphicsDevice;
        private BasicEffect effect;
        private SpriteBatch spriteBatch;

        public GUIContextRender ContextRender { get; set; }

        public GUIRenderBatch(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            effect = new BasicEffect(graphicsDevice);
            spriteBatch = new SpriteBatch(graphicsDevice);
        }

        public void Begin()
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointWrap,
                              DepthStencilState.DepthRead, RasterizerState.CullCounterClockwise);

            //spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap,
            //      DepthStencilState.DepthRead, RasterizerState.CullClockwise, null, camera.TranslationVirtualMatrix);
        }

        public void End()
        {
            spriteBatch.End();
        }

        public void Draw(Texture2D texture, Rectangle rectangle, Color color, int layer)
        {
            rectangle = Rectangle.Intersect(ContextRender.Mask, rectangle);

            DrawTexture(texture, rectangle, ContextRender.Mask, color, layer, 0);
        }

        public void DrawTexture(Texture2D texture, Rectangle rectangle, Rectangle mask, Color color, int layer, float rotation)
        {
            spriteBatch.Draw(texture, rectangle, mask, color, rotation, new Vector2(), SpriteEffects.None, layer / k);
        }

        public void DrawString(SpriteFont font, string text, Vector2 position, Color color, Vector2 origin, float scale, int layer = 0)
        {
            Rectangle mask = ContextRender.Mask;

            //if (clippingRectangle.HasValue)
            //{
            //    mask = Rectangle.Intersect(mask, clippingRectangle.Value);
            //}

            spriteBatch.DrawString(font, text, position, color, 0, origin, scale, SpriteEffects.None, layer / k, mask);
        }

        public void FillRectangle(Point positiopn, Point scale, Color color, int layer)
        {
            //Rectangle rectangle = Rectangle.Intersect(ContextRender.Mask, new Rectangle(positiopn, scale));
            //spriteBatch.FillRectangle(rectangle.Location.ToVector2(), rectangle.Size.ToVector2(), color, 0, layer / k);

            Draw(GUIStyle.DefoultTexture, new Rectangle(positiopn, scale), color, layer);
        }

        public void Draw(TextureRegion2D textureRegion, Color color, RectangleF rectangle, GUIImageEffect effect, int layer = 0, Rectangle? clippingRectangle = null)
        {
            Vector2 sourceSize = textureRegion.Bounds.Size.ToVector2();
            Vector2 scale = rectangle.Size / sourceSize;
            Rectangle mask = ContextRender.Mask;

            if (clippingRectangle.HasValue)
            {
                mask = Rectangle.Intersect(mask, clippingRectangle.Value);
            }

            spriteBatch.Draw(textureRegion, rectangle.Position, color, 0, Vector2.Zero,
                             scale, (SpriteEffects)effect, layer / k, mask);
        }

        //public void Draw(VertexPositionTexture[] vertices, int[] ind)
        //{
        //    SetStates();

        //    effect.World = Matrix.Identity;
        //    effect.View = Camera.Main.ViewportScaleMatrix * Matrix.CreateScale(1, -1, 1);
        //    effect.Projection = Camera.GetProjectionMatrix();

        //    foreach (var pass in effect.CurrentTechnique.Passes)
        //    {
        //        pass.Apply();                

        //        graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, ind, 0, ind.Length / 3);
        //    }

        //    void SetStates()
        //    {
        //        graphicsDevice.DepthStencilState = DepthStencilState.Default;
        //        graphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
        //        graphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
        //    }
        //}

        //public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Rectangle rectangle, Color color, float layerDepth = 0)
        //{
        //    Vector2 position = rectangle.Location.ToVector2();
        //    spriteBatch.Draw(texture, position, rectangle, color, 0, new Vector2(), 1, SpriteEffects.None, layerDepth);
        //}
    }
}
