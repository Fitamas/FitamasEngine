using Fitamas.Math2D;
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

    public class PhysicsRigidBody
    {
        public MotionType MotionType;
        public bool FixedRotation;

        internal Body Body { get; set; }
    }
}
