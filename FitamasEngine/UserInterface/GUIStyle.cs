using Fitamas.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fitamas.UserInterface
{
    public class GUIStyle
    {
        private static Texture2D defoultTexture;
        private static SpriteFont defoultFont;

        public static Texture2D DefoultTexture 
        {
            get
            {
                if (defoultTexture == null)
                {
                    defoultTexture = new Texture2D(GameEngine.Instance.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                    defoultTexture.SetData(new[] { Color.White });
                }
                return defoultTexture;
            }
        }

        public static SpriteFont DefoultFont
        {
            get
            {
                if (defoultFont == null)
                {
                    defoultFont = GameEngine.Instance.Content.Load<SpriteFont>("Font\\Pixel_20");
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
