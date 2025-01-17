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

using System;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace MonoGame.Extended.Content.Pipeline.BitmapFonts
{
    [ContentProcessor(DisplayName = "BMFont Processor - MonoGame.Extended")]
    public class BitmapFontProcessor : ContentProcessor<BitmapFontFile, BitmapFontProcessorResult>
    {
        public override BitmapFontProcessorResult Process(BitmapFontFile bitmapFontFile, ContentProcessorContext context)
        {
            try
            {
                context.Logger.LogMessage("Processing BMFont");
                var result = new BitmapFontProcessorResult(bitmapFontFile);

                foreach (var fontPage in bitmapFontFile.Pages)
                {
                    var assetName = Path.GetFileNameWithoutExtension(fontPage.File);
                    context.Logger.LogMessage("Expected texture asset: {0}", assetName);
                    result.TextureAssets.Add(assetName);
                }

                return result;
            }
            catch (Exception ex)
            {
                context.Logger.LogMessage("Error {0}", ex);
                throw;
            }
        }
    }
}