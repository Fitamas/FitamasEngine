using Fitamas.Graphics;
using Fitamas.Graphics.TextureAtlases;
using Fitamas.Math2D;
using Fitamas.UserInterface.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
                clipRectangle = Transform(clipRectangle);
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

        public void Draw(Texture2D texture, Rectangle rectangle, Color color)
        {
            Draw(texture, rectangle, color, 0);
        }

        public void Draw(Texture2D texture, Rectangle rectangle, Color color, float rotation)
        {
            rectangle = Transform(rectangle);

            spriteBatch.Draw(texture, rectangle, null, color, rotation, Vector2.Zero, SpriteEffects.None, 1);
        }

        public void DrawString(SpriteFont font, string text, Point position, Color color, float scale)
        {
            Rectangle rectangle = Transform(new Rectangle(position, Point.Zero));
            Vector2 origin = font.MeasureString(text);
            spriteBatch.DrawString(font, text, rectangle.Location.ToVector2(), color, 0, new Vector2(0, origin.Y), scale, SpriteEffects.None, 1);
        }

        public void FillRectangle(Point position, Point scale, Color color)
        {
            Rectangle rectangle = new Rectangle(position, scale);

            Draw(Texture2DHelper.DefaultTexture, rectangle, color);
        }

        public void Draw(TextureRegion2D textureRegion, Color color, Rectangle rectangle, GUIImageEffect effect)
        {
            rectangle = Transform(rectangle);
            Vector2 position = rectangle.Location.ToVector2();
            Vector2 sourceSize = textureRegion.Bounds.Size.ToVector2();
            Vector2 scale = rectangle.Size.ToVector2() / sourceSize;

            spriteBatch.Draw(textureRegion, position, color, 0, Vector2.Zero, scale, (SpriteEffects)effect, 1);
        }

        public virtual Point GetViewportSize()
        {
            if (Camera.Current != null)
            {
                return Camera.Current.VirtualSize.ToPoint();
            }

            return Point.Zero;
        }

        protected virtual Rectangle Transform(Rectangle rectangle)
        {
            rectangle.Location = new Point(rectangle.X, (int)Camera.Current.VirtualSize.Y - rectangle.Y - rectangle.Height);
            return rectangle;
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
