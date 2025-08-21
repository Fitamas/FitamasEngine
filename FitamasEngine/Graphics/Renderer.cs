using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fitamas.Graphics
{
    public abstract class Renderer
    {
        public HashSet<string> Tags { get; set; }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public bool Has(string tag)
        {
            return Tags != null ? Tags.Contains(tag) : false;
        }

        public bool HasAny(params string[] tags)
        {
            return Tags != null ? tags.Any(Tags.Contains) : false;
        }
    }
}
