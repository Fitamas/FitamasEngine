using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitamas.ECS;
using Microsoft.Xna.Framework;

namespace Fitamas.Graphics
{
    public abstract class EntityDrawSystem : EntitySystem, IDrawSystem
    {
        protected EntityDrawSystem(AspectBuilder aspect) : base(aspect)
        {
        }

        public abstract void Draw(GameTime gameTime);
    }
}
