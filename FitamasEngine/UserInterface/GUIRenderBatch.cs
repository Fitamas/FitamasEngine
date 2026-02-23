using Fitamas.Fonts;
using Fitamas.Graphics;
using Fitamas.Math;
using Fitamas.UserInterface.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fitamas.UserInterface
{
    public class GUIRenderBatch
    {
        private AlphaTestEffect effect;
        private SpriteBatch spriteBatch;
        private RasterizerState rasterState;
        private Rectangle clipRectangle;

        public Rectangle ClipRectangle => clipRectangle;

        public GUIRenderBatch(GraphicsDevice graphicsDevice)
        {
            spriteBatch = new SpriteBatch(graphicsDevice);
            effect = new AlphaTestEffect(graphicsDevice);
            effect.VertexColorEnabled = true;

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

            Matrix view = Camera.Current.ViewportScaleMatrix;
            Matrix projection = Camera.Current.GetProjectionMatrix();

            effect.View = view;
            effect.Projection = projection;

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointWrap,
                              DepthStencilState.None, rasterState, effect);
        }

        public void End()
        {
            spriteBatch.End();
        }

        public void FillRectangle(Rectangle rectangle, Color color, float alpha)
        {
            Draw(TextureHelper.DefaultTexture, rectangle, color, alpha);
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

        public void Draw(FontAtlas font, string text, float size, Point position, Color color, float alpha)
        {
            color.A = (byte)(alpha * color.A);
            Vector2 origin = font.MeasureString(text);
            spriteBatch.Draw(font, text, size, position.ToVector2(), color, 0, new Vector2(0, 0), SpriteEffects.None, 1);
        }

        public void Draw(Sprite sprite, Color color, Rectangle rectangle, GUIImageEffect effect, float alpha)
        {
            color.A = (byte)(alpha * color.A);

            Vector2 position = rectangle.Location.ToVector2();
            Vector2 sourceSize = sprite.DefaultFrame.Bounds.Size.ToVector2();
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
