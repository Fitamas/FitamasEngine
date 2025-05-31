using Fitamas.Math2D;
using Fitamas.Serialization;
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
    }
}
