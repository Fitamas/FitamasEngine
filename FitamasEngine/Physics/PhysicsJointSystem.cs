using Fitamas.Collections;
using Fitamas.DebugTools;
using Fitamas.ECS;
using Fitamas.Extended.Entities;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Joints;
using System;

namespace Fitamas.Physics
{
    public class PhysicsJointSystem : EntitySystem, IDrawGizmosSystem
    {
        private PhysicsWorld physicsWorld;

        private Bag<Joint> joints;
        private ComponentMapper<PhysicsJoint> jointMapper;

        public PhysicsJointSystem(PhysicsWorld physicsWorld) : base(Aspect.All(typeof(PhysicsJoint)))
        {
            this.physicsWorld = physicsWorld;

            joints = new Bag<Joint>();

            physicsWorld.World.JointRemoved += (s, j) =>
            {
                int index = joints.IndexOf(j);

                if (index != -1)
                {
                    joints[index] = null;
                    jointMapper.Delete(index);
                }
            };
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            jointMapper = mapperService.GetMapper<PhysicsJoint>();

            jointMapper.OnPut += PutJoint;
            jointMapper.OnDelete += DeleteJoint;
        }

        private void PutJoint(int entityId)
        {
            PhysicsJoint physicsJoint = jointMapper.Get(entityId);
            Body bodyA = physicsWorld.GetBody(physicsJoint.EntityA.Id);
            Body bodyB = physicsWorld.GetBody(physicsJoint.EntityB.Id);
            Vector2 anchorA = physicsJoint.AnchorA;
            Vector2 anchorB = physicsJoint.AnchorB;
            bool useWorldCoordinates = physicsJoint.UseWorldCoordinates;
            Joint joint = null;

            switch (physicsJoint.JointType)
            {
                case PhysicsJointType.Distance:
                    joint = JointFactory.CreateDistanceJoint(physicsWorld.World, bodyA, bodyB, anchorA, anchorB, useWorldCoordinates);
                    break;
                case PhysicsJointType.Rope:
                    RopeJoint ropeJoint = JointFactory.CreateRopeJoint(physicsWorld.World, bodyA, bodyB, anchorA, anchorB, useWorldCoordinates);
                    ropeJoint.MaxLength = physicsJoint.Distance;
                    joint = ropeJoint;
                    break;
                case PhysicsJointType.Weld:
                    WeldJoint weldJoint = JointFactory.CreateWeldJoint(physicsWorld.World, bodyA, bodyB, anchorA, anchorB, useWorldCoordinates);
                    joint = weldJoint;
                    break;
                case PhysicsJointType.Revolute:
                    RevoluteJoint revoluteJoint = JointFactory.CreateRevoluteJoint(physicsWorld.World, bodyA, bodyB, anchorA, anchorB, useWorldCoordinates);
                    joint = revoluteJoint;
                    break;
                case PhysicsJointType.Wheel:
                    WheelJoint wheelJoint = JointFactory.CreateWheelJoint(physicsWorld.World, bodyA, bodyB, anchorA, anchorB, useWorldCoordinates);
                    wheelJoint.Frequency = physicsJoint.MotorFrequency;
                    joint = wheelJoint;
                    break;
            }

            joint.CollideConnected = physicsJoint.CollideConnected;
            joints[entityId] = joint;

            physicsJoint.Joint = joint;
        }

        private void DeleteJoint(int entityId)
        {
            if (jointMapper.TryGet(entityId, out PhysicsJoint joint))
            {
                physicsWorld.World.Remove(joint.Joint);
            }
        }

        public void DrawGizmos()
        {
            foreach (var entityId in ActiveEntities)
            {
                Joint joint = jointMapper.Get(entityId).Joint;
                Gizmos.DrawLine(joint.WorldAnchorA, joint.WorldAnchorB, Color.Green);
            }
        }

        internal Joint GetJoint(int entityId)
        {
            return joints[entityId];
        }

        internal Entity GetEntity(Joint joint)
        {
            int entityId = joints.IndexOf(joint);
            return GetEntity(entityId);
        }
    }
}
