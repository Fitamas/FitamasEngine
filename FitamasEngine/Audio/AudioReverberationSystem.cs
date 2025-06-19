using Fitamas.DebugTools;
using Fitamas.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Fitamas.Audio
{
    public class AudioReverberationSystem : EntityUpdateSystem, IDrawGizmosSystem
    {
        private ComponentMapper<AudioSource> sourceMapper;
        private ComponentMapper<AudioReverbZone> reverbZoneMapper;
        private ComponentMapper<Transform> transformMapper;

        public AudioReverberationSystem() : base(Aspect.All(typeof(Transform)).One(typeof(AudioSource), typeof(AudioReverbZone)))
        {

        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            sourceMapper = mapperService.GetMapper<AudioSource>();
            reverbZoneMapper = mapperService.GetMapper<AudioReverbZone>();
            transformMapper = mapperService.GetMapper<Transform>();
        }

        public override void Update(GameTime gameTime)
        {
            List<(Transform, AudioSource)> sources = new List<(Transform, AudioSource)>();
            List<(Transform, AudioReverbZone)> reverbZones = new List<(Transform, AudioReverbZone)>();

            foreach (var entityId in ActiveEntities)
            {
                Transform transform = transformMapper.Get(entityId);

                if (sourceMapper.TryGet(entityId, out AudioSource source))
                {
                    sources.Add((transform, source));
                }

                if (reverbZoneMapper.TryGet(entityId, out AudioReverbZone reverbZone))
                {
                    reverbZones.Add((transform, reverbZone));
                }
            }

            foreach (var source in sources)
            {
                AudioReverbZone result = null;
                foreach (var reverbZone in reverbZones)
                {
                    if (reverbZone.Item2.MaxDistance > Vector2.Distance(source.Item1.Position, reverbZone.Item1.Position))
                    {
                        result = reverbZone.Item2;
                    }
                }

                if (source.Item2.ReverbZone != result)
                {
                    if (source.Item2.ReverbZone != null)
                    {
                        source.Item2.Group.RemoveFilter(source.Item2.ReverbZone.Fiter);
                        source.Item2.ReverbZone = null;
                    }

                    if (result != null)
                    {
                        source.Item2.Group.AddFilter(result.Fiter);
                        source.Item2.ReverbZone = result;
                    }
                }
            }
        }

        public void DrawGizmos()
        {
            foreach (var entityId in ActiveEntities)
            {
                if (reverbZoneMapper.TryGet(entityId, out AudioReverbZone reverbZone))
                {
                    Transform transform = transformMapper.Get(entityId);

                    Gizmos.DrawCircle(transform.Position, reverbZone.MinDistance, Color.White);
                    Gizmos.DrawCircle(transform.Position, reverbZone.MaxDistance, Color.White);
                }
            }
        }
    }
}
