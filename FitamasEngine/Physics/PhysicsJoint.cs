using Fitamas.Entities;
using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Physics
{
    public enum PhysicsJointType
    {
        Distance,
        Rope,
        Weld,
        Revolute,
        Wheel
    }

    public class PhysicsJoint : Component
    {
        [SerializeField] private PhysicsJointType jointType = PhysicsJointType.Distance;
        [SerializeField] private Entity entityA;
        [SerializeField] private Vector2 anchorA;
        [SerializeField] private Entity entityB;
        [SerializeField] private Vector2 anchorB;
        [SerializeField] private bool useWorldCoordinates;
        [SerializeField] private bool collideConnected = false;
        [SerializeField] private float motorFrequency = 8;
        [SerializeField] private float distance;

        public PhysicsJointType JointType => jointType;
        public Entity EntityA => entityA;
        public Vector2 AnchorA => anchorA;
        public Entity EntityB => entityB;
        public Vector2 AnchorB => anchorB;
        public bool UseWorldCoordinates => useWorldCoordinates;
        public bool CollideConnected => collideConnected;
        public float MotorFrequency => motorFrequency;
        public float Distance => distance;

        public static PhysicsJoint CreateRevolt(Entity entityA, Vector2 anchorA, Entity entityB, Vector2 anchorB, bool useWorldCoordinates = false)
        {
            PhysicsJoint joint = new PhysicsJoint();
            joint.jointType = PhysicsJointType.Revolute;
            joint.entityA = entityA;
            joint.anchorA = anchorA;
            joint.entityB = entityB;
            joint.anchorB = anchorB;
            joint.useWorldCoordinates = useWorldCoordinates;
            return joint;
        }

        public static PhysicsJoint CreateWheel(Entity entityA, Vector2 anchorA, Entity entityB, Vector2 anchorB, bool useWorldCoordinates = false)
        {
            PhysicsJoint joint = new PhysicsJoint();
            joint.jointType = PhysicsJointType.Wheel;
            joint.entityA = entityA;
            joint.anchorA = anchorA;
            joint.entityB = entityB;
            joint.anchorB = anchorB;
            joint.useWorldCoordinates = useWorldCoordinates;
            return joint;
        }

        public static PhysicsJoint CreateRope(Entity entityA, Vector2 anchorA, Entity entityB, Vector2 anchorB, float distance, bool useWorldCoordinates = false)
        {
            PhysicsJoint joint = new PhysicsJoint();
            joint.jointType = PhysicsJointType.Rope;
            joint.entityA = entityA;
            joint.anchorA = anchorA;
            joint.entityB = entityB;
            joint.anchorB = anchorB;
            joint.useWorldCoordinates = useWorldCoordinates;
            joint.distance = distance;
            return joint;
        }
    }
}
