using Fitamas.Graphics;
using Fitamas.Math;
using Fitamas.UserInterface.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fitamas.UserInterface
{
    public class GUIRenderBatch
    {
        private GraphicsDevice graphicsDevice;
        private SpriteEffect effect;
        private SpriteBatch spriteBatch;
        private RasterizerState rasterState;
        private Rectangle clipRectangle;

        public Rectangle ClipRectangle => clipRectangle;

        public GUIRenderBatch(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            effect = new SpriteEffect(graphicsDevice);
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

        public void FillRectangle(Point position, Point scale, Color color, float alpha)
        {
            Rectangle rectangle = new Rectangle(position, scale);

            Draw(Texture2DHelper.DefaultTexture, rectangle, color, alpha);
        }

        public void Draw(Texture2D texture, Rectangle rectangle, Color color, float alpha)
        {
            Draw(texture, rectangle, color, 0, alpha);
        }

        public void Draw(Texture2D texture, Rectangle rectangle, Color color, float rotation, float alpha)
        {
            color.A = (byte)(alpha * color.A);
            spriteBatch.Draw(texture, rectangle, null, color, rotation, Vector2.Zero, SpriteEffects.None, 1);
        }

        public void DrawString(SpriteFont font, string text, Point position, Color color, float scale, float alpha)
        {
            color.A = (byte)(alpha * color.A);
            Vector2 origin = font.MeasureString(text);
            spriteBatch.DrawString(font, text, position.ToVector2(), color, 0, new Vector2(0, 0), scale, SpriteEffects.None, 1);
        }

        public void Draw(Sprite sprite, Color color, Rectangle rectangle, GUIImageEffect effect, float alpha)
        {
            color.A = (byte)(alpha * color.A);

            Vector2 position = rectangle.Location.ToVector2();
            Vector2 sourceSize = sprite.Bounds.Size.ToVector2();
            Vector2 scale = rectangle.Size.ToVector2() / sourceSize;

            spriteBatch.Draw(sprite, position, color, 0, Vector2.Zero, scale, (SpriteEffects)effect, 1);
        }

        public virtual Point GetViewportSize()
        {
            if (Camera.Current != null)
            {
                return Camera.Current.VirtualSize.ToPoint();
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
