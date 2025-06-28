using Fitamas.ECS;
using System;

namespace Fitamas.Physics
{
    public class PhysicsContactEvent
    {
        public Entity Entity { get; set; }
        public PhysicsCollider Collider { get; set; }
    }
}
