using Fitamas.Entities;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Audio
{
    public class AudioSystem : EntityUpdateSystem
    {
        private AudioManager manager;

        private ComponentMapper<AudioSource> sourceMapper;
        private ComponentMapper<AudioRequest> requestMapper;

        public AudioSystem(AudioManager manager) : base(Aspect.All(typeof(AudioSource), typeof(AudioRequest)))
        {
            this.manager = manager;
        }

        public override void Initialize(GameWorld world)
        {
            world.RegisterSystem(new AudioListenerSystem(manager));
            world.RegisterSystem(new Audio3dSystem());
            world.RegisterSystem(new AudioReverberationSystem());

            base.Initialize(world);
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            sourceMapper = mapperService.GetMapper<AudioSource>();
            sourceMapper.OnPut += PutSource;
            requestMapper = mapperService.GetMapper<AudioRequest>();
        }

        private void PutSource(int entityId)
        {
            AudioSource source = sourceMapper.Get(entityId);

            AudioSourceBusInstance sourceBus = new AudioSourceBusInstance(manager);
            sourceBus.Play();
            source.Group = sourceBus;

            AudioClipInstance instance = new AudioClipInstance(manager, source.Clip);
            instance.Source = sourceBus;
            instance.Is3d = source.Is3d;
            instance.Volume = source.Volume;
            instance.Looping = source.Looping;
            instance.LoopPoint = source.LoopPoint;
            instance.Pan = source.Pan;
            instance.Speed = source.Speed;
            instance.MinDistance = source.MinDistance;
            instance.MaxDistance = source.MaxDistance;
            instance.AttenuationModel = source.AttenuationModel;
            instance.AttenuationRolloffFactor = source.AttenuationRolloffFactor;
            instance.DopplerFactor = source.DopplerFactor;
            source.Instance = instance;

            if (source.PlayOnAwake)
            {
                instance.Play();
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entityId in ActiveEntities)
            {
                AudioSource source = sourceMapper.Get(entityId);
                AudioRequest request = requestMapper.Get(entityId);

                switch (request.State)
                {
                    case AudioState.Playing:
                        source.Instance.Play();
                        break;
                    case AudioState.Paused:
                        source.Instance.Pause();
                        break;
                    case AudioState.Stopped:
                        source.Instance.Stop();
                        break;
                }

                requestMapper.Delete(entityId);
            }
        }
    }
}