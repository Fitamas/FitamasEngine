using Fitamas.DebugTools;
using Fitamas.Serializeble;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;

namespace Fitamas.Graphics
{
    public enum SpriteMode
    {
        Defoult,
        Multiple
    }

    [AssetMenu(fileName: "NewSprite.spr", title: "Sprite")]
    public class Sprite : MonoObject
    {
        [SerializableField] private Texture2D texture;
        [SerializableField] private Rectangle[] regions;
        [SerializableField] private SpriteMode spriteMode;

        private List<TextureRegion2D> multipleRegions;

        public int PixelInUnit = 32;

        public SpriteMode SpriteMode
        {
            get
            {
                return spriteMode;
            }
            set
            {
                spriteMode = value;
            }
        }

        public Texture2D Texture2D => texture;
        public int TextureWidth => texture.Width;
        public int TextureHeight => texture.Height;

        public Sprite()
        {

        }

        public Sprite(Texture2D texture, int countWidth, int countHeight)
        {
            this.texture = texture;

            CreateRegions(countWidth, countHeight);

            SpriteMode = SpriteMode.Multiple;
        }

        public Sprite(Texture2D texture, Rectangle[] regions)
        {
            this.texture = texture;
            this.regions = regions;

            CreateRegions(regions);

            SpriteMode = SpriteMode.Multiple;
        }

        public TextureRegion2D GetRegion(int index)
        {
            if (texture == null)
            {
                return null;
            }

            TextureRegion2D region = null;

            switch (SpriteMode)
            {
                case SpriteMode.Defoult:
                    if (multipleRegions == null || multipleRegions.Count == 0)
                    {
                        CreateRegions(TextureWidth, TextureHeight);
                    }

                    region = multipleRegions[0];
                    break;
                case SpriteMode.Multiple:
                    if (multipleRegions == null)
                    {
                        CreateRegions(regions);
                    }

                    if (index < 0 || index >= multipleRegions.Count)
                        throw new IndexOutOfRangeException();

                    region = multipleRegions[index];
                    break;
            }

            return region;
        }

        public void Add(TextureRegion2D region)
        {
            multipleRegions.Add(region);
        }

        public TextureRegion2D Create(int x, int y, int width, int height)
        {
            var region = new TextureRegion2D(texture.Name, texture, x, y, width, height);
            Add(region);
            return region;
        }

        private void CreateRegions(Rectangle[] regions)
        {
            multipleRegions = new List<TextureRegion2D>();
            foreach (var region in regions)
            {
                Create(region.X, region.Y, region.Width, region.Height);
            }
        }

        public void CreateRegions(int regionWidth, int regionHeight,
                int maxRegionCount = int.MaxValue, int margin = 0, int spacing = 0)
        {
            multipleRegions = new List<TextureRegion2D>();
            var count = 0;
            var width = texture.Width - margin;
            var height = texture.Height - margin;
            var xIncrement = regionWidth + spacing;
            var yIncrement = regionHeight + spacing;

            for (var y = margin; y < height; y += yIncrement)
            {
                for (var x = margin; x < width; x += xIncrement)
                {
                    Create(x, y, regionWidth, regionHeight);
                    count++;

                    if (count >= maxRegionCount)
                        return;
                }
            }
        }
    }
}
