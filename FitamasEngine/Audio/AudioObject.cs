using Microsoft.Xna.Framework;
using SoLoud;
using System;

namespace Fitamas.Audio
{
    public abstract class AudioObject : IDisposable
    {
        private AudioGroup group;
        private float volume;
        private bool looping;
        private double loopPoint;
        private float pan;
        private float speed;
        private bool isPaused;
        private Vector3 position;
        private Vector3 velocity;
        private float minDistance;
        private float maxDistance;
        private AudioAttenuationModel attenuationModel;
        private float attenuationRolloffFactor;
        private float dopplerFactor;

        internal AudioManager Manager;
        internal uint SourceId;
        internal bool HasSourceId;

        internal abstract SoloudObject SoloudObject { get; }

        public bool IsDisposed { get; private set; }
        public AudioSourceInstance Source { get; set; }
        public bool Is3d { get; set; }

        public AudioGroup VolumeGroup
        {
            get
            {
                return group;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                group = value;
                group.AddInstance(this);
            }
        }

        public float Volume
        {
            get
            {
                return volume;
            }
            set
            {
                volume = value;
                if (IsValid())
                {
                    Manager.Soloud.setVolume(SourceId, value);
                }
            }
        }

        public bool Looping
        {
            get
            {
                return looping;
            }
            set
            {
                looping = value;
                if (IsValid())
                {
                    Manager.Soloud.setLooping(SourceId, value ? 1 : 0);
                }
            }
        }

        public double LoopPoint
        {
            get
            {
                return loopPoint;
            }
            set
            {
                loopPoint = value;
                if (IsValid())
                {
                    Manager.Soloud.setLoopPoint(SourceId, value);
                }
            }
        }

        public uint LoopCount
        {
            get
            {
                return Manager.Soloud.getLoopCount(SourceId);
            }
        }

        public float Pan
        {
            get
            {
                return pan;
            }
            set
            {
                pan = value;
                if (IsValid())
                {
                    Manager.Soloud.setPan(SourceId, value);
                }
            }
        }

        public float Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
                if (IsValid())
                {
                    Manager.Soloud.setRelativePlaySpeed(SourceId, value);
                }
            }
        }

        public bool IsPaused
        {
            get
            {
                return isPaused;
            }
            set
            {
                isPaused = value;
                if (IsValid())
                {
                    Manager.Soloud.setPause(SourceId, value ? 1 : 0);
                }
            }
        }

        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                if (IsValid() && Is3d)
                {
                    Manager.Soloud.set3dSourcePosition(SourceId, value.X, value.Y, value.Z);
                }
            }
        }

        public Vector3 Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
                if (IsValid() && Is3d)
                {
                    Manager.Soloud.set3dSourceVelocity(SourceId, value.X, value.Y, value.Z);
                }
            }
        }

        public float MinDistance
        {
            get
            {
                return minDistance;
            }
            set
            {
                minDistance = value;
                if (IsValid())
                {
                    Manager.Soloud.set3dSourceMinMaxDistance(SourceId, minDistance, maxDistance);
                }
            }
        }

        public float MaxDistance
        {
            get
            {
                return maxDistance;
            }
            set
            {
                maxDistance = value;
                if (IsValid())
                {
                    Manager.Soloud.set3dSourceMinMaxDistance(SourceId, minDistance, maxDistance);
                }
            }
        }

        public AudioAttenuationModel AttenuationModel
        {
            get
            {
                return attenuationModel;
            }
            set
            {
                attenuationModel = value;
                if (IsValid())
                {
                    Manager.Soloud.set3dSourceAttenuation(SourceId, (uint)value, attenuationRolloffFactor);
                }
            }
        }

        public float AttenuationRolloffFactor
        {
            get
            {
                return attenuationRolloffFactor;
            }
            set
            {
                attenuationRolloffFactor = value;
                if (IsValid())
                {
                    Manager.Soloud.set3dSourceAttenuation(SourceId, (uint)attenuationModel, value);
                }
            }
        }

        public float DopplerFactor
        {
            get
            {
                return dopplerFactor;
            }
            set
            {
                dopplerFactor = value;
                if (IsValid())
                {
                    Manager.Soloud.set3dSourceDopplerFactor(SourceId, dopplerFactor);
                }
            }
        }

        public AudioObject(AudioManager manager)
        {
            this.Manager = manager;
            volume = 1;
            looping = false;
            loopPoint = 0;
            pan = 0;
            speed = 1;
            isPaused = false;
            minDistance = 1;
            maxDistance = 10;
            attenuationModel = AudioAttenuationModel.NoAttenuation;
            attenuationRolloffFactor = 0;
            dopplerFactor = 1;
        }

        public virtual void Play()
        {
            AudioState state = GetState();

            switch (state)
            {
                case AudioState.Playing:
                    return;
                case AudioState.Paused:
                    Resume();
                    return;
            }

            HasSourceId = true;
            if (Source == null)
            {
                SourceId = Play(this);
            }
            else
            {
                SourceId = Source.Play(this);
            }

            Manager.Soloud.setVolume(SourceId, volume);
            Manager.Soloud.setLooping(SourceId, looping ? 1 : 0);
            Manager.Soloud.setLoopPoint(SourceId, loopPoint);
            Manager.Soloud.setPan(SourceId, pan);
            Manager.Soloud.setRelativePlaySpeed(SourceId, speed);
            Manager.Soloud.setPause(SourceId, isPaused ? 1 : 0);

            if (Is3d)
            {
                Manager.Soloud.set3dSourceMinMaxDistance(SourceId, minDistance, maxDistance);
                Manager.Soloud.set3dSourceAttenuation(SourceId, (uint)attenuationModel, attenuationRolloffFactor);
                Manager.Soloud.set3dSourceDopplerFactor(SourceId, dopplerFactor);
            }
        }

        protected virtual uint Play(AudioObject audioObject)
        {
            if (audioObject.Is3d)
            {
                Vector3 position = audioObject.Position;
                Vector3 velocity = audioObject.Velocity;
                return Manager.Soloud.play3d(audioObject.SoloudObject, position.X, position.Y, position.Z, velocity.X, velocity.Y, velocity.Z);
            }
            else
            {
                return Manager.Soloud.play(audioObject.SoloudObject);
            }
        }

        public virtual void Resume()
        {
            if (IsValid())
            {
                Manager.Soloud.setPause(SourceId, 0);
            }
        }

        public virtual void Pause()
        {
            if (IsValid())
            {
                Manager.Soloud.setPause(SourceId, 1);
            }
        }

        public virtual void Stop()
        {
            if (IsValid())
            {
                Manager.Soloud.stop(SourceId);
            }
        }

        public AudioState GetState()
        {
            if (Manager.Soloud.isValidVoiceHandle(SourceId) == 0)
            {
                return AudioState.Stopped;
            }

            if (Manager.Soloud.getPause(SourceId) != 0)
            {
                return AudioState.Paused;
            }

            return AudioState.Playing;
        }

        public bool IsValid()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(AudioObject));
            }

            return Manager.Soloud.isValidVoiceHandle(SourceId) != 0;
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                Stop();
            }
        }
    }
}
