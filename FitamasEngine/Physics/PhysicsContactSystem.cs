using Fitamas.ECS;
using Fitamas.Extended.Entities;
using nkast.Aether.Physics2D.Dynamics.Contacts;
using System;

namespace Fitamas.Physics
{
    public class PhysicsContactSystem : EntityFixedUpdateSystem
    {
        private PhysicsWorld physicsWorld;

        private ComponentMapper<PhysicsRigidBody> rigidBodyMapper;

        public PhysicsContactSystem(PhysicsWorld physicsWorld) : base(Aspect.All(typeof(PhysicsRigidBody)))
        {
            this.physicsWorld = physicsWorld;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            rigidBodyMapper = mapperService.GetMapper<PhysicsRigidBody>();
        }

        public override void FixedUpdate(float deltaTime)
        {

            foreach (int entityId in ActiveEntities)
            {
                Entity entity = GetEntity(entityId);
                PhysicsRigidBody rigidBody = rigidBodyMapper.Get(entityId);

                if (rigidBody.Body.ContactList != null)
                {
                    if (!entity.TryGet(out PhysicsContactEvent contactEvent))
                    {
                        contactEvent = new PhysicsContactEvent();
                        entity.Attach(contactEvent);
                    }

                    ContactEdge contact = rigidBody.Body.ContactList;
                    contactEvent.Entity = physicsWorld.GetEntity(contact.Other);
                    contactEvent.Collider = contactEvent.Entity.Get<PhysicsCollider>();
                }
                else
                {
                    entity.Detach<PhysicsContactEvent>();
                }
            }
        }
    }
}
