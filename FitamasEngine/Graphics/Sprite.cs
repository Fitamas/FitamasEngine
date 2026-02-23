using Fitamas.Core;
using Fitamas.DebugTools;
using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fitamas.Graphics
{
    public struct SpriteFrame
    {
        public Rectangle Bounds;
        public Rectangle Border;
    }

    [AssetType(fileName: "NewSprite", title: "Sprite")]
    public class Sprite : MonoContentObject
    {
        [SerializeField] private Texture2D texture;
        [SerializeField] private List<SpriteFrame> frames;

        public int PixelInUnit = 32;

        public SpriteFrame DefaultFrame
        {
            get
            {
                Init();
                return frames[0];
            }
        }

        public Texture2D Texture => texture;
        public int X => DefaultFrame.Bounds.X;
        public int Y => DefaultFrame.Bounds.Y;
        public int Width => DefaultFrame.Bounds.Width;
        public int Height => DefaultFrame.Bounds.Height;
        public Point Size => DefaultFrame.Bounds.Size;
        public int TextureWidth => texture.Width;
        public int TextureHeight => texture.Height;
        public int FramesCount => frames.Count;
        public IReadOnlyList<SpriteFrame> Frames => frames;

        private Sprite()
        {
            frames = new List<SpriteFrame>();

            Init();
        }

        public Sprite(Texture2D texture, IEnumerable<SpriteFrame> frames = null)
        {
            this.texture = texture;
            this.frames = new List<SpriteFrame>();

            if (frames != null)
            {
                this.frames.AddRange(frames);
            }

            Init();
        }

        private void Init()
        {
            if (frames.Count == 0)
            {
                //TODO FIX

                //frames.Add(new SpriteFrame()
                //{
                //    Bounds = texture != null? texture.Bounds : new Rectangle(),
                //});
            }
        }

        public static Sprite Create(string path, IEnumerable<SpriteFrame> rectangles = null)
        {
            Texture2D texture = GameEngine.Instance.Content.Load<Texture2D>(path);
            return new Sprite(texture, rectangles.ToArray());
        }

        public static Sprite Create(string path, int regionWidth, int regionHeight,
                int maxRegionCount = int.MaxValue, int margin = 0, int spacing = 0)
        {
            Texture2D texture = GameEngine.Instance.Content.Load<Texture2D>(path);
            List<SpriteFrame> frames = new List<SpriteFrame>();
            var count = 0;
            var width = texture.Width - margin;
            var height = texture.Height - margin;
            var xIncrement = regionWidth + spacing;
            var yIncrement = regionHeight + spacing;

            for (var y = margin; y < height; y += yIncrement)
            {
                for (var x = margin; x < width; x += xIncrement)
                {
                    frames.Add(new SpriteFrame()
                    {
                        Bounds = new Rectangle(x, y, regionHeight, regionWidth)
                    });

                    count++;

                    if (count >= maxRegionCount)
                    {
                        return new Sprite(texture, frames.ToArray());
                    }
                }
            }

            return new Sprite(texture, frames.ToArray());
        }
    }
}
