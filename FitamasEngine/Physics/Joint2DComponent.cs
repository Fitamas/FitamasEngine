using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Joints;
using System;
using System.Collections.Generic;

namespace Fitamas.Physics
{
    public enum Joint2DType
    {
        Distance,
        Rope,
        Weld,
        Revolute,
        Wheel
    }

    public class Joint2D
    {
        [SerializableField] private Joint2DType jointType = Joint2DType.Distance;
        [SerializableField] private Vector2 anchorA;
        [SerializableField] private Vector2 anchorB;
        [SerializableField] private bool collideConnected = false;
        [SerializableField] private float motorFrequency = 8;

        [SerializableField] private Collider colliderA;
        [SerializableField] private Collider colliderB;

        private World world;
        private Joint joint;

        public float maxRopeLenght;




        public Joint Joint => joint;
        public Collider ColliderA => colliderA;
        public Collider ColliderB => colliderB;

        public bool CollideConnected
        {
            get
            {
                return collideConnected;
            }
            set
            {
                collideConnected = value;
                if (IsReady)
                {
                    joint.CollideConnected = collideConnected;
                }
            }
        }

        public bool IsReady
        {
            get
            {
                return world != null && joint != null;
            }
        }

        public Joint2D(Joint2DType jointType, Collider colliderB, Vector2 anchorA, Vector2 anchorB)
        {
            this.jointType = jointType;
            this.colliderB = colliderB;
            this.anchorA = anchorA;
            this.anchorB = anchorB;
        }

        public void CreateJoint(World world, Collider colliderA)
        {
            this.colliderA = colliderA;
            this.world = world;
            Vector2 anchorA = this.anchorA - colliderA.Offset;
            Vector2 anchorB = this.anchorB - colliderB.Offset;

            if (colliderA == null || colliderB == null || joint != null)
            {
                throw new Exception("Joint2D: Unable to create a joint");
            }

            if (world != null)
            {
                if (!colliderA.IsReady)
                {
                    colliderA.CreateBody(world);
                }

                if (!colliderB.IsReady)
                {
                    colliderB.CreateBody(world);
                }

                Body bodyA = colliderA.Body;
                Body bodyB = colliderB.Body;  

                switch (jointType)
                {
                    case Joint2DType.Distance:
                        joint = JointFactory.CreateDistanceJoint(world, bodyA, bodyB, anchorA, anchorB);
                        break;
                    case Joint2DType.Rope:
                        RopeJoint ropeJoint = JointFactory.CreateRopeJoint(world, bodyA, bodyB, anchorA, anchorB);
                        ropeJoint.MaxLength = maxRopeLenght;
                        joint = ropeJoint;
                        break;
                    case Joint2DType.Weld:
                        WeldJoint weldJoint = JointFactory.CreateWeldJoint(world, bodyA, bodyB, anchorA, anchorB);
                        joint = weldJoint;
                        break;
                    case Joint2DType.Revolute:
                        RevoluteJoint revoluteJoint = JointFactory.CreateRevoluteJoint(world, bodyA, bodyB, anchorA, anchorB);
                        joint = revoluteJoint;
                        break;
                    case Joint2DType.Wheel:
                        WheelJoint wheelJoint = JointFactory.CreateWheelJoint(world, bodyA, bodyB, anchorA, anchorB);
                        wheelJoint.Frequency = motorFrequency;
                        joint = wheelJoint;
                        break;
                }

                joint.CollideConnected = collideConnected;
                joint.Broke += (j, v) =>
                {

                };
            }
        }

        public void RemoveJoint()
        {
            if (IsReady)
            {
                if (world.JointList.Contains(joint))
                {
                    world.Remove(joint);
                }
                joint = null;
                world = null;
            }
        }
    }

    public class Joint2DComponent
    {
        [SerializableField] private List<Joint2D> joints = new List<Joint2D>();

        private World world;
        private Collider collider;

        public List<Joint2D> Joints => joints;

        public bool IsReady //TODO
        {
            get
            {
                return world != null && collider != null;
            }
        }

        public void AddJoint(Joint2D joint)
        {
            joints.Add(joint);
            if (IsReady)
            {
                joint.CreateJoint(world, collider);
            }
        }

        public void RemoveJoint(Joint2D joint) 
        { 
            if (joints.Contains(joint))
            {
                joint.RemoveJoint();
                joints.Remove(joint);
            }
        }

        public void CreateJoints(World _world, Collider colliderA)
        {
            world = _world;
            collider = colliderA;

            foreach (Joint2D joint in joints) 
            { 
                if (!joint.IsReady)
                {
                    joint.CreateJoint(world, collider);
                }
            }
        }

        public void RemoveJoints() 
        {
            foreach (var joint in joints)
            {
                joint.RemoveJoint();
            }

            world = null;
        }
    }
}
