using Fitamas.Graphics;
using Fitamas.Math;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface.Components
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
        Top,
        Left,
        Bottom,
        Right,
    }

    public class GUIImage : GUIComponent
    {
        public static readonly DependencyProperty<Color> ColorProperty = new DependencyProperty<Color>(Color.White);

        public static readonly DependencyProperty<GUIImageType> ImageTypeProperty = new DependencyProperty<GUIImageType>(GUIImageType.Simple);

        public static readonly DependencyProperty<GUIImageEffect> ImageEffectProperty = new DependencyProperty<GUIImageEffect>(GUIImageEffect.None);

        public static readonly DependencyProperty<GUIFillMethod> FillMethodProperty = new DependencyProperty<GUIFillMethod>(GUIFillMethod.Horizontal);

        public static readonly DependencyProperty<GUIFillOrigin> FillOriginProperty = new DependencyProperty<GUIFillOrigin>(GUIFillOrigin.Top);

        public static readonly DependencyProperty<Sprite> SpriteProperty = new DependencyProperty<Sprite>(defaultValue: null);

        public static readonly DependencyProperty<bool> ClockwiseProperty = new DependencyProperty<bool>(false);

        public static readonly DependencyProperty<bool> PreserveAspectProperty = new DependencyProperty<bool>(false);

        public static readonly DependencyProperty<float> FillAmountProperty = new DependencyProperty<float>(0);

        public Color Color
        {
            get 
            { 
                return GetValue(ColorProperty); 
            }
            set 
            { 
                SetValue(ColorProperty, value); 
            }
        }

        public GUIImageType ImageType
        {
            get
            {
                return GetValue(ImageTypeProperty);
            }
            set
            {
                SetValue(ImageTypeProperty, value);
            }
        }

        public GUIImageEffect ImageEffect
        {
            get
            {
                return GetValue(ImageEffectProperty);
            }
            set
            {
                SetValue(ImageEffectProperty, value);
            }
        }

        public GUIFillMethod FillMethod
        {
            get
            {
                return GetValue(FillMethodProperty);
            }
            set
            {
                SetValue(FillMethodProperty, value);
            }
        }

        public GUIFillOrigin FillOrigin
        {
            get
            {
                return GetValue(FillOriginProperty);
            }
            set
            {
                SetValue(FillOriginProperty, value);
            }
        }

        public Sprite Sprite
        {
            get
            {
                return GetValue(SpriteProperty);
            }
            set
            {
                SetValue(SpriteProperty, value);
            }
        }

        public bool Clockwise
        {
            get
            {
                return GetValue(ClockwiseProperty);
            }
            set
            {
                SetValue(ClockwiseProperty, value);
            }
        }

        public bool PreserveAspect
        {
            get
            {
                return GetValue(PreserveAspectProperty);
            }
            set
            {
                SetValue(PreserveAspectProperty, value);
            }
        }

        public float FillAmount
        {
            get
            {
                return GetValue(FillAmountProperty);
            }
            set
            {
                SetValue(FillAmountProperty, value);
            }
        }

        public GUIImage()
        {

        }

        protected override void OnDraw(GameTime gameTime, GUIContextRender context)
        {
            if (!IsVisible)
            {
                base.OnDraw(gameTime, context);
                return;
            }

            Render.Begin(context.Mask);

            if (Sprite != null)
            {
                Rectangle rectangle = GetImageRactengle(Sprite.Bounds, Rectangle);

                switch (ImageType)
                {
                    case GUIImageType.Simple:
                        Render.Draw(Sprite, Color, rectangle, ImageEffect);
                        break;
                    case GUIImageType.Sliced: //TODO
                        break;
                    case GUIImageType.Tiled:  //TODO
                        break;
                    case GUIImageType.Filled:
                        Rectangle mask = GetFilledMask(rectangle);
                        Render.Draw(Sprite, Color, rectangle, ImageEffect);
                        break;
                }
            }
            else
            {
                Render.Draw(Texture2DHelper.DefaultTexture, Rectangle, Color);
            }

            Render.End();

            base.OnDraw(gameTime, context);
        }

        private Rectangle GetImageRactengle(Rectangle source, Rectangle destination)
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

            return new RectangleF(position, destinationSize).ToRectangle();
        }

        private Rectangle GetFilledMask(Rectangle rectangle)
        {
            switch (FillMethod)
            {
                case GUIFillMethod.Horizontal:
                    int width = rectangle.Width;
                    int maskWidth = (int)(width * FillAmount);
                    rectangle.Width = maskWidth;

                    if (FillOrigin == GUIFillOrigin.Right)
                    {
                        rectangle.X += width - maskWidth;
                    }
                    break;
                case GUIFillMethod.Verical:
                    int height = rectangle.Height;
                    int maskHeight = (int)(height * FillAmount);
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
            LocalSize = Sprite.Bounds.Size;
        }
    }
}
