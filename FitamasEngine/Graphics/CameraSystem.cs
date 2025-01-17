using Fitamas.Entities;
using Fitamas.Input;
using Fitamas.Main;
using Fitamas.Math2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Fitamas.Graphics
{
    public class CameraSystem : EntitySystem, IUpdateSystem
    {
        private ComponentMapper<Transform> transformMapper;
        private ComponentMapper<CameraFollow> followMapper;

        private static Camera camera;

        public static Camera MainCamera => camera;

        private Vector2 localPosition;
        private float cameraSpeed = 5;
        private float attachment = 0.3f;
        private float maxDistance = 2;

        private float debugCameraSpeed = 15;

        public CameraSystem(Game game) : base(Aspect.All(typeof(Transform), typeof(CameraFollow)))
        {
            camera = new Camera(game);
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform>();
            followMapper = mapperService.GetMapper<CameraFollow>();
        }

        public void Update(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (camera.CameraType)
            {
                case CameraType.Game:
                    if (ActiveEntities.Count > 0)
                    {
                        int id = ActiveEntities[0];
                        Transform transform = transformMapper.Get(id);

                        Vector2 targetPosition = transform.Position;
                        Vector2 cameraTarget = Vector2.Lerp(targetPosition, InputSystem.mousePositionInWorld, attachment) - targetPosition;
                        if (cameraTarget.LengthSquared() > maxDistance * maxDistance)
                        {
                            localPosition = cameraTarget.NormalizeF() * maxDistance;
                        }
                        else
                        {
                            localPosition = Vector2.Lerp(localPosition, cameraTarget, timeDelta * cameraSpeed);
                        }

                        camera.Position = localPosition + targetPosition;
                    }
                    break;
                case CameraType.Follow:
                    if (ActiveEntities.Count > 0)
                    {
                        int id = ActiveEntities[0];
                        Transform transform = transformMapper.Get(id);

                        camera.Position = transform.Position;
                    }
                    break;
                case CameraType.Debug:
                    Vector2 direction = InputSystem.actionMaps.Direction.Value;
                    float zoom = InputSystem.actionMaps.Scroll.Value * 4;
                    camera.MoveCamera(direction * debugCameraSpeed * timeDelta);
                    camera.AdjustZoom(zoom);
                    break;
            }
        }
    }

    public enum CameraType
    {
        Game,
        Follow,
        Debug
    }
}
