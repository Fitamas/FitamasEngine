using Fitamas.ECS;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Animation.Rigging
{
    public class RiggingSystem : EntityUpdateSystem
    {
        public RiggingSystem() : base(Aspect.All(typeof(RigController)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {

        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
