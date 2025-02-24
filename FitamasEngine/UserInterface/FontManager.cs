using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Fitamas.UserInterface
{
    public class FontManager
    {
        public static SpriteFont DefaultFont { get; set; }

        public static int GetHeight()
        {
            return GetHeight(DefaultFont);
        }

        public static int GetHeight(SpriteFont font)
        {
            return GetDefaultCharacterSize(font).Y;
        }

        public static Point GetDefaultCharacterSize()
        {
            return GetDefaultCharacterSize(DefaultFont);
        }

        public static Point GetDefaultCharacterSize(SpriteFont font)
        {
            if (font == null)
            {
                return Point.Zero;
            }

            return font.MeasureString(font.DefaultCharacter.ToString()).ToPoint();
        }
    }
}
