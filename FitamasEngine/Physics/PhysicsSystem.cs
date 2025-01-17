using Fitamas.Entities;
using Fitamas.Math2D;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;

namespace Fitamas.Physics
{
    public class PhysicsSystem : EntityUpdateSystem
    {
        private Vector2 gravity = new Vector2(0, -9.8f);
        private float accamulatorTime;

        public World physicsWorld { get; private set; }

        private ComponentMapper<Transform> transformMapper;
        private ComponentMapper<Collider> colliderMapper;
        private ComponentMapper<Joint2DComponent> jointMapper;

        public PhysicsSystem() : base(Aspect.All(typeof(Transform)).One(typeof(Collider), typeof(Joint2DComponent)))
        {
            physicsWorld = new World(gravity);

            Physics2D.Init(this);
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform>();
            colliderMapper = mapperService.GetMapper<Collider>();
            jointMapper = mapperService.GetMapper<Joint2DComponent>();
        }

        protected override void OnEntityAdded(int entityId)
        {
            InitEntity(entityId);
        }

        protected override void OnEntityChanged(int entityId)
        {
            InitEntity(entityId);
        }

        private void InitEntity(int entityId)
        {
            if (ActiveEntities.Contains(entityId))
            {
                if (colliderMapper.TryGet(entityId, out Collider collider))
                {
                    Transform transform = transformMapper.Get(entityId);

                    collider.CreateBody(physicsWorld, transform);

                    if (GetEntity(entityId).TryGet(out Mesh mesh))
                    {
                        collider.SetPoligons(mesh.Vertices, mesh.Ind);
                    }

                    if (jointMapper.TryGet(entityId, out Joint2DComponent joint))
                    {
                        joint.CreateJoints(physicsWorld, collider);
                    }
                }
            }
        }

        protected override void OnEntityRemoved(int entityId)
        {
            if (ActiveEntities.Contains(entityId))
            {
                Entity entity = GetEntity(entityId);
                
                if (entity.TryGet(out Collider collider))
                {
                    collider.RemoveBody();
                }

                if (entity.TryGet(out Joint2DComponent joint))
                {
                    joint.RemoveJoints();
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            accamulatorTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (accamulatorTime > Physics2D.FixedTimeStep)
            {
                accamulatorTime -= Physics2D.FixedTimeStep;

                GameWorld.FixedUpdate(Physics2D.FixedTimeStep);

                physicsWorld.Step(Physics2D.FixedTimeStep);
            }

            foreach (var box in ActiveEntities)
            {
                Transform transform = transformMapper.Get(box);
                Collider collider = colliderMapper.Get(box);

                if (transform.Parent != null && collider.BodyType != BodyType.Dynamic)
                {
                    collider.Position = transform.Position;
                    collider.Rotation = transform.Rotation;
                }
                else
                {
                    transform.Position = collider.Position;
                    transform.Rotation = collider.Rotation;
                }
            }
        }

        public bool TryGetEntity(Body body, out Entity entity)
        {
            foreach (var box in ActiveEntities)
            {
                Collider boxCollider = colliderMapper.Get(box);

                if (body == boxCollider.Body)
                {
                    entity = GetEntity(box);
                    return true;
                }
            }

            entity = null;
            return false;
        }
    }
}
