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
using System.Collections;
using System.Collections.Generic;
using Fitamas.DebugTools;
using Fitamas.Editor;
using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fitamas.Graphics.TextureAtlases
{
    /// <summary>
    ///     Defines a texture atlas which stores a source image and contains regions specifying its sub-images.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Texture atlas (also called a tile map, tile engine, or sprite sheet) is a large image containing a collection,
    ///         or "atlas", of sub-images, each of which is a texture map for some part of a 2D or 3D model.
    ///         The sub-textures can be rendered by modifying the texture coordinates of the object's uvmap on the atlas,
    ///         essentially telling it which part of the image its texture is in.
    ///         In an application where many small textures are used frequently, it is often more efficient to store the
    ///         textures in a texture atlas which is treated as a single unit by the graphics hardware.
    ///         This saves memory and because there are less rendering state changes by binding once, it can be faster to bind
    ///         one large texture once than to bind many smaller textures as they are drawn.
    ///         Careful alignment may be needed to avoid bleeding between sub textures when used with mipmapping, and artefacts
    ///         between tiles for texture compression.
    ///     </para>
    /// </remarks>
    public class TextureAtlas
    {
        private string name;
        private Texture2D texture;
        private List<TextureRegion2D> _regionsByIndex;

        public TextureAtlas()
        {

        }

        /// <summary>
        ///     Initializes a new texture atlas with an empty list of regions.
        /// </summary>
        /// <param name="name">The asset name of this texture atlas</param>
        /// <param name="texture">Source <see cref="Texture2D " /> image used to draw on screen.</param>
        public TextureAtlas(string name, Texture2D texture)
        {
            this.name = name;
            this.texture = texture;

            _regionsByIndex = new List<TextureRegion2D>();
        }

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new texture atlas and populates it with regions.
        /// </summary>
        /// <param name="name">The asset name of this texture atlas</param>
        /// <param name="texture">Source <see cref="!:Texture2D " /> image used to draw on screen.</param>
        /// <param name="regions">A collection of regions to populate the atlas with.</param>
        public TextureAtlas(string name, Texture2D texture, Dictionary<string, Rectangle> regions)
            : this(name, texture)
        {
            foreach (var region in regions)
                Create(region.Key, region.Value.X, region.Value.Y, region.Value.Width, region.Value.Height);
        }

        public string Name => name;

        /// <summary>
        ///     Gets a source <see cref="Texture2D" /> image.
        /// </summary>
        public Texture2D Texture => texture;

        /// <summary>
        ///     Gets a list of regions in the <see cref="TextureAtlas" />.
        /// </summary>
        public IEnumerable<TextureRegion2D> Regions => _regionsByIndex;

        /// <summary>
        ///     Gets the number of regions in the <see cref="TextureAtlas" />.
        /// </summary>
        public int Count => _regionsByIndex.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public TextureRegion2D this[string name] => GetRegion(name);
        public TextureRegion2D this[int index] => GetRegion(index);

        /// <summary>
        ///     Gets the enumerator of the <see cref="TextureAtlas" />' list of regions.
        /// </summary>
        /// <returns>The <see cref="IEnumerator" /> of regions.</returns>
        public IEnumerator<TextureRegion2D> GetEnumerator()
        {
            return _regionsByIndex.GetEnumerator();
        }

        public bool Contains(TextureRegion2D texture)
        {
            return _regionsByIndex.Contains(texture);
        }

        /// <summary>
        /// Internal method for adding region
        /// </summary>
        /// <param name="region">Texture region.</param>
        public void Add(TextureRegion2D region)
        {
            _regionsByIndex.Add(region);
        }

        /// <summary>
        ///     Creates a new texture region and adds it to the list of the <see cref="TextureAtlas" />' regions.
        /// </summary>
        /// <param name="name">Name of the texture region.</param>
        /// <param name="x">X coordinate of the region's top left corner.</param>
        /// <param name="y">Y coordinate of the region's top left corner.</param>
        /// <param name="width">Width of the texture region.</param>
        /// <param name="height">Height of the texture region.</param>
        /// <returns>Created texture region.</returns>
        public TextureRegion2D Create(string name, int x, int y, int width, int height)
        {
            //if (_regionsByName.ContainsKey(name))
            //    throw new InvalidOperationException($"Region {name} already exists in the texture atlas");

            var region = new TextureRegion2D(name, texture, x, y, width, height);
            Add(region);
            return region;
        }

        /// <summary>
        ///     Removes a texture region from the <see cref="TextureAtlas" />
        /// </summary>
        /// <param name="index">An index of the <see cref="TextureRegion2D" /> in <see cref="Region" /> to remove</param>
        public void Remove(int index)
        {
            var region = _regionsByIndex[index];
            _regionsByIndex.RemoveAt(index);
        }

        public void Remove(TextureRegion2D texture)
        {
            _regionsByIndex.Remove(texture);
        }

        /// <summary>
        ///     Gets a <see cref="TextureRegion2D" /> from the <see cref="TextureAtlas" />' list.
        /// </summary>
        /// <param name="index">An index of the <see cref="TextureRegion2D" /> in <see cref="Region" /> to get.</param>
        /// <returns>The <see cref="TextureRegion2D" />.</returns>
        public TextureRegion2D GetRegion(int index)
        {
            if (index < 0 || index >= _regionsByIndex.Count)
                throw new IndexOutOfRangeException();

            return _regionsByIndex[index];
        }

        /// <summary>
        ///     Gets a <see cref="TextureRegion2D" /> from the <see cref="TextureAtlas" />' list.
        /// </summary>
        /// <param name="name">Name of the <see cref="TextureRegion2D" /> to get.</param>
        /// <returns>The <see cref="TextureRegion2D" />.</returns>
        public TextureRegion2D GetRegion(string name)
        {
            foreach (var region in _regionsByIndex)
            {
                if (region.Name == name)
                {
                    return region;
                }
            }

            return null;
        }

        /// <summary>
        ///     Creates a new <see cref="TextureAtlas" /> and populates it with a grid of <see cref="TextureRegion2D" />.
        /// </summary>
        /// <param name="name">The name of this texture atlas</param>
        /// <param name="texture">Source <see cref="Texture2D" /> image used to draw on screen</param>
        /// <param name="regionWidth">Width of the <see cref="TextureRegion2D" />.</param>
        /// <param name="regionHeight">Height of the <see cref="TextureRegion2D" />.</param>
        /// <param name="maxRegionCount">The number of <see cref="TextureRegion2D" /> to create.</param>
        /// <param name="margin">Minimum distance of the regions from the border of the source <see cref="Texture2D" /> image.</param>
        /// <param name="spacing">Horizontal and vertical space between regions.</param>
        /// <returns>A created and populated <see cref="TextureAtlas" />.</returns>
        public static TextureAtlas Create(string name, Texture2D texture, int regionWidth, int regionHeight,
            int maxRegionCount = int.MaxValue, int margin = 0, int spacing = 0)
        {
            var textureAtlas = new TextureAtlas(name, texture);
            var count = 0;
            var width = texture.Width - margin;
            var height = texture.Height - margin;
            var xIncrement = regionWidth + spacing;
            var yIncrement = regionHeight + spacing;

            for (var y = margin; y < height; y += yIncrement)
            {
                for (var x = margin; x < width; x += xIncrement)
                {
                    var regionName = $"{texture.Name ?? "region"}{count}";
                    textureAtlas.Create(regionName, x, y, regionWidth, regionHeight);
                    count++;

                    if (count >= maxRegionCount)
                        return textureAtlas;
                }
            }

            return textureAtlas;
        }

        public static TextureAtlas Create(string name, Texture2D texture, Rectangle[] rectangles)
        {
            Dictionary<string, Rectangle> regions = new Dictionary<string, Rectangle>();

            for (var i = 0; i < rectangles.Length; i++)
            {
                string regionName = name + "_" + i;
                regions[regionName] = rectangles[i];
            }

            return new TextureAtlas(name, texture, regions);
        }
    }
}