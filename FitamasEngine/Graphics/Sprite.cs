using Fitamas.Core;
using Fitamas.DebugTools;
using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Fitamas.Graphics
{
    public enum SpriteMode
    {
        Default,
        Multiple
    }

    [AssetMenu(fileName: "NewSprite.spr", title: "Sprite")]
    public class Sprite : MonoObject
    {
        [SerializableField] private string name;
        [SerializableField] private Texture2D texture;
        [SerializableField] private Rectangle bounds;
        [SerializableField] private Rectangle border;

        public int PixelInUnit = 32;

        public string Name => name;
        public Texture2D Texture => texture;
        public int X => bounds.X;
        public int Y => bounds.Y;
        public int Width => bounds.Width;
        public int Height => bounds.Height;
        public Point Size => bounds.Size;
        public Rectangle Bounds => bounds;
        public Rectangle Border => border;
        public int TextureWidth => texture.Width;
        public int TextureHeight => texture.Height;

        public Sprite(string name, Texture2D texture, Rectangle bounds)
        {
            this.name = name;
            this.texture = texture;
            this.bounds = bounds;
        }

        public Sprite(Texture2D texture, Rectangle bounds) : this(texture.Name, texture, bounds)
        {
            this.texture = texture;
            this.bounds = bounds;
        }

        public Sprite(Texture2D texture) : this(texture.Name, texture, texture.Bounds)
        {
            this.texture = texture;
        }

        public static Sprite Create(string path)
        {
            Texture2D texture = GameEngine.Instance.Content.Load<Texture2D>(path);
            return new Sprite(texture);
        }

        //private void CreateRegions(Rectangle[] regions)
        //{
        //    multipleRegions = new List<TextureRegion2D>();
        //    foreach (var region in regions)
        //    {
        //        Create(region.X, region.Y, region.Width, region.Height);
        //    }
        //}

        //public void CreateRegions(int regionWidth, int regionHeight,
        //        int maxRegionCount = int.MaxValue, int margin = 0, int spacing = 0)
        //{
        //    multipleRegions = new List<TextureRegion2D>();
        //    var count = 0;
        //    var width = texture.Width - margin;
        //    var height = texture.Height - margin;
        //    var xIncrement = regionWidth + spacing;
        //    var yIncrement = regionHeight + spacing;

        //    for (var y = margin; y < height; y += yIncrement)
        //    {
        //        for (var x = margin; x < width; x += xIncrement)
        //        {
        //            Create(x, y, regionWidth, regionHeight);
        //            count++;

        //            if (count >= maxRegionCount)
        //                return;
        //        }
        //    }
        //}
    }
}
