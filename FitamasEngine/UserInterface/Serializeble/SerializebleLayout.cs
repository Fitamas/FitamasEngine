using Fitamas.Container;
using Fitamas.Serializeble;
using Fitamas.UserInterface.Scripting;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface.Serializeble
{
    public class SerializebleLayout : MonoObject
    {
        [SerializableField] protected GUICanvas canvas = new GUICanvas();

        public GUIScripting Scripting { get; }

        public GUICanvas Canvas => canvas;

        public SerializebleLayout(GUICanvas canvas, GUIScripting scripting)
        {
            this.canvas = canvas;
            Scripting = scripting;
        }

        public void OpenScreen(GUISystem system)
        {
            canvas.Init(system);

            canvas.Enable = true;

            Scripting?.OnOpen(system.Container);
        }

        public void CloseScreen()
        {
            canvas.Enable = false;

            Scripting?.OnClose();
        }
    }
}
