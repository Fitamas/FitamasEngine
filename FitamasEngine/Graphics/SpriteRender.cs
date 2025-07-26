using System;
using System.Linq;
using Fitamas.ECS;
using Fitamas.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fitamas.Graphics
{
    public class SpriteRender : Component
    {
        public Effect Effect;
        public Sprite Sprite;
        public float Alpha;
        public Color Color;
        public Vector2 SpriteOffset;
        public int Layer;
        public SpriteEffects SpriteEffect;
        public int RectangleIndex;

        public Vector2 Origin
        { 
            get
            {
                return new Vector2(0.5f * Sprite.Width, 0.5f * Sprite.Height);
            }
        }

        public Vector2 RenderSize
        {
            get
            {
                return new Vector2((float)Sprite.Width / Sprite.PixelInUnit, (float)Sprite.Height / Sprite.PixelInUnit);
            }
        }

        public SpriteRender()
        {
            Alpha = 1.0f;
            Color = Color.White;
            RectangleIndex = -1;
        }
    }
}
