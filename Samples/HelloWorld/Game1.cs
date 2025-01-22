using Fitamas.Entities;
using Fitamas.Main;
using Fitamas.UserInterface;
using Microsoft.Xna.Framework;

namespace Fitamas.Samples.HelloWorld
{
    public class HelloWorldGame : GameEngine
    {
        protected override void Initialize()
        {
            base.Initialize();

            Debug.Log("Hello World");
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            GUIDebug.Active = true;

            GUISystem system = Container.Resolve<GUISystem>("gui_system");

            GUIButton button = GUI.CreateButton(new Rectangle(0, -100, 400, 80), "Button from C# script");
            button.Alignment = GUIAlignment.Center;

            system.AddComponent(button);
        }
    }
}
