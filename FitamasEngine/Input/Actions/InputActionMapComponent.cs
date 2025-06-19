using Fitamas.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Fitamas.Input.Actions
{
    public class InputActionMapComponent : GameComponent
    {
        public List<InputActionMap> ActionMaps { get; }

        public InputActionMapComponent(Game game) : base(game)
        {
            ActionMaps = new List<InputActionMap>();
        }

        public override void Initialize()
        {

        }

        public override void Update(GameTime gameTime)
        {
            foreach (var map in ActionMaps)
            {
                map.Update(gameTime);
            }
        }
    }
}
