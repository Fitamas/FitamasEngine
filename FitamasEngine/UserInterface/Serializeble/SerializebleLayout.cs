using Fitamas.Serializeble;
using Fitamas.UserInterface.Scripting;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface.Serializeble
{
    public class SerializebleLayout : MonoObject
    {
        [SerializableField] protected GUICanvas canvas = new GUICanvas();

        private GUISystem system;
        public GUIScripting Scripting { get; set; }

        public GUICanvas Canvas => canvas;

        public SerializebleLayout(GUICanvas canvas)
        {
            this.canvas = canvas;
        }

        public void Update(GameTime gameTime)
        {
            Scripting.Update();

            canvas.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            system.GUIRender.Begin();

            canvas.Draw(gameTime);

            system.GUIRender.End();

            if (GUIDebug.DebugModeOn)
            {
                GUIDebug.Render(canvas);
            }
        }

        public void OpenScreen(GUISystem system)
        {
            this.system = system;

            canvas.Init(system);

            canvas.Enable = true;

            Scripting?.OnOpen(system);
        }

        public void CloseScreen()
        {
            canvas.Enable = false;

            Scripting?.OnClose();
        }
    }
}
