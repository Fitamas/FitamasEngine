using Fitamas.DebugTools;
using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fitamas.Fonts
{
    [Flags]
    public enum Alignment
    {
        CenterX = 1, 
        Left = 2, 
        Right = 4, 
        CenterY = 8, 
        Top = 16, 
        Bottom = 32,
        TopLeft = (Top | Left), 
        TopCenter = (CenterX | Top), 
        TopRight = (Top | Right),
        CenterLeft = (Left | CenterY), 
        Center = (CenterX | CenterY), 
        CenterRight = (Right | CenterY),
        BottomLeft = (Bottom | Left), 
        BottomCenter = (CenterX | Bottom), 
        BottomRight = (Bottom | Right),
        Any = Left | Right | Top | Bottom | Center
    }

    [AssetType]
    public class FontAtlas : MonoContentObject
    {
        public struct Glyph
        {
            public char Character;
            public Rectangle Bounds;
            public Rectangle Cropping;
            public float LeftSideBearing;
            public float Width;
            public float RightSideBearing;

            public float WidthIncludingBearings => LeftSideBearing + Width + RightSideBearing;
        }

        [SerializeField] private Texture2D texture;
        [SerializeField] private Glyph[] glyphs;
        [SerializeField] private int size;
        [SerializeField] private int lineSpacing;
        [SerializeField] private float spacing;
        [SerializeField] private int defaultCharacterIndex;

        public Texture2D Texture => texture;
        public IReadOnlyCollection<Glyph> Glyphs => glyphs;
        public int Size => size;
        public int LineSpacing => lineSpacing;
        public float Spacing => spacing;
        public char? DefaultCharacter => defaultCharacterIndex >= 0 ? glyphs[defaultCharacterIndex].Character : null;

        private FontAtlas()
        {

        }

        public FontAtlas(Texture2D texture, Glyph[] glyphs, int size, int lineSpacing, float spacing, char? defaultCharacter)
        {
            this.texture = texture;
            this.glyphs = glyphs;
            this.size = size;
            this.lineSpacing = lineSpacing;
            this.spacing = spacing;

            if (defaultCharacter.HasValue)
            {
                for (int i = 0; i < glyphs.Length; i++)
                {
                    if (glyphs[i].Character == defaultCharacter.Value)
                    {
                        defaultCharacterIndex = i;
                    }
                }
            }
        }

        public Vector2 MeasureString(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new Vector2(0, size);
            }

            Vector2 result = new Vector2(0, size);
            bool newLine = true;

            for (int i = 0; i < text.Length; i++)
            {
                switch (text[i])
                {
                    case '\n':
                        result.X = 0f;
                        result.Y += size + LineSpacing;
                        newLine = true;
                        continue;
                    case '\r':
                        continue;
                }

                Glyph glyph = GetGlyphOrDefault(text[i]);

                if (newLine)
                {
                    result.X = System.Math.Max(glyph.LeftSideBearing, 0f);
                    newLine = false;
                }
                else
                {
                    result.X += Spacing + glyph.LeftSideBearing;
                }

                result.X += glyph.Width + glyph.RightSideBearing;
            }

            return result;
        }

        public bool TryGetGlyph(char c, out Glyph result)
        {
            foreach (var glyph in glyphs)
            {
                if (glyph.Character == c)
                {
                    result = glyph;
                    return true;
                }
            }

            result = default;
            return false;
        }

        public Glyph GetGlyphOrDefault(char c)
        {
            if (TryGetGlyph(c, out Glyph glyph))
            {
                return glyph;
            }

            if (defaultCharacterIndex != -1)
            {
                return glyphs[defaultCharacterIndex];
            }

            throw new ArgumentException("defaultCharacterIndex");
        }
    }
}
