using Fitamas.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Fitamas.UserInterface
{
    public class Texture2DHelper
    {
        private static Texture2D defaultTexture;

        public static Texture2D DefaultTexture
        {
            get
            {
                if (defaultTexture == null)
                {
                    defaultTexture = new Texture2D(GameEngine.Instance.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                    defaultTexture.SetData(new[] { Color.White });
                }
                return defaultTexture;
            }
        }
    }
}
