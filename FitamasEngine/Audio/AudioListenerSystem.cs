using Fitamas.Core;
using Fitamas.Entities;
using Fitamas.Math2D;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Audio
{
    public class AudioListenerSystem : EntityUpdateSystem
    {
        private AudioManager manager;

        private ComponentMapper<Transform> transformMapper;
        private ComponentMapper<AudioListener> listenerMapper;

        public AudioListenerSystem(AudioManager manager) : base(Aspect.All(typeof(Transform), typeof(AudioListener)))
        {
            this.manager = manager;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform>();
            listenerMapper = mapperService.GetMapper<AudioListener>();
            listenerMapper.OnPut += PutListener;
        }

        private void PutListener(int entityId)
        {
            AudioListener listener = listenerMapper.Get(entityId);
            manager.Listener = listener;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entityId in ActiveEntities)
            {
                AudioListener listener = listenerMapper.Get(entityId);
                Transform transform = transformMapper.Get(entityId);
                Vector3 position = transform.Position.ToXYZ();

                listener.Velocity = position - listener.Position;
                listener.Position = position;
            }
        }
    }
}
