using Fitamas.DebugTools;
using Fitamas.ECS;
using Fitamas.Extended.Entities;
using Fitamas.Math;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Common.PolygonManipulation;
using nkast.Aether.Physics2D.Common;
using nkast.Aether.Physics2D.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using Transform = Fitamas.ECS.Transform;

namespace Fitamas.Physics
{
    public class PhysicsColliderSystem : EntitySystem, IDrawGizmosSystem
    {
        private ComponentMapper<Transform> transformMapper;
        private ComponentMapper<PhysicsRigidBody> rigidBodyMapper;
        private ComponentMapper<PhysicsCollider> colliderMapper;

        public PhysicsColliderSystem() : base(Aspect.All(typeof(Transform), typeof(PhysicsRigidBody), typeof(PhysicsCollider)))
        {

        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform>();
            rigidBodyMapper = mapperService.GetMapper<PhysicsRigidBody>();
            rigidBodyMapper.OnPut += PutCollider;
            rigidBodyMapper.OnDelete += DeleteCollider;
            colliderMapper = mapperService.GetMapper<PhysicsCollider>();
            colliderMapper.OnPut += PutCollider;
            colliderMapper.OnDelete += DeleteCollider;
        }

        private void PutCollider(int entityId)
        {
            if (ActiveEntities.Contains(entityId))
            {
                PhysicsRigidBody rigidBody = rigidBodyMapper.Get(entityId);
                PhysicsCollider collider = colliderMapper.Get(entityId);
                Body body = rigidBody.Body;

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
        }

        private void DeleteCollider(int entityId)
        {
            if (rigidBodyMapper.TryGet(entityId, out PhysicsRigidBody rigidBody))
            {
                foreach (var fixture in rigidBody.Body.FixtureList.ToArray())
                {
                    rigidBody.Body.Remove(fixture);
                }
            }
        }

        public void DrawGizmos()
        {
            foreach (var entityId in ActiveEntities)
            {
                Transform transform = transformMapper.Get(entityId);
                PhysicsRigidBody rigidBody = rigidBodyMapper.Get(entityId);
                PhysicsCollider collider = colliderMapper.Get(entityId);

                Color color = Color.GreenYellow;
                Body body = rigidBody.Body;
                float rotation = body.Rotation;
                Vector2 position = body.Position + MathV.Rotate(collider.Offset, rotation);

                switch (collider.ColliderType)
                {
                    case ColliderType.Box:
                        Gizmos.DrawRectangle(position, rotation, collider.Size, color);
                        break;
                    case ColliderType.Circle:
                        Gizmos.DrawCircle(position, collider.Radius, color);
                        break;
                    case ColliderType.Polygon:
                        if (collider.ColliderShapes != null)
                        {
                            foreach (var polygon in collider.ColliderShapes)
                            {
                                Gizmos.DrawPolygon(position, rotation, polygon, color);
                            }
                        }
                        break;
                    case ColliderType.Capsule:
                        Vector2 scale = collider.Size;
                        Gizmos.DrawRectangle(position, rotation, new Vector2(scale.X * 2, scale.Y), color);

                        Vector2 posUp = transform.ToAbsolutePosition(new Vector2(0, scale.Y / 2) - collider.Offset);
                        Vector2 posDown = transform.ToAbsolutePosition(new Vector2(0, -scale.Y / 2) - collider.Offset);

                        Gizmos.DrawCircle(posUp, scale.X, color);
                        Gizmos.DrawCircle(posDown, scale.X, color);
                        break;
                }
            }
        }
    }
}
