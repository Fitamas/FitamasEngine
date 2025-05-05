using Fitamas.Entities;
using Microsoft.Xna.Framework;

namespace Fitamas.Physics
{
    public static class Joint2DHelper
    {
        public static void CreateRevolt(Entity entityA, Vector2 anchorA, Entity entityB, Vector2 anchorB)
        {
            Collider collider = entityB.Get<Collider>();
            Joint2D joint = new Joint2D(Joint2DType.Revolute, collider, anchorA, anchorB);
            AddJoint(entityA, joint);
        }

        public static void CreateWheel(Entity entityA, Vector2 anchorA, Entity entityB, Vector2 anchorB)
        {
            Collider collider = entityB.Get<Collider>();
            Joint2D joint = new Joint2D(Joint2DType.Wheel, collider, anchorA, anchorB);
            AddJoint(entityA, joint);
        }

        public static void CreateRope(Entity entityA, Vector2 anchorA, Entity entityB, Vector2 anchorB, float distance)
        {
            Collider collider = entityB.Get<Collider>();
            Joint2D joint = new Joint2D(Joint2DType.Rope, collider, anchorA, anchorB);
            joint.maxRopeLenght = distance;
            joint.CollideConnected = true;
            AddJoint(entityA, joint);
        }

        public static void AddJoint(IEntity entity, Joint2D joint)
        {
            if (entity.TryGet(out Joint2DComponent component))
            {
                component.AddJoint(joint);
            }
            else
            {
                component = new Joint2DComponent();
                component.AddJoint(joint);
                entity.Attach(component);
            }
        }
    }
}
