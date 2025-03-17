using Fitamas.Core;
using Fitamas.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WDL.Gameplay.DigitalLogic;

namespace WDL
{
    public class WDLGame : GameEngine
    {
        public override WorldBuilder CreateWorldBuilder()
        {
            return base.CreateWorldBuilder().AddSystem(new LogicSystem());
        }
    }
}
