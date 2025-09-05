using Fitamas.Core;
using Fitamas.ECS;
using Fitamas.Graphics.ViewportAdapters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using R3;
using Fitamas.DebugTools;
using Fitamas.Math;

namespace Fitamas.Graphics
{
    public class CameraSystem : EntityUpdateSystem, IDrawGizmosSystem
    {
        private GameEngine game;

        private ComponentMapper<Transform> transformMapper;
        private ComponentMapper<Camera> cameraMapper;

        public CameraSystem(GameEngine game) : base(Aspect.All(typeof(Transform), typeof(Camera)))
        {
            this.game = game;
            game.ViewportAdapterProperty.Subscribe(value =>
            {
                if (Camera.Main != null)
                {
                    Camera.Main.ViewportAdapter = value;
                }
            });
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
            Camera.Current = Camera.Main;
            camera.ViewportAdapter = game.ViewportAdapterProperty.Value;
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

        public void DrawGizmos()
        {
            foreach (var entityId in ActiveEntities)
            {
                Camera camera = cameraMapper.Get(entityId);

                RectangleF rectangle = camera.WorldBounds();

                Gizmos.DrawRectangle(rectangle.Center, 0, rectangle.Size, Color.White);
            }
        }
    }
}
