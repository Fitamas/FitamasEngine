using Fitamas.DebugTools;
using Fitamas.ECS;
using Fitamas.Math;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Audio
{
    public class Audio3dSystem : EntityUpdateSystem, IDrawGizmosSystem
    {
        private ComponentMapper<AudioSource> sourceMapper;
        private ComponentMapper<Transform> transformMapper;

        public Audio3dSystem() : base(Aspect.All(typeof(AudioSource), typeof(Transform)))
        {

        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            sourceMapper = mapperService.GetMapper<AudioSource>();
            transformMapper = mapperService.GetMapper<Transform>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entityId in ActiveEntities)
            {
                AudioSource source = sourceMapper.Get(entityId);
                Transform transform = transformMapper.Get(entityId);

                if (source.Is3d)
                {
                    Vector3 oldPosition = source.Instance.Position;
                    Vector3 newPosition = transform.Position.ToXYZ();
                    source.Instance.Position = newPosition;
                    source.Instance.Velocity = newPosition - oldPosition;
                }
            }
        }

        public void DrawGizmos()
        {
            foreach (var entityId in ActiveEntities)
            {
                AudioSource source = sourceMapper.Get(entityId);
                Transform transform = transformMapper.Get(entityId);

                if (source.Is3d)
                {
                    Gizmos.DrawCircle(transform.Position, source.MinDistance, Color.White);
                    Gizmos.DrawCircle(transform.Position, source.MaxDistance, Color.White);
                }
            }
        }
    }
}
