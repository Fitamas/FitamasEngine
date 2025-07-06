using Fitamas.Core;
using Fitamas.DebugTools;
using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fitamas.Graphics
{
    public enum SpriteMode
    {
        Default,
        Multiple
    }

    [AssetMenu(fileName: "NewSprite.sprite", title: "Sprite")]
    public class Sprite : MonoContentObject
    {
        [SerializeField] private Texture2D texture;

        public int PixelInUnit = 32;
        public Rectangle Bounds;
        public Rectangle Border;
        public Rectangle[] Rectangles;

        public Texture2D Texture => texture;
        public int X => Bounds.X;
        public int Y => Bounds.Y;
        public int Width => Bounds.Width;
        public int Height => Bounds.Height;
        public Point Size => Bounds.Size;
        public int TextureWidth => texture.Width;
        public int TextureHeight => texture.Height;

        private Sprite()
        {

        }

        public Sprite(string name, Texture2D texture, Rectangle bounds, Rectangle[] rectangles) : base(name)
        {
            this.texture = texture;
            Bounds = bounds;
            Rectangles = rectangles;
        }

        public Sprite(Texture2D texture, Rectangle bounds) : this(texture.Name, texture, bounds, [])
        {
            this.texture = texture;
            Bounds = bounds;
        }

        public Sprite(Texture2D texture, Rectangle[] rectangles) : this(texture)
        {
            this.texture = texture;
            Rectangles = rectangles;
        }

        public Sprite(Texture2D texture) : this(texture.Name, texture, texture.Bounds, [])
        {
            this.texture = texture;
        }

        public static Sprite Create(string path)
        {
            Texture2D texture = GameEngine.Instance.Content.Load<Texture2D>(path);
            return new Sprite(texture);
        }

        public static Sprite Create(string path, IEnumerable<Rectangle> rectangles)
        {
            Texture2D texture = GameEngine.Instance.Content.Load<Texture2D>(path);
            return new Sprite(texture, rectangles.ToArray());
        }

        public static Sprite Create(string path, int regionWidth, int regionHeight,
                int maxRegionCount = int.MaxValue, int margin = 0, int spacing = 0)
        {
            Texture2D texture = GameEngine.Instance.Content.Load<Texture2D>(path);
            List<Rectangle> rectangles = new List<Rectangle>();
            var count = 0;
            var width = texture.Width - margin;
            var height = texture.Height - margin;
            var xIncrement = regionWidth + spacing;
            var yIncrement = regionHeight + spacing;

            for (var y = margin; y < height; y += yIncrement)
            {
                for (var x = margin; x < width; x += xIncrement)
                {
                    rectangles.Add(new Rectangle(x, y, regionHeight, regionWidth));
                    count++;

                    if (count >= maxRegionCount)
                    {
                        return new Sprite(texture, rectangles.ToArray());
                    }
                }
            }

            return new Sprite(texture, rectangles.ToArray());
        }
    }
}
