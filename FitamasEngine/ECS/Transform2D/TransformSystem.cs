using Microsoft.Xna.Framework;
using System;

namespace Fitamas.ECS.Transform2D
{
    public class TransformSystem : EntityUpdateSystem
    {
        private ComponentMapper<Transform> transformMapper;
        private ComponentMapper<TransformMovement> transformMovementMapper;

        public TransformSystem() : base(Aspect.All(typeof(Transform), typeof(TransformMovement)))
        {

        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform>();
            transformMovementMapper = mapperService.GetMapper<TransformMovement>();
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var entityId in ActiveEntities)
            {
                Transform transform = transformMapper.Get(entityId);
                TransformMovement transformMovement = transformMovementMapper.Get(entityId);

                transform.Position += transformMovement.Velocity * deltaTime;
                transform.Rotation += transformMovement.RotationVelocity * deltaTime;
            }
        }
    }
}
