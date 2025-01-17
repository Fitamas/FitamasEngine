using Fitamas.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface
{
    public class GUICanvas : GUIComponent
    {
        public GUICanvas() : base(new Rectangle()) 
        {
            Interecteble = false;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (Camera.Main != null)
            {
                Vector2 size = Camera.Main.VirtualSize;
                LocalScale = size.ToPoint();
            }
        }
    }
}
