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

using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Content;
using MonoGame.Extended.TextureAtlases;

namespace MonoGame.Extended.BitmapFonts
{
    public class BitmapFontReader : ContentTypeReader<BitmapFont>
    {
        protected override BitmapFont Read(ContentReader reader, BitmapFont existingInstance)
        {
            var textureAssetCount = reader.ReadInt32();
            var assets = new List<string>();

            for (var i = 0; i < textureAssetCount; i++)
            {
                var assetName = reader.ReadString();
                assets.Add(assetName);
            }

            var textures = assets
                .Select(textureName => reader.ContentManager.Load<Texture2D>(reader.GetRelativeAssetName(textureName)))
                .ToArray();

            var lineHeight = reader.ReadInt32();
            var regionCount = reader.ReadInt32();
            var regions = new BitmapFontRegion[regionCount];

            for (var r = 0; r < regionCount; r++)
            {
                var character = reader.ReadInt32();
                var textureIndex = reader.ReadInt32();
                var x = reader.ReadInt32();
                var y = reader.ReadInt32();
                var width = reader.ReadInt32();
                var height = reader.ReadInt32();
                var xOffset = reader.ReadInt32();
                var yOffset = reader.ReadInt32();
                var xAdvance = reader.ReadInt32();
                var textureRegion = new TextureRegion2D(textures[textureIndex], x, y, width, height);
                regions[r] = new BitmapFontRegion(textureRegion, character, xOffset, yOffset, xAdvance);
            }

            var characterMap = regions.ToDictionary(r => r.Character);
            var kerningsCount = reader.ReadInt32();

            for (var k = 0; k < kerningsCount; k++)
            {
                var first = reader.ReadInt32();
                var second = reader.ReadInt32();
                var amount = reader.ReadInt32();

                // Find region
                if (!characterMap.TryGetValue(first, out var region))
                    continue;

                region.Kernings[second] = amount;
            }

            return new BitmapFont(reader.AssetName, regions, lineHeight);
        }
    }
}