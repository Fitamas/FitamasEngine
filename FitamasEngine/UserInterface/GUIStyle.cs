using Fitamas.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Fitamas.UserInterface
{
    public static class GUIStyle
    {
        public static void Init(Game game)
        {
            DefoultTexture = new Texture2D(game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            DefoultTexture.SetData(new[] { Color.White });

            DefoultFont = game.Content.Load<BitmapFont>("Font\\DefoultFont");
        }

        public static Texture2D DefoultTexture { get; set; }

        public static BitmapFont DefoultFont { get; set; }

        public static Color TextColor = Color.Black;

        public static Color BackgroundTitle = Color.White;

        public static Point DefoultCheckBoxSize = new Point(40, 40);

        public static Point ContextItemSize = new Point(150, 30);
    }
}
