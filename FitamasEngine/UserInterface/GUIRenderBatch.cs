using Fitamas.Graphics;
using Fitamas.Math2D;
using Fitamas.UserInterface.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using System;

namespace Fitamas.UserInterface
{
    public class GUIRenderBatch
    {
        private GraphicsDevice graphicsDevice;
        private BasicEffect effect;
        private SpriteBatch spriteBatch;
        private RasterizerState rasterState;
        private Rectangle clipRectangle;

        public GUIRenderBatch(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            effect = new BasicEffect(graphicsDevice);
            spriteBatch = new SpriteBatch(graphicsDevice);

            rasterState = new RasterizerState();
            rasterState.MultiSampleAntiAlias = true;
            rasterState.ScissorTestEnable = true;
            rasterState.FillMode = FillMode.Solid;
            rasterState.CullMode = CullMode.CullCounterClockwiseFace;
            rasterState.DepthBias = 0;
            rasterState.SlopeScaleDepthBias = 0;
        }

        public void Begin(Rectangle clipRectangle = default)
        {
            if (clipRectangle != Rectangle.Empty)
            {
                spriteBatch.GraphicsDevice.ScissorRectangle = clipRectangle;
                this.clipRectangle = clipRectangle;
            }            

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointWrap,
                              DepthStencilState.DepthRead, rasterState);
        }

        public void End()
        {
            spriteBatch.End();
        }

        public void Draw(Texture2D texture, Rectangle rectangle, Color color, int layer)
        {
            Draw(texture, rectangle, color, layer, 0);
        }

        public void Draw(Texture2D texture, Rectangle rectangle, Color color, int layer, float rotation)
        {
            spriteBatch.Draw(texture, rectangle, null, color, rotation, new Vector2(), SpriteEffects.None, layer);
        }

        public void DrawString(SpriteFont font, string text, Vector2 position, Color color, Vector2 origin, float scale, int layer = 0)
        {
            //if (!spriteBatch.GraphicsDevice.ScissorRectangle.Intersects(ContextRender.Mask))
            //{
            //    return;
            //}

            spriteBatch.DrawString(font, text, position, color, 0, origin, scale, SpriteEffects.None, layer);
        }

        public void FillRectangle(Point positiopn, Point scale, Color color, int layer)
        {
            Draw(Texture2DHelper.DefaultTexture, new Rectangle(positiopn, scale), color, layer);
        }

        public void Draw(TextureRegion2D textureRegion, Color color, RectangleF rectangle, GUIImageEffect effect, int layer = 0)
        {
            Vector2 sourceSize = textureRegion.Bounds.Size.ToVector2();
            Vector2 scale = rectangle.Size / sourceSize;

            spriteBatch.Draw(textureRegion, rectangle.Position, color, 0, Vector2.Zero, scale, (SpriteEffects)effect, layer);
        }

        public Point GetViewportSize()
        {
            if (Camera.Main != null)
            {
                return Camera.Main.VirtualSize.ToPoint();
            }

            return Point.Zero;
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
    }
}
