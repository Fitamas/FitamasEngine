using Fitamas.Graphics;
using Fitamas.Math2D;
using Fitamas.Serializeble;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;
using System;

namespace Fitamas.UserInterface
{
    public enum GUIImageType
    {
        Simple,
        Sliced,
        Tiled,
        Filled
    }

    [Flags]
    public enum GUIImageEffect
    {
        None = 0,
        FlipHorizontally = 1,
        FlipVertically = 2
    }

    public enum GUIFillMethod
    {
        Horizontal,
        Verical,
        Radial90,
        Radial180,
        Radial360
    }

    public enum GUIFillOrigin
    {
        Bottom,
        Right,
        Top,
        Left,
    }

    public class GUIImage : GUIComponent
    {
        [SerializableField] private float fillAmount;
        [SerializableField] private int selectRegion;

        public GUIImageType ImageType;
        public GUIImageEffect ImageEffect;
        public GUIFillMethod FillMethod;
        public GUIFillOrigin FillOrigin;
        public Sprite Sprite;
        public Color Color = Color.White;
        public bool Clockwise;
        public bool PreserveAspect;

        public float FillAmount
        {
            get
            {
                return fillAmount;
            }
            set
            {
                fillAmount = MathV.Clamp01(value);
            }
        }

        public GUIImage(Rectangle rectangle = new Rectangle()) : base(rectangle)
        {
            selectRegion = 0;
            ImageType = GUIImageType.Simple;
            ImageEffect = GUIImageEffect.None;
        }

        protected override void OnDraw(GameTime gameTime)
        {
            if (Sprite != null)
            {
                TextureRegion2D textureRegion = Sprite.GetRegion(selectRegion);
                RectangleF rectangle = GetImageRactengle(textureRegion.Bounds, Rectangle);

                switch (ImageType)
                {
                    case GUIImageType.Simple:
                        GUIRender.Draw(textureRegion, Color, rectangle, ImageEffect, Layer);
                        break;
                    case GUIImageType.Sliced: //TODO
                        break;
                    case GUIImageType.Tiled:  //TODO
                        break;
                    case GUIImageType.Filled:
                        Rectangle mask = GetFilledMask(rectangle.ToRectangle());
                        GUIRender.Draw(textureRegion, Color, rectangle, ImageEffect, Layer, mask);
                        break;
                }
            }
            else
            {
                GUIRender.Draw(GUIStyle.DefoultTexture, Rectangle, Color, Layer);
            }
        }

        private RectangleF GetImageRactengle(Rectangle source, Rectangle destination)
        {
            Vector2 position = destination.Location.ToVector2();
            Vector2 destinationSize = destination.Size.ToVector2();
            Vector2 sourceSize = source.Size.ToVector2();

            if (PreserveAspect)
            {
                float x = destinationSize.X / sourceSize.X;
                float y = destinationSize.Y / sourceSize.Y;
                Vector2 center = position + destinationSize / 2;

                if (x < y)
                {
                    destinationSize = sourceSize * x;
                }
                else
                {
                    destinationSize = sourceSize * y;
                }

                position = center - destinationSize / 2;
            }

            return new RectangleF(position, destinationSize);
        }

        private Rectangle GetFilledMask(Rectangle rectangle)
        {
            switch (FillMethod)
            {
                case GUIFillMethod.Horizontal:
                    int width = rectangle.Width;
                    int maskWidth = (int)(width * fillAmount);
                    rectangle.Width = maskWidth;

                    if (FillOrigin == GUIFillOrigin.Right)
                    {
                        rectangle.X += width - maskWidth;
                    }
                    break;
                case GUIFillMethod.Verical:
                    int height = rectangle.Height;
                    int maskHeight = (int)(height * fillAmount);
                    rectangle.Height = maskHeight;

                    if (FillOrigin == GUIFillOrigin.Bottom)
                    {
                        rectangle.Y += height - maskHeight;
                    }
                    break;
                case GUIFillMethod.Radial90:  //TODO
                    break;
                case GUIFillMethod.Radial180: //TODO
                    break;
                case GUIFillMethod.Radial360: //TODO
                    break;
            }

            return rectangle;
        }

        public void SetNativeSize()
        {
            LocalScale = Sprite.GetRegion(selectRegion).Bounds.Size;
        }
    }
}
