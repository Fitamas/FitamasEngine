using Fitamas.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Joints;
using System;

namespace Fitamas.Physics
{
    public static class PhysicsDebugTools
    {
        private static FixedMouseJoint mouseJoint;
        private static bool isConnect = false;

        public static bool IsConnected => isConnect;

        public static void RemoveMouseJoint()
        {
            if (mouseJoint != null)
            {
                Physics2D.World.Remove(mouseJoint);
                mouseJoint = null;
                isConnect = false;
            }
        }

        public static void SetMousePosition(Vector2 position)
        {
            if (mouseJoint != null)
            {
                mouseJoint.WorldAnchorB = position;
            }
        }

        public static void CreateMousJoint(Vector2 position)
        {
            Fixture fixture = Physics2D.World.TestPoint(position);

            if (fixture != null)
            {
                CreateMousJoint(fixture.Body, position);
            }
        }

        public static void CreateMousJoint(Entity entity, Vector2 position)
        {
            Collider collider = entity.Get<Collider>();
            if (!collider.IsReady)
            {
                return;
            }

            CreateMousJoint(collider.Body, position);
        }

        public static void CreateMousJoint(Body body, Vector2 position)
        {
            RemoveMouseJoint();

            mouseJoint = JointFactory.CreateFixedMouseJoint(Physics2D.World, body, position);
            isConnect = true;

            mouseJoint.WorldAnchorB = position;
        }

        private static  Entity entityA;
        private static Vector2 positionA;

        public static void CreateRopeJoint(Vector2 position)
        {
            RayCastHit hit = Physics2D.TestPoint(position);

            if (hit.Entity != null)
            {
                if (entityA == null)
                {
                    entityA = hit.Entity;
                    positionA = position;
                }
                else if (entityA != hit.Entity)
                {
                    Vector2 localA = entityA.Get<Transform>().ToLocalPosition(positionA);
                    Vector2 localB = hit.Entity.Get<Transform>().ToLocalPosition(position);

                    Joint2DHelper.CreateRope(entityA, localA, hit.Entity, localB, Vector2.Distance(positionA, position));

                    entityA = null;
                }
            }
        }

        public static void CreateRevoltJoint(Vector2 position)
        {
            RayCastHit hit = Physics2D.TestPoint(position);

            if (hit.Entity != null)
            {
                if (entityA == null)
                {
                    entityA = hit.Entity;
                    positionA = position;
                }
                else if (entityA != hit.Entity)
                {
                    Vector2 localA = entityA.Get<Transform>().ToLocalPosition(positionA);
                    Vector2 localB = hit.Entity.Get<Transform>().ToLocalPosition(position);

                    Joint2DHelper.CreateRevolt(entityA, localA, hit.Entity, localB);

                    entityA = null;
                }
            }
        }

        public static void CreateWheelJoint(Vector2 position)
        {
            RayCastHit hit = Physics2D.TestPoint(position);

            if (hit.Entity != null)
            {
                if (entityA == null)
                {
                    entityA = hit.Entity;
                    positionA = position;
                }
                else if (entityA != hit.Entity)
                {
                    Vector2 localA = entityA.Get<Transform>().ToLocalPosition(positionA);
                    Vector2 localB = hit.Entity.Get<Transform>().ToLocalPosition(position);

                    Joint2DHelper.CreateWheel(entityA, localA, hit.Entity, localB);

                    entityA = null;
                }
            }
        }
    }
}
