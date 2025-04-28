using System;
using System.Linq;
using Fitamas.Entities;
using Fitamas.Math2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fitamas.Graphics
{
    public class SpriteRender : Component
    {
        public Sprite sprite;
        public int selectRegion = 0;
        public float alpha;
        public Color color;
        public bool isVisible;
        public Vector2 spriteOffset;
        public int spriteLayer = 0;
        public bool flipHorizontally = false;
        public bool flipVertically = false;

        public SpriteEffects SpriteEffect
        {
            get
            {
                SpriteEffects sprite = SpriteEffects.None;

                if (flipHorizontally)
                {
                    sprite |= SpriteEffects.FlipHorizontally;
                }
                if (flipVertically)
                {
                    sprite |= SpriteEffects.FlipVertically;
                }

                return sprite;
            }
        }

        public Vector2 Origin
        { 
            get
            {
                return new Vector2(0.5f * sprite.Width, 0.5f * sprite.Height);
            }
        }
       
        public SpriteRender()
        {
            alpha = 1.0f;
            color = Color.White;
            isVisible = true;
        }

        public SpriteRender(Sprite texture, int selectRegion = 0, int layer = 0, Vector2 offset = new Vector2())
        {
            this.spriteOffset = offset;
            this.sprite = texture;
            this.selectRegion = selectRegion;
            spriteLayer = layer;
            alpha = 1.0f;
            color = Color.White;
            isVisible = true;
        }

        public Vector2[] GetAbsoluteSpriteRectangle(Transform transform)
        {
            RectangleF rectangle = GetSpriteRectangle();

            Vector2[] points = rectangle.GetCorners();

            for (int i = 0; i < points.Length; i++)
            {
                points[i] = transform.ToAbsolutePosition(points[i]);
            }

            return points;
        }

        public RectangleF GetSpriteRectangle()
        {
            if (sprite == null)
            {
                return new RectangleF();
            }

            float pixelInUnit = sprite.PixelInUnit;
            Vector2 min = -Origin / pixelInUnit;
            Vector2 max = min + new Vector2(sprite.Width / pixelInUnit, sprite.Height / pixelInUnit);

            var corners = new Vector2[4];
            corners[0] = min;
            corners[1] = new Vector2(max.X, min.Y);
            corners[2] = max;
            corners[3] = new Vector2(min.X, max.Y);

            min = new Vector2(corners.Min(i => i.X), corners.Min(i => i.Y));
            max = new Vector2(corners.Max(i => i.X), corners.Max(i => i.Y));
            return new RectangleF(min.X, min.Y, max.X - min.X, max.Y - min.Y);
        }
    }
}
