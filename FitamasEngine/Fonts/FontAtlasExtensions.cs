using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Fitamas.Fonts
{
    public static class FontAtlasExtensions
    {
        public static void Draw(this SpriteBatch spriteBatch, FontAtlas font, string text, float size, Vector2 position, 
            Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth, Rectangle? clippingRectangle = null)
        {
            float scale = font.Size != 0 ? size / font.Size : 0f;
            bool newLine = false;
            Vector2 currentPos = Vector2.Zero;
            Vector2 unit = rotation == 0.0f ? Vector2.UnitX : new Vector2(MathF.Cos(rotation), MathF.Sin(rotation));

            for (int i = 0; i < text.Length; i++)
            {
                bool flagVertically = (effects & SpriteEffects.FlipVertically) == SpriteEffects.FlipVertically;
                bool flagHorizontally = (effects & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally;

                switch (text[i])
                {
                    case '\n':
                        currentPos.X = 0f;
                        currentPos.Y += font.LineSpacing;
                        newLine = true;
                        continue;
                    case '\r':
                        continue;
                }

                FontAtlas.Glyph glyph = font.GetGlyphOrDefault(text[i]);

                if (newLine)
                {
                    currentPos.X = System.Math.Max(glyph.LeftSideBearing, 0f);
                    newLine = false;
                }
                else
                {
                    currentPos.X += font.Spacing + glyph.LeftSideBearing;
                }

                Vector2 drawPosition = position + currentPos * scale * unit;
                spriteBatch.Draw(font.Texture, drawPosition, glyph.Bounds, color, rotation, origin, scale, effects, layerDepth);

                currentPos.X += glyph.Width + glyph.RightSideBearing;
            }
        }
    }
}
