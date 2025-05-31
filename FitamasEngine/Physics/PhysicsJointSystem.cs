using Fitamas.Entities;
using Fitamas.Extended.Entities;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Joints;
using System;

namespace Fitamas.Physics
{
    public class PhysicsJointSystem : EntityFixedUpdateSystem
    {
        private PhysicsWorldSystem physicsWorld;

        private ComponentMapper<PhysicsJoint> jointMapper;

        public PhysicsJointSystem(PhysicsWorldSystem physicsWorld) : base(Aspect.All(typeof(PhysicsJoint)))
        {
            this.physicsWorld = physicsWorld;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            jointMapper = mapperService.GetMapper<PhysicsJoint>();

            jointMapper.OnPut += PutJoint;
            jointMapper.OnDelete += DeleteJoint;
        }

        private void PutJoint(int entityId)
        {
            physicsWorld.RemoveJoint(entityId);

            PhysicsJoint physicsJoint = jointMapper.Get(entityId);
            physicsWorld.CreateJoint(entityId, physicsJoint);
        }

        private void DeleteJoint(int entityId)
        {
            physicsWorld.RemoveJoint(entityId);
        }

        public override void FixedUpdate(float deltaTime)
        {

        }
    }
}
