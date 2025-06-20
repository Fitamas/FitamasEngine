using Fitamas.ECS;
using Fitamas.Extended.Entities;
using nkast.Aether.Physics2D.Dynamics;
using System;

namespace Fitamas.Physics
{
    public class PhysicsCollisionFilterSystem : EntityFixedUpdateSystem
    {
        private PhysicsWorldSystem physicsWorld;

        private ComponentMapper<PhysicsCollisionFilter> collisionFilterMapper;

        public PhysicsCollisionFilterSystem(PhysicsWorldSystem physicsWorld) : base(Aspect.All(typeof(PhysicsCollisionFilter)))
        {
            this.physicsWorld = physicsWorld;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            collisionFilterMapper = mapperService.GetMapper<PhysicsCollisionFilter>();
        }

        public override void FixedUpdate(float deltaTime)
        {
            foreach (var entityId in ActiveEntities)
            {
                PhysicsCollisionFilter collisionFilter = collisionFilterMapper.Get(entityId);
                Body body = physicsWorld.GetBody(entityId);

                Category category = collisionFilter.Filter.CollisionCategories;
                Category collidesWith = collisionFilter.Filter.CollidesWith;

                foreach (var fixture in body.FixtureList)
                {
                    fixture.CollisionCategories = category;
                    fixture.CollidesWith = collidesWith;
                }
            }
        }
    }
}
