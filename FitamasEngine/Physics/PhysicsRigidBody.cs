using Fitamas.ECS;
using Fitamas.Math;
using Fitamas.Serialization;
using nkast.Aether.Physics2D.Dynamics;
using System;

namespace Fitamas.Physics
{
    public enum MotionType
    {
        Static,
        Kinematic,
        Dynamic
    }

    public class PhysicsRigidBody : Component
    {
        public MotionType MotionType;
        public bool FixedRotation;

        internal Body Body { get; set; }
    }
}
