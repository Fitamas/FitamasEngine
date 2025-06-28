using Fitamas.ECS;
using Fitamas.Extended.Entities;
using nkast.Aether.Physics2D.Dynamics;
using System;

namespace Fitamas.Physics
{
    public class PhysicsCollisionFilterSystem : EntityFixedUpdateSystem
    {
        private ComponentMapper<PhysicsRigidBody> rigidBodyMapper;
        private ComponentMapper<PhysicsCollisionFilter> collisionFilterMapper;

        public PhysicsCollisionFilterSystem() : base(Aspect.All(typeof(PhysicsCollisionFilter)))
        {

        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            rigidBodyMapper = mapperService.GetMapper<PhysicsRigidBody>();
            collisionFilterMapper = mapperService.GetMapper<PhysicsCollisionFilter>();
        }

        public override void FixedUpdate(float deltaTime)
        {
            foreach (var entityId in ActiveEntities)
            {
                PhysicsCollisionFilter collisionFilter = collisionFilterMapper.Get(entityId);
                PhysicsRigidBody rigidBody = rigidBodyMapper.Get(entityId);
                Body body = rigidBody.Body;

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
