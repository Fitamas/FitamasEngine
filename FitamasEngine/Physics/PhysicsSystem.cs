using Fitamas.Entities;
using Fitamas.Extended.Entities;
using Fitamas.Math2D;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;

namespace Fitamas.Physics
{
    public class PhysicsSystem : EntityFixedUpdateSystem
    {
        public World World { get; }

        private ComponentMapper<Transform> transformMapper;
        private ComponentMapper<Collider> colliderMapper;
        private ComponentMapper<Joint2DComponent> jointMapper;

        public PhysicsSystem() : base(Aspect.All(typeof(Transform)).One(typeof(Collider), typeof(Joint2DComponent)))
        {
            Vector2 gravity = new Vector2(0, -9.8f);

            World = new World(gravity);
            Physics2D.System = this;
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

                    collider.CreateBody(World, transform);

                    if (GetEntity(entityId).TryGet(out Mesh mesh))
                    {
                        collider.SetPoligons(mesh.Vertices, mesh.Ind);
                    }

                    if (jointMapper.TryGet(entityId, out Joint2DComponent joint))
                    {
                        joint.CreateJoints(World, collider);
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

        public override void FixedUpdate(float deltaTime)
        {
            World.Step(deltaTime);

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

        internal bool TryGet(Body body, out Entity entity, out Collider collider)
        {
            foreach (var box in ActiveEntities)
            {
                collider = colliderMapper.Get(box);

                if (body == collider.Body)
                {
                    entity = GetEntity(box);
                    return true;
                }
            }

            entity = null;
            collider = null;
            return false;
        }
    }
}
