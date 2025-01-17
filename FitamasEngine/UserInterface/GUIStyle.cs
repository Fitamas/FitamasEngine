using Fitamas.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Fitamas.UserInterface
{
    public static class GUIStyle
    {
        private static Texture2D defoultTexture;
        private static BitmapFont defoultFont;

        public static Texture2D DefoultTexture 
        {
            get
            {
                if (defoultTexture == null)
                {
                    defoultTexture = new Texture2D(GameMain.Instance.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                    defoultTexture.SetData(new[] { Color.White });
                }
                return defoultTexture;
            }
        }

        public static BitmapFont DefoultFont 
        {
            get
            {
                if (defoultFont == null)
                {
                    defoultFont = GameMain.Instance.Content.Load<BitmapFont>("Font\\DefoultFont");
                }
                return defoultFont;
            }
        }

        public static Color TextColor = Color.Black;

        public static Color BackgroundTitle = Color.White;

        public static Point DefoultCheckBoxSize = new Point(40, 40);

        public static Point ContextItemSize = new Point(150, 30);
    }
}
