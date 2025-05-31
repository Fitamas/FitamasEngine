using Fitamas;
using Fitamas.Core;
using Fitamas.Entities;
using Fitamas.Graphics;
using Fitamas.Math2D;
using Fitamas.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Physics.Gameplay
{
    public static class EntityHelper
    {
        public static void CreatePumpkin(GameWorld world, Vector2 position)
        {
            Entity entity = world.CreateEntity();
            entity.Attach(new SpriteRender()
            {
                Sprite = Sprite.Create("Pumpkin"),
            });
            entity.Attach(new Transform()
            {
                Position = position
            });
            entity.Attach(PhysicsCollider.CreateCircle(0.5f));
            entity.Attach(new PhysicsRigidBody()
            {
                MotionType = MotionType.Dynamic,
            });
        }

        public static void CreateLog(GameWorld world, Vector2 position)
        {
            Entity entity = world.CreateEntity();
            entity.Attach(new SpriteRender()
            {
                Sprite = Sprite.Create("Log"),
            });
            entity.Attach(new Transform()
            {
                Position = position
            });
            entity.Attach(PhysicsCollider.CreateBox(new Vector2(3, 0.6f)));
            entity.Attach(new PhysicsRigidBody()
            {
                MotionType = MotionType.Dynamic,
            });
        }

        public static void CreateRock(GameWorld world, Vector2 position)
        {
            Entity entity = world.CreateEntity();
            Vector2[][] mesh = [[
                    new Vector2(100, 10),
                    new Vector2(-100, 10),
                    new Vector2(-100, -10),
                    new Vector2(100, -10)
                    ]];
            entity.Attach(new Mesh()
            {
                Shapes = mesh
            });
            entity.Attach(new MeshRender()
            {
                Matireal = new Matireal(GameEngine.Instance.Content.Load<Texture2D>("Rock"))

            });
            entity.Attach(new Transform()
            {
                Position = position
            });
            entity.Attach(PhysicsCollider.CreatePolygon(mesh));
        }

        public static void CreateTestBox(GameWorld world, Vector2 position)
        {
            Entity entity = world.CreateEntity();
            entity.Attach(new SpriteRender()
            {
                Sprite = Sprite.Create("TestBox"),
            });
            entity.Attach(new Transform()
            {
                Position = position
            });
            entity.Attach(PhysicsCollider.CreateBox(new Vector2(1.3f, 1.3f)));
            entity.Attach(new PhysicsRigidBody()
            {
                MotionType = MotionType.Dynamic,
            });
        }

        public static Entity CreateWheel(GameWorld world, Vector2 position)
        {
            Entity entity = world.CreateEntity();
            entity.Attach(new SpriteRender()
            {
                Sprite = Sprite.Create("Wheel"),
            });
            entity.Attach(new Transform()
            {
                Position = position
            });
            entity.Attach(PhysicsCollider.CreateCircle(0.5f));
            entity.Attach(new PhysicsRigidBody()
            {
                MotionType = MotionType.Dynamic,
            });
            entity.Attach(new PhysicsCollisionFilter()
            {
                Filter = new CollisionFilter(1 << 5, 1)
            });

            return entity;
        }
    }
}
