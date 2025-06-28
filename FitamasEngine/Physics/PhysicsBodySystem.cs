using Fitamas.Collections;
using Fitamas.ECS;
using Fitamas.Extended.Entities;
using nkast.Aether.Physics2D.Dynamics;
using System.Linq;

namespace Fitamas.Physics
{
    public class PhysicsBodySystem : EntityFixedUpdateSystem
    {
        private PhysicsWorld physicsWorld;

        private Bag<Body> bodies;
        private ComponentMapper<Transform> transformMapper;
        private ComponentMapper<PhysicsRigidBody> rigidBodyMapper;

        public PhysicsBodySystem(PhysicsWorld physicsWorld) : base(Aspect.All(typeof(Transform), typeof(PhysicsRigidBody)))
        {
            this.physicsWorld = physicsWorld;

            bodies = new Bag<Body>();

            physicsWorld.World.BodyRemoved += (s, b) =>
            {
                int index = bodies.IndexOf(b);

                if (index != -1)
                {
                    bodies[index] = null;
                    rigidBodyMapper.Delete(index);
                }
            };
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform>();
            transformMapper.OnPut += PutBody;
            transformMapper.OnDelete += DeleteBody;
            rigidBodyMapper = mapperService.GetMapper<PhysicsRigidBody>();
            rigidBodyMapper.OnPut += PutBody;
            rigidBodyMapper.OnDelete += DeleteBody;
        }

        private void PutBody(int entityId)
        {
            if (ActiveEntities.Contains(entityId))
            {
                Transform transform = transformMapper.Get(entityId);
                PhysicsRigidBody rigidBody = rigidBodyMapper.Get(entityId);
                
                Body body = physicsWorld.World.CreateBody(transform.Position, transform.Rotation, (BodyType)rigidBody.MotionType);
                body.BodyType = BodyType.Static;
                body.FixedRotation = rigidBody.FixedRotation;
                bodies[entityId] = body;

                rigidBody.Body = body;
            }
        }

        private void DeleteBody(int entityId)
        {
            if (rigidBodyMapper.TryGet(entityId, out PhysicsRigidBody rigidBody))
            {
                physicsWorld.World.Remove(rigidBody.Body);
            }
        }

        public override void FixedUpdate(float deltaTime)
        {
            foreach (var entityId in ActiveEntities)
            {
                Transform transform = transformMapper.Get(entityId);
                Body body = physicsWorld.GetBody(entityId);
                PhysicsRigidBody rigidBody = rigidBodyMapper.Get(entityId);

                body.BodyType = (BodyType)rigidBody.MotionType;
                body.FixedRotation = rigidBody.FixedRotation;

                if (rigidBody.MotionType != MotionType.Static)
                {
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

        internal Body GetBody(int entityId)
        {
            return bodies[entityId];
        }

        internal Entity GetEntity(Body body)
        {
            int entityId = bodies.IndexOf(body);
            return GetEntity(entityId);
        }
    }
}
