using Fitamas.Core;
using Fitamas.Math;
using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpFont;
using System;
using System.Collections.Generic;


namespace Fitamas.Fonts
{
    public class FontAsset : MonoContentObject
    {
        private static Library library = new Library();

        private Face face;

        public int FaceCount => face.FaceCount;
        public int FaceIndex => face.FaceIndex;
        public string FamilyName => face.FamilyName;
        public string StyleName => face.StyleName;

        private FontAsset()
        {

        }

        internal FontAsset(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            this.face = new Face(library, path);
        }

        public override void LoadData(string path)
        {
            face = new Face(library, path);
        }

        public FontAtlas CreateAtlas(int texDims, int size, int lineSpacing, float spacing, char? defaultCharacter)
        {
            GraphicsDevice graphicsDevice = GameEngine.Instance.GraphicsDevice;
            Texture2D texture = new Texture2D(graphicsDevice, texDims, texDims);
            return CreateAtlas(texture, size, lineSpacing, spacing, defaultCharacter);
        }

        public FontAtlas CreateAtlas(Texture2D texture, int size, int lineSpacing, float spacing, char? defaultCharacter)
        {
            int texDims = System.Math.Min(texture.Width, texture.Height);
            uint[] charRanges = new uint[] { 0x20, 0xFFFF };
            uint[] pixelBuffer = new uint[texDims * texDims];
            for (int i = 0; i < texDims * texDims; i++)
            {
                pixelBuffer[i] = 0;
            }

            List<FontAtlas.Glyph> glyphs = new List<FontAtlas.Glyph>();
            Vector2 currentCoords = Vector2.Zero;
            int nextY = 0;

            face.SetPixelSizes(0, (uint)size);
            face.LoadGlyph(face.GetCharIndex(defaultCharacter.Value), LoadFlags.Default, LoadTarget.Normal);
            int baseHeight = face.Glyph.Metrics.Height.ToInt32();

            for (int i = 0; i < charRanges.Length; i += 2)
            {
                uint start = charRanges[i];
                uint end = charRanges[i + 1];
                for (uint j = start; j <= end; j++)
                {
                    uint glyphIndex = face.GetCharIndex(j);
                    if (glyphIndex == 0)
                    {
                        continue;
                    }
                    face.LoadGlyph(glyphIndex, LoadFlags.Default, LoadTarget.Normal);
                    if (face.Glyph.Metrics.Width == 0 || face.Glyph.Metrics.Height == 0)
                    {
                        continue;
                    }
                    
                    face.Glyph.RenderGlyph(RenderMode.Normal);
                    byte[] bitmap = face.Glyph.Bitmap.BufferData;
                    int glyphWidth = face.Glyph.Bitmap.Width;
                    int glyphHeight = bitmap.Length / glyphWidth;

                    if (glyphWidth > texDims - 1 || glyphHeight > texDims - 1)
                    {
                        throw new Exception("Glyph dimensions exceed texture atlas dimensions");
                    }

                    nextY = System.Math.Max(nextY, glyphHeight + 2);

                    if (currentCoords.X + glyphWidth + 2 > texDims - 1)
                    {
                        currentCoords.X = 0;
                        currentCoords.Y += nextY;
                        nextY = 0;
                    }
                    if (currentCoords.Y + glyphHeight + 2 > texDims - 1)
                    {
                        break;
                    }

                    GlyphMetrics metrics = face.Glyph.Metrics;
                    glyphs.Add(new FontAtlas.Glyph()
                    {
                        Character = (char)j,
                        Bounds = new Rectangle((int)currentCoords.X, (int)currentCoords.Y, glyphWidth, glyphHeight),
                        LeftSideBearing = (float)metrics.HorizontalBearingX,
                        Width = (float)metrics.Width,
                        RightSideBearing = (float)(metrics.HorizontalAdvance - metrics.Width - metrics.HorizontalBearingX)
                    });

                    for (int y = 0; y < glyphHeight; y++)
                    {
                        for (int x = 0; x < glyphWidth; x++)
                        {
                            byte byteColor = bitmap[x + y * glyphWidth];
                            pixelBuffer[((int)currentCoords.X + x) + ((int)currentCoords.Y + y) * texDims] = (uint)(byteColor << 24 | 0x00ffffff);
                        }
                    }

                    currentCoords.X += glyphWidth + 2;
                }
            }

            texture.SetData(pixelBuffer);

            FontAtlas font = new FontAtlas(texture, glyphs.ToArray(), size, lineSpacing, spacing, defaultCharacter);

            return font;
        }
    }
}
