using Fitamas.Graphics;
using Fitamas.UserInterface;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface.Components
{
    public class GUICanvas : GUIComponent
    {
        public GUICanvas() : base()
        {
            Interecteble = false;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            LocalScale = Render.GetViewportSize();
        }
    }
}
