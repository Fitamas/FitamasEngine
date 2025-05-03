using Fitamas;
using Fitamas.Entities;
using Fitamas.Graphics;
using Fitamas.Physics;
using Microsoft.Xna.Framework;

namespace Physics.Gameplay
{
    public static class EntityHelper
    {
        public static void CreatePumpkin(GameWorld world, Vector2 position)
        {
            Entity entity = world.CreateEntity();
            entity.Attach(new SpriteRender()
            {
                sprite = Sprite.Create("Pumpkin"),
            });
            entity.Attach(new Transform()
            {
                Position = position
            });
            entity.Attach(new Collider(0.5f, Vector2.Zero)
            {
                  
            });
        }

        public static void CreateLog(GameWorld world, Vector2 position)
        {
            Entity entity = world.CreateEntity();
            entity.Attach(new SpriteRender()
            {
                sprite = Sprite.Create("Log"),
            });
            entity.Attach(new Transform()
            {
                Position = position
            });
            entity.Attach(new Collider(ColliderType.Box, new Vector2(2, 1), Vector2.Zero, bodyType: nkast.Aether.Physics2D.Dynamics.BodyType.Static)
            {

            });
        }
    }
}
