using System;
using System.Collections.Generic;
using Fitamas.Collections;
using Fitamas.Core;
using Fitamas.DebugTools;
using Fitamas.ECS;
using Fitamas.Graphics.PostProcessors;
using Fitamas.Graphics.RendererFeatures;
using Fitamas.Graphics.ViewportAdapters;
using Fitamas.Math;
using Fitamas.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
