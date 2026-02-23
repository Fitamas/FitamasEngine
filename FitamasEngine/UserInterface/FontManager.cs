using Fitamas.Fonts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Linq;

namespace Fitamas.UserInterface
{
    public class FontManager
    {
        public static readonly string DefaultFontName = "Arial";
        public static readonly int TexDims = 512;
        public static readonly int Size = 32;
        public static readonly int LineSpacing = 4;
        public static readonly float Spacing = 4;
        public static readonly char? DefaultCharacter = '?';
        public static readonly FontAsset DefaultFontAsset;

        public static FontAtlas DefaultFont { get; set; }

        static FontManager()
        {
            string fontsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Fonts");
   
            string[] ttfFiles = Directory.GetFiles(fontsFolder, "*.ttf");
            string filePath = ttfFiles.FirstOrDefault(f => Path.GetFileName(f).StartsWith(DefaultFontName, StringComparison.OrdinalIgnoreCase));

            DefaultFontAsset = new FontAsset(filePath);
            DefaultFont = DefaultFontAsset.CreateAtlas(TexDims, Size, LineSpacing, Spacing, DefaultCharacter);
        }

        //public static int GetHeight()
        //{
        //    return GetHeight(DefaultFont);
        //}

        //public static int GetHeight(SpriteFont font)
        //{
        //    return GetDefaultCharacterSize(font).Y;
        //}

        //public static Point GetDefaultCharacterSize()
        //{
        //    return GetDefaultCharacterSize(DefaultFont);
        //}

        //public static Point GetDefaultCharacterSize(SpriteFont font)
        //{
        //    if (font == null)
        //    {
        //        return Point.Zero;
        //    }

        //    return font.MeasureString(font.DefaultCharacter.ToString()).ToPoint();
        //}
    }
}
