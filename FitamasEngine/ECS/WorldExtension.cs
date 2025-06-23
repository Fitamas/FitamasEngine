using Fitamas.Audio;
using Fitamas.Graphics;
using System;

namespace Fitamas.ECS
{
    public static class WorldExtension
    {
        public static Entity CreateMainCamera(this GameWorld world)
        {
            Entity entity = world.CreateEntity("MainCamera");
            entity.Attach(new Transform());
            entity.Attach(new Camera());
            entity.Attach(new AudioListener());

            return entity;
        }
    }
}
