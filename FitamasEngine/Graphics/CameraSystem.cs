using Fitamas.Core;
using Fitamas.ECS;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Graphics
{
    public class CameraSystem : EntityUpdateSystem
    {
        private GameEngine game;

        private ComponentMapper<Transform> transformMapper;
        private ComponentMapper<Camera> cameraMapper;

        public CameraSystem(GameEngine game) : base(Aspect.All(typeof(Transform), typeof(Camera)))
        {
            this.game = game;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform>();
            cameraMapper = mapperService.GetMapper<Camera>();
            cameraMapper.OnPut += PutCamera;
        }

        private void PutCamera(int entityId)
        {
            Camera camera = cameraMapper.Get(entityId);
            Camera.Main = camera;
            camera.ViewportAdapter = game.WindowViewportAdapter;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entityId in ActiveEntities)
            {
                Camera camera = cameraMapper.Get(entityId);
                Transform transform = transformMapper.Get(entityId);

                camera.Position = transform.Position;
                camera.Rotation = transform.Rotation;
            }
        }
    }
}
