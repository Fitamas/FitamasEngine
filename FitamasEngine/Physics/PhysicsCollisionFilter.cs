using Fitamas.ECS;
using Fitamas.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitamas.Physics
{
    public class PhysicsCollisionFilter : Component
    {
        public CollisionFilter Filter = CollisionFilter.DefaultFilter;
    }
}
