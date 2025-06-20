using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Fitamas.ECS;
using Fitamas.Extended.Entities;
using Fitamas.Collections;
using Fitamas.Physics.Characters;
using nkast.Aether.Physics2D.Dynamics.Joints;
using nkast.Aether.Physics2D.Dynamics;

namespace Fitamas.Physics
{
    public class PhysicsWorldSystem : IFixedUpdateSystem
    {
        private PhysicsBodySystem bodySystem;
        private PhysicsJointSystem jointSystem;

        internal World World { get; }

        public PhysicsWorldSystem()
        {
            Vector2 gravity = new Vector2(0, -9.8f);

            World = new World(gravity);
        }

        public void Initialize(GameWorld world)
        {
            world.RegisterSystem(bodySystem = new PhysicsBodySystem(this));
            world.RegisterSystem(new PhysicsColliderSystem(this));
            world.RegisterSystem(jointSystem = new PhysicsJointSystem(this));
            world.RegisterSystem(new PhysicsCollisionFilterSystem(this));
            world.RegisterSystem(new CharacterController(this));
        }

        public void FixedUpdate(float deltaTime)
        {
            World.Step(deltaTime);
        }

        internal Body GetBody(int entityId)
        {
            return bodySystem.GetBody(entityId);
        }

        internal Entity GetEntity(Body body)
        {
            return bodySystem.GetEntity(body);
        }

        internal Joint GetJoint(int entityId)
        {
            return jointSystem.GetJoint(entityId);
        }

        internal Entity GetEntity(Joint joint)
        {
            return jointSystem.GetEntity(joint);
        }

        public void Dispose()
        {
            World.Clear();
        }
    }
}
