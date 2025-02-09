using System;
using Fitamas;
using Fitamas.Math2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fitamas.UserInterface.Extensions
{
    public static class SpriteFontExtensions
    {
        public static void DrawString(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position,
            Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float layerDepth, Rectangle? clippingRectangle = null)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            if (effect != SpriteEffects.None)
                throw new NotSupportedException($"{effect} is not currently supported for {nameof(SpriteFont)}");

            var glyphs = font.GetGlyphs();
            Vector2 zero = Vector2.Zero;
            foreach (var c in text)
            {
                var glyph = glyphs[c];
                var characterOrigin = origin;
                zero.X += font.Spacing + glyph.RightSideBearing;// (rtl ? ptr2->RightSideBearing : ptr2->LeftSideBearing);

                Vector2 pos = zero;
                pos.X += glyph.Cropping.X;
                pos.Y += glyph.Cropping.Y;

                Vector2 size = glyph.BoundsInTexture.Size.ToVector2();
                Rectangle rectangle = new RectangleF(position + pos * scale, size * scale).ToRectangle();
                rectangle = Rectangle.Intersect(rectangle, clippingRectangle.Value);

                spriteBatch.Draw(font.Texture, rectangle, glyph.BoundsInTexture, color, rotation, characterOrigin, effect, layerDepth);

                zero.X += glyph.Width + glyph.LeftSideBearing;
            }
        }

        public static void DrawString(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position,
            Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect, float layerDepth, Rectangle? clippingRectangle = null)
        {
            spriteBatch.DrawString(font, text, position, color, rotation, origin, new Vector2(scale, scale), effect, layerDepth, clippingRectangle);
        }

        public static void DrawString(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, float layerDepth, Rectangle? clippingRectangle = null)
        {
            spriteBatch.DrawString(font, text, position, color, rotation: 0, origin: Vector2.Zero, scale: Vector2.One, effect: SpriteEffects.None,
                layerDepth: layerDepth, clippingRectangle: clippingRectangle);
        }

        public static void DrawString(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, Rectangle? clippingRectangle = null)
        {
            spriteBatch.DrawString(font, text, position, color, rotation: 0, origin: Vector2.Zero, scale: Vector2.One, effect: SpriteEffects.None,
                layerDepth: 0, clippingRectangle: clippingRectangle);
        }
    }
}