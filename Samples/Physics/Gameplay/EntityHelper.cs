using Fitamas;
using Fitamas.Core;
using Fitamas.ECS;
using Fitamas.Graphics;
using Fitamas.Math;
using Fitamas.Physics;
using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Physics.Gameplay
{
    public static class EntityHelper
    {
        public static Entity CreatePumpkin(GameWorld world, Vector2 position)
        {
            Entity entity = world.CreateEntity();
            entity.Attach(new SpriteRendererComponent()
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
            return entity;
        }

        public static Entity CreateLight(GameWorld world, Vector2 position)
        {
            Entity entity = world.CreateEntity();
            entity.Attach(new SpriteRendererComponent()
            {
                Sprite = Sprite.Create("Spot1"),
                Color = Color.Red,
            });
            entity.Attach(new Transform()
            {
                Position = position,
                Scale = Vector2.One / 2
            });
            entity.Attach(PhysicsCollider.CreateCircle(0.5f));
            entity.Attach(new PhysicsRigidBody()
            {
                MotionType = MotionType.Dynamic,
            });
            entity.Attach(new TagsComponent([Tags.Light]));
            return entity;
        }

        public static void CreateLog(GameWorld world, Vector2 position)
        {
            Entity entity = world.CreateEntity();
            entity.Attach(new SpriteRendererComponent()
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
            Material material = Material.DefaultOpaqueMaterial.Clone();
            material.MainTexture = GameEngine.Instance.Content.Load<Texture2D>("Rock");

            entity.Attach(new MeshComponent()
            {
                Shapes = mesh
            });
            entity.Attach(new MeshRendererComponent()
            {
                Matireal = material

            });
            entity.Attach(new Transform()
            {
                Position = position
            });
            entity.Attach(PhysicsCollider.CreatePolygon(mesh));
            entity.Attach(new PhysicsRigidBody()
            {
                MotionType = MotionType.Static,
            });
        }

        public static void CreateTestBox(GameWorld world, Vector2 position)
        {
            Entity entity = world.CreateEntity();
            entity.Attach(new SpriteRendererComponent()
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
            entity.Attach(new SpriteRendererComponent()
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
