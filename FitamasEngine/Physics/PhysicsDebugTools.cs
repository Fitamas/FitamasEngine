using Fitamas.Core;
using Fitamas.ECS;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Joints;
using System;

namespace Fitamas.Physics
{
    public static class PhysicsDebugTools
    {
        private static PhysicsWorld physicsWorld = GameEngine.Instance.PhysicsWorld;
        private static FixedMouseJoint mouseJoint;
        private static bool isConnect = false;

        public static bool IsConnected => isConnect;

        public static void RemoveMouseJoint()
        {
            if (mouseJoint != null)
            {
                physicsWorld.World.Remove(mouseJoint);
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
            Fixture fixture = physicsWorld.World.TestPoint(position);

            if (fixture != null)
            {
                CreateMousJoint(fixture.Body, position);
            }
        }

        public static void CreateMousJoint(Entity entity, Vector2 position)
        {
            CreateMousJoint(physicsWorld.GetBody(entity.Id), position);
        }

        public static void CreateMousJoint(Body body, Vector2 position)
        {
            RemoveMouseJoint();

            mouseJoint = JointFactory.CreateFixedMouseJoint(physicsWorld.World, body, position);
            isConnect = true;

            mouseJoint.WorldAnchorB = position;
        }
    }
}
