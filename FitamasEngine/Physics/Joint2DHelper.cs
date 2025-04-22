using Fitamas.Animation;
using Fitamas.Entities;
using Microsoft.Xna.Framework;

namespace Fitamas.Physics
{
    public static class Joint2DHelper
    {
        public static void CreateRevolt(IEntity entityA, Vector2 anchorA, IEntity entityB, Vector2 anchorB)
        {
            Collider collider = entityB.Get<Collider>();

            Joint2D joint = new Joint2D(Joint2DType.Revolute, collider, anchorA, anchorB);
            AddJoint(entityA, joint);
        }

        public static void CreateWheel<T>(EntityManager entityManager, string entityId, Entity body, Vector2 anchor, Vector2 axis)
        {
            Entity entity = entityManager.Create(/*entityId*/);
            //if (entity.TryGet(out Transform transform) && body.TryGet(out Transform transformBody))
            //{
            //    transform.Position = transformBody.Position;
            //}
            CreateWheel(entity, body, anchor, axis);
        }

        public static void CreateWheel(IEntity wheel, IEntity body, Vector2 anchor, Vector2 axis)
        {
            Joint2D joint = new Joint2D(Joint2DType.Wheel, body.Get<Collider>(), anchor, axis);
            
            AddJoint(wheel, joint);
        }

        public static Entity[] CreateRope(EntityManager entityManager, string entityId,
            Entity entityA, Vector2 anchorA, Entity entityB, Vector2 anchorB, float distance) 
        {
            return CreateRope(entityManager, entityManager.Create(/*entityId*/), entityA, anchorA, entityB, anchorB, distance);
        }

        public static Entity[] CreateRope(EntityManager entityManager, Entity entity,
            Entity entityA, Vector2 anchorA, Entity entityB, Vector2 anchorB, float distance)
        {
            Collider collider = entity.Get<Collider>();
            Collider colliderA = entityA.Get<Collider>();
            Collider colliderB = entityB.Get<Collider>();

            float chainLenght = collider.Scale.X;
            if (chainLenght <= 0)
            {
                chainLenght = 1;
            }
            int chainCount = (int)(distance / chainLenght);

            Entity[] entities = CreateRope(entityManager, entity, chainLenght, chainCount);

            Joint2D joint1 = new Joint2D(Joint2DType.Revolute, colliderA, new Vector2(-chainLenght / 2, 0), anchorA);
            AddJoint(entities[0], joint1);
  
            Joint2D joint2 = new Joint2D(Joint2DType.Revolute, colliderB, new Vector2(chainLenght / 2, 0), anchorB);
            AddJoint(entities[entities.Length - 1], joint2);

            Joint2D jointRope = new Joint2D(Joint2DType.Rope, colliderB, anchorA, anchorB);
            jointRope.maxRopeLenght = distance;
            jointRope.CollideConnected = true;
            AddJoint(entityA, jointRope);

            return entities;
        }

        public static Entity[] CreateRope<T>(EntityManager entityManager, string entityId, float chainLenght, int count)
        {
            return CreateRope(entityManager, entityManager.Create(/*entityId*/), chainLenght, count);
        }

        public static Entity[] CreateRope(EntityManager entityManager, Entity entity, float chainLenght, int count)
        {
            if (!entity.Has<Collider>())
            {
                return null;
            }

            if (count <= 0)
            {
                count = 1;
            }

            Entity[] entities = new Entity[count];
            entities[0] = entity;
            for (int i = 1; i < count; i++)
            {
                entities[i] = entityManager.Create(entity);
            }

            for (int i = 0; i < count - 1; i++) 
            {
                Collider collider = entities[i + 1].Get<Collider>();

                Joint2D joint = new Joint2D(Joint2DType.Revolute, collider, new Vector2(chainLenght / 2, 0), new Vector2(-chainLenght / 2, 0));
                AddJoint(entities[i], joint);
            }

            return entities;
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

        public static void CreateRagDoll(IEntity[] entities)
        {
            Collider[] colliders = new Collider[entities.Length];
            CharacterElement[] elements = new CharacterElement[entities.Length];

            for (int i = 0;i < entities.Length;i++)
            {
                if (entities[i].TryGet(out Collider collider))
                {
                    collider.BodyType = nkast.Aether.Physics2D.Dynamics.BodyType.Dynamic;
                    colliders[i] = collider;
                }
                else
                {
                    return;
                }

                if (entities[i].TryGet(out CharacterElement element))
                {
                    elements[i] = element;
                }
                else
                {
                    return;
                }
            }

            for (int i = 0; i < entities.Length;i++)
            {
                if (!elements[i].IsRoot)
                {
                    IEntity parent = null;
                    for (int j = 0; j < entities.Length; j++)
                    {
                        if (elements[i].ParentElement == elements[j].Name)
                        {
                            parent = entities[j];
                        }
                    }

                    if (parent != null && !entities[i].Has<Joint2DComponent>())
                    {
                        CreateRevolt(entities[i], Vector2.Zero, parent, elements[i].ConnetionPosition);
                    }
                }
            }
        }

        public static void RemoveRagDoll(IEntity[] entities)
        {
            for (int i = 0; i < entities.Length; i++)
            {
                if (entities[i].TryGet(out Collider collider))
                {
                    collider.BodyType = nkast.Aether.Physics2D.Dynamics.BodyType.Kinematic;
                }

                if (entities[i].TryGet(out CharacterElement element))
                {
                    if (element.IsRoot)
                    {
                        entities[i].Get<Transform>().LocalPosition = Vector2.Zero;
                    }
                }
            }
        }
    }
}
