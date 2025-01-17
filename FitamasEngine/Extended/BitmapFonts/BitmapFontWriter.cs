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

using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace MonoGame.Extended.Content.Pipeline.BitmapFonts
{
    [ContentTypeWriter]
    public class BitmapFontWriter : ContentTypeWriter<BitmapFontProcessorResult>
    {
        protected override void Write(ContentWriter writer, BitmapFontProcessorResult result)
        {
            writer.Write(result.TextureAssets.Count);

            foreach (var textureAsset in result.TextureAssets)
                writer.Write(textureAsset);

            var fontFile = result.FontFile;
            writer.Write(fontFile.Common.LineHeight);
            writer.Write(fontFile.Chars.Count);

            foreach (var c in fontFile.Chars)
            {
                writer.Write(c.Id);
                writer.Write(c.Page);
                writer.Write(c.X);
                writer.Write(c.Y);
                writer.Write(c.Width);
                writer.Write(c.Height);
                writer.Write(c.XOffset);
                writer.Write(c.YOffset);
                writer.Write(c.XAdvance);
            }

            writer.Write(fontFile.Kernings.Count);
            foreach(var k in fontFile.Kernings)
            {
                writer.Write(k.First);
                writer.Write(k.Second);
                writer.Write(k.Amount);
            }
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return "MonoGame.Extended.BitmapFonts.BitmapFont, Fitamas";
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "MonoGame.Extended.BitmapFonts.BitmapFontReader, Fitamas";
        }
    }
}