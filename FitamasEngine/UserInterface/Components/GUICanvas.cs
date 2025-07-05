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

        }

        protected override void OnDraw(GameTime gameTime, GUIContextRender context)
        {
            LocalSize = Render.GetViewportSize();

            base.OnDraw(gameTime, context);
        }
    }
}
