using Fitamas.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Joints;
using System;
using System.Runtime.CompilerServices;

namespace Fitamas.Physics
{
    public static class PhysicsDebugTools
    {
        private static FixedMouseJoint mouseJoint;
        private static bool isConnect = false;

        public static bool IsConnected => isConnect;

        public static void RemoveDebugMouseJoint()
        {
            if (mouseJoint != null)
            {
                Physics2D.World.Remove(mouseJoint);
                mouseJoint = null;
                isConnect = false;
            }
        }

        public static void SetDebugMousePosition(Vector2 position)
        {
            if (mouseJoint == null)
            {
                return;
            }
            else
            {
                mouseJoint.WorldAnchorB = position;
            }
        }

        public static void CreateDebugMousJoint(Entity entity, Vector2 position)
        {
            Collider collider = entity.Get<Collider>();
            if (!collider.IsReady)
            {
                return;
            }

            CreateDebugMousJoint(collider.Body, position);
        }

        public static void CreateDebugMousJoint(Body body, Vector2 position)
        {
            if (mouseJoint != null)
            {
                Physics2D.World.Remove(mouseJoint);
            }
            mouseJoint = JointFactory.CreateFixedMouseJoint(Physics2D.World, body, position);
            isConnect = true;

            mouseJoint.WorldAnchorB = position;
        }

        private static  Entity entityA;
        private static Vector2 positionA;
        private static bool isFirstConnection = true;

        public static void CreateRopeConnection(EntityManager entityManager, string ropeId, Entity connectionEntity, Vector2 worldPosition)
        {
            if (isFirstConnection)
            {
                entityA = connectionEntity;
                positionA = worldPosition;
                isFirstConnection = false;
            }
            else
            {
                Vector2 localA = entityA.Get<Transform>().ToLocalPosition(positionA);
                Vector2 localB = connectionEntity.Get<Transform>().ToLocalPosition(worldPosition);
                float distance = Vector2.Distance(worldPosition, positionA);

                Joint2DHelper.CreateRope(entityManager, ropeId, entityA, localA, connectionEntity, localB, distance);

                isFirstConnection = true;
            }
        }

        public static void CreateRevoltConnection(Entity connectionEntity, Vector2 worldPosition)
        {
            if (isFirstConnection)
            {
                entityA = connectionEntity;
                positionA = worldPosition;
                isFirstConnection = false;
            }
            else
            {
                Vector2 localA = entityA.Get<Transform>().ToLocalPosition(positionA);
                Vector2 localB = connectionEntity.Get<Transform>().ToLocalPosition(worldPosition);
                float distance = Vector2.Distance(worldPosition, positionA);

                Joint2DHelper.CreateRevolt(entityA, localA, connectionEntity, localB);

                isFirstConnection = true;
            }
        }

        public static void CreateWheelConnection(EntityManager entityManager, string entityId, Entity body, Vector2 worldAnchor, Vector2 axis)
        {
            Vector2 anchor = body.Get<Transform>().ToLocalPosition(worldAnchor);
            Entity wheel = entityManager.Create(/*entityId*/);

            wheel.Get<Collider>().BodyPosition = worldAnchor;

            Joint2DHelper.CreateWheel(wheel, body, anchor, axis);
        }
    }
}
