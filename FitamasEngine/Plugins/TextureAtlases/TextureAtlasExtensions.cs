/*
    The MIT License (MIT)

    Copyright (c) 2015-2024:
    - Dylan Wilson (https://github.com/dylanwilson80)
    - Lucas Girouard-Stranks (https://github.com/lithiumtoast)
    - Christopher Whitley (https://github.com/aristurtledev)

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Extended.TextureAtlases
{
    public static class TextureAtlasExtensions
    {
        public static void Draw(this SpriteBatch spriteBatch, TextureRegion2D textureRegion, Vector2 position, Color color, Rectangle? clippingRectangle = null)
        {
            Draw(spriteBatch, textureRegion, position, color, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0, clippingRectangle);
        }

        public static void Draw(this SpriteBatch spriteBatch, TextureRegion2D textureRegion, Vector2 position, Color color,
            float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth, Rectangle? clippingRectangle = null)
        {
            var sourceRectangle = textureRegion.Bounds;

            if (clippingRectangle.HasValue)
            {
                var x = (int)(position.X - origin.X);
                var y = (int)(position.Y - origin.Y);
                var width = (int)(textureRegion.Width * scale.X);
                var height = (int)(textureRegion.Height * scale.Y);
                var destinationRectangle = new Rectangle(x, y, width, height);

                sourceRectangle = ClipSourceRectangle(textureRegion.Bounds, destinationRectangle, clippingRectangle.Value);
                position.X += (sourceRectangle.X - textureRegion.Bounds.X) * scale.X;
                position.Y += (sourceRectangle.Y - textureRegion.Bounds.Y) * scale.Y;

                if (sourceRectangle.Width <= 0 || sourceRectangle.Height <= 0)
                    return;
            }

            spriteBatch.Draw(textureRegion.Texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }

        private static Rectangle ClipSourceRectangle(Rectangle sourceRectangle, Rectangle destinationRectangle, Rectangle clippingRectangle)
        {
            var left = (float)(clippingRectangle.Left - destinationRectangle.Left);
            var right = (float)(destinationRectangle.Right - clippingRectangle.Right);
            var top = (float)(clippingRectangle.Top - destinationRectangle.Top);
            var bottom = (float)(destinationRectangle.Bottom - clippingRectangle.Bottom);
            var x =  left > 0 ? left : 0;
            var y = top > 0 ? top : 0;
            var w = (right > 0 ? right : 0) + x;
            var h = (bottom > 0 ? bottom : 0) + y;

            var scaleX = (float)destinationRectangle.Width / sourceRectangle.Width;
            var scaleY = (float)destinationRectangle.Height / sourceRectangle.Height;
            x /= scaleX;
            y /= scaleY;
            w /= scaleX;
            h /= scaleY;

            return new Rectangle((int)(sourceRectangle.X + x), (int)(sourceRectangle.Y + y), (int)(sourceRectangle.Width - w), (int)(sourceRectangle.Height - h));
        }
    }
}