using Fitamas.DebugTools;
using Fitamas.Entities;
using Fitamas.Extended.Entities;
using Fitamas.Math2D;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;

namespace Fitamas.Physics
{
    public class PhysicsBodySystem : EntityFixedUpdateSystem, IDrawGizmosSystem
    {
        private PhysicsWorldSystem physicsWorld;

        private ComponentMapper<Transform> transformMapper;
        private ComponentMapper<PhysicsRigidBody> rigidBodyMapper;
        private ComponentMapper<PhysicsCollider> colliderMapper;


        public PhysicsBodySystem(PhysicsWorldSystem physicsWorld) : base(Aspect.All(typeof(Transform)).One(typeof(PhysicsRigidBody), typeof(PhysicsCollider)))
        {
            this.physicsWorld = physicsWorld;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform>();
            rigidBodyMapper = mapperService.GetMapper<PhysicsRigidBody>();
            rigidBodyMapper.OnPut += PutBody;
            rigidBodyMapper.OnDelete += DeleteBody;
            colliderMapper = mapperService.GetMapper<PhysicsCollider>();
            colliderMapper.OnPut += PutCollider;
            colliderMapper.OnDelete += DeleteCollider;
        }

        private void PutBody(int entityId)
        {
            if (ActiveEntities.Contains(entityId))
            {
                Transform transform = transformMapper.Get(entityId);
                PhysicsRigidBody rigidBody = rigidBodyMapper.Get(entityId);
                physicsWorld.CreateRigidBody(entityId, transform, rigidBody);
            }
        }

        private void DeleteBody(int entityId)
        {
            if (!colliderMapper.Has(entityId))
            {
                physicsWorld.RemoveBody(entityId);
            }
        }

        private void PutCollider(int entityId)
        {
            if (ActiveEntities.Contains(entityId))
            {
                Transform transform = transformMapper.Get(entityId);
                PhysicsCollider collider = colliderMapper.Get(entityId);
                physicsWorld.CreateCollider(entityId, transform, collider);
            }
        }

        private void DeleteCollider(int entityId)
        {
            if (rigidBodyMapper.Has(entityId))
            {
                physicsWorld.RemoveCollider(entityId);
            }
            else
            {
                physicsWorld.RemoveBody(entityId);
            }
        }

        public override void FixedUpdate(float deltaTime)
        {
            foreach (var entityId in ActiveEntities)
            {
                Transform transform = transformMapper.Get(entityId);
                Body body = physicsWorld.GetBody(entityId);
                PhysicsRigidBody rigidBody = rigidBodyMapper.Get(entityId);

                if (rigidBody != null && rigidBody.MotionType != MotionType.Static)
                {
                    body.BodyType = (BodyType)rigidBody.MotionType;
                    body.FixedRotation = rigidBody.FixedRotation;

                    transform.Position = body.Position;
                    transform.Rotation = body.Rotation;
                }
                else
                {
                    body.Position = transform.Position;
                    body.Rotation = transform.Rotation;
                }
            }
        }

        public void DrawGizmos()
        {
            foreach (var entityId in ActiveEntities)
            {
                Transform transform = transformMapper.Get(entityId);

                if (colliderMapper.TryGet(entityId, out PhysicsCollider collider))
                {
                    Color color = Color.GreenYellow;
                    Body body = physicsWorld.GetBody(entityId);
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
}
