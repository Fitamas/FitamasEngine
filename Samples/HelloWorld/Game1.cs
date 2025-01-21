using Fitamas.Entities;
using Fitamas.Main;
using Fitamas.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Fitamas.Samples.HelloWorld
{
    public class HelloWorldGame : GameEngine
    {
        protected override void Initialize()
        {
            base.Initialize();

            Debug.Log("Hello World");

            GUIDebug.DebugModeOn = true;
        }
    }
}
