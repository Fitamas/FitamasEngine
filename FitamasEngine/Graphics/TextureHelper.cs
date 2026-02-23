using Fitamas.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Fitamas.Graphics
{
    public class TextureHelper
    {
        public static readonly HashSet<string> Extensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".png",
            ".jpg",
            ".gif",
            ".bmp",
            ".tif",
            ".dds",
        };

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
