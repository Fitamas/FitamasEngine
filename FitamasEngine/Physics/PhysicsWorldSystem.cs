using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Fitamas.Entities;
using Fitamas.Extended.Entities;
using Fitamas.Collections;
using Fitamas.Physics.Characters;
using Fitamas.Math2D;
using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Common.PolygonManipulation;
using nkast.Aether.Physics2D.Common;
using nkast.Aether.Physics2D.Dynamics.Joints;
using nkast.Aether.Physics2D.Dynamics;
using System.Linq;
using Transform = Fitamas.Entities.Transform;

namespace Fitamas.Physics
{
    public class PhysicsWorldSystem : IFixedUpdateSystem
    {
        internal World World { get; }

        private EntityManager entityManager;

        private Dictionary<int, Body> entityToBodyMap;
        private Dictionary<Body, int> bodyToEntityMap;

        private Dictionary<int, Joint> entityToJointMap;
        private Dictionary<Joint, int> jointToEntityMap;

        public PhysicsWorldSystem()
        {
            Vector2 gravity = new Vector2(0, -9.8f);

            World = new World(gravity);
 
            entityToBodyMap = new Dictionary<int, Body>();
            bodyToEntityMap = new Dictionary<Body, int>();

            entityToJointMap = new Dictionary<int, Joint>();
            jointToEntityMap = new Dictionary<Joint, int>();

            World.BodyRemoved += (s, b) =>
            {
                if (bodyToEntityMap.Remove(b, out int entityId))
                {
                    entityToBodyMap.Remove(entityId);
                }
            };

            World.JointRemoved += (s, j) =>
            {
                if (jointToEntityMap.Remove(j, out int entityId))
                {
                    entityToJointMap.Remove(entityId);
                }
            };
        }

        public void Initialize(GameWorld world)
        {
            entityManager = world.EntityManager;

            entityManager.EntityRemoved += RemoveEntity;

            world.RegisterSystem(new PhysicsBodySystem(this));
            world.RegisterSystem(new PhysicsJointSystem(this));
            world.RegisterSystem(new PhysicsCollisionFilterSystem(this));
            world.RegisterSystem(new CharacterController(this));
        }

        public void FixedUpdate(float deltaTime)
        {
            World.Step(deltaTime);
        }

        private void RemoveEntity(int entityId)
        {
            RemoveBody(entityId);
            RemoveJoint(entityId);
        }

        internal Body CreateBody(int entityId, Transform transform, MotionType motionType)
        {
            RemoveBody(entityId);
            Body body = World.CreateBody(transform.Position, transform.Rotation, (BodyType)motionType);
            body.BodyType = BodyType.Static;
            entityToBodyMap.Add(entityId, body);
            bodyToEntityMap.Add(body, entityId);
            return body;
        }

        public void CreateRigidBody(int entityId, Transform transform, PhysicsRigidBody rigidBody)
        {
            if (!TryGetBody(entityId, out Body body))
            {
                body = CreateBody(entityId, transform, rigidBody.MotionType);
            }
            body.FixedRotation = rigidBody.FixedRotation;
        }

        public void CreateCollider(int entityId, Transform transform, PhysicsCollider collider)
        {
            if (!TryGetBody(entityId, out Body body))
            {
                body = CreateBody(entityId, transform, MotionType.Static);
            }

            foreach (var fixture in body.FixtureList.ToArray())
            {
                body.Remove(fixture);
            }

            switch (collider.ColliderType)
            {
                case ColliderType.Box:
                    {
                        body.CreateRectangle(collider.Size.X, collider.Size.Y, 1, collider.Offset);
                        break;
                    }
                case ColliderType.Circle:
                    {
                        body.CreateCircle(collider.Radius, 1, collider.Offset);
                        break;
                    }
                case ColliderType.Polygon:
                    {
                        if (collider.ColliderShapes != null)
                        {
                            Triangulator.Process(collider.ColliderShapes, out Vector2[] verts, out int[] ind);
                            List<Vertices> triangles = new List<Vertices>();
                            int count = ind.Length / 3;

                            for (int i = 0; i < count; i++)
                            {
                                Vector2[] polygon = new Vector2[3];
                                for (int j = 0; j < 3; j++)
                                {
                                    int index = ind[i * 3 + j];
                                    polygon[j] = verts[index];
                                }
                                Vertices triangle = new Vertices(polygon);
                                triangle.ForceCounterClockWise();
                                triangles.Add(triangle);
                            }

                            List<Vertices> compositeShape = SimpleCombiner.PolygonizeTriangles(triangles);

                            foreach (var shape in compositeShape)
                            {
                                PolygonShape polygon = new PolygonShape(shape, 1);
                                body.CreateFixture(polygon);
                            }
                        }

                        break;
                    }
                case ColliderType.Capsule:
                    {
                        //body = world.CreateCapsule(scale.Y, scale.X, 1, position, rotation);
                        break;
                    }
            }
        }

        public void RemoveCollider(int entityId)
        {
            if (TryGetBody(entityId, out Body body))
            {
                foreach (var fixture in body.FixtureList.ToArray())
                {
                    body.Remove(fixture);
                }
            }
        }

        internal bool TryGetBody(int entityId, out Body body)
        {
            return entityToBodyMap.TryGetValue(entityId, out body);
        }

        internal Body GetBody(int entityId)
        {
            return entityToBodyMap[entityId];
        }

        internal Entity GetEntity(Body body)
        {
            int entityId = bodyToEntityMap[body];
            return entityManager.Get(entityId);
        }

        public void RemoveBody(int entityId)
        {
            if (entityToBodyMap.Remove(entityId, out Body body))
            {
                bodyToEntityMap.Remove(body);
                World.Remove(body);
            }
        }

        internal Joint CreateJoint(int entityId, PhysicsJoint physicsJoint)
        {
            RemoveJoint(entityId);
            Body bodyA = GetBody(physicsJoint.EntityA.Id);
            Body bodyB = GetBody(physicsJoint.EntityB.Id);
            Vector2 anchorA = physicsJoint.AnchorA;
            Vector2 anchorB = physicsJoint.AnchorB;
            bool useWorldCoordinates = physicsJoint.UseWorldCoordinates;
            Joint joint = null;

            switch (physicsJoint.JointType)
            {
                case PhysicsJointType.Distance:
                    joint = JointFactory.CreateDistanceJoint(World, bodyA, bodyB, anchorA, anchorB, useWorldCoordinates);
                    break;
                case PhysicsJointType.Rope:
                    RopeJoint ropeJoint = JointFactory.CreateRopeJoint(World, bodyA, bodyB, anchorA, anchorB, useWorldCoordinates);
                    ropeJoint.MaxLength = physicsJoint.Distance;
                    joint = ropeJoint;
                    break;
                case PhysicsJointType.Weld:
                    WeldJoint weldJoint = JointFactory.CreateWeldJoint(World, bodyA, bodyB, anchorA, anchorB, useWorldCoordinates);
                    joint = weldJoint;
                    break;
                case PhysicsJointType.Revolute:
                    RevoluteJoint revoluteJoint = JointFactory.CreateRevoluteJoint(World, bodyA, bodyB, anchorA, anchorB, useWorldCoordinates);
                    joint = revoluteJoint;
                    break;
                case PhysicsJointType.Wheel:
                    WheelJoint wheelJoint = JointFactory.CreateWheelJoint(World, bodyA, bodyB, anchorA, anchorB, useWorldCoordinates);
                    wheelJoint.Frequency = physicsJoint.MotorFrequency;
                    joint = wheelJoint;
                    break;
            }

            entityToJointMap.Add(entityId, joint);
            jointToEntityMap.Add(joint, entityId);
            joint.CollideConnected = physicsJoint.CollideConnected;
            return joint;
        }

        internal bool TryGetJoint(int entityId, out Joint joint)
        {
            return entityToJointMap.TryGetValue(entityId, out joint);
        }

        internal Joint GetJoint(int entityId)
        {
            return entityToJointMap[entityId];
        }

        internal Entity GetEntity(Joint joint)
        {
            int entityId = jointToEntityMap[joint];
            return entityManager.Get(entityId);
        }

        public void RemoveJoint(int entityId)
        {
            if (entityToJointMap.Remove(entityId, out Joint joint))
            {
                jointToEntityMap.Remove(joint);
                World.Remove(joint);
            }
        }

        public void Dispose()
        {
            World.Clear();
        }
    }
}
