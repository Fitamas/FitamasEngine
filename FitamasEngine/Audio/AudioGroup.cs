using System;

namespace Fitamas.Audio
{
    public class AudioGroup : IDisposable
    {
        private AudioManager manager;

        internal uint SourceId;

        public bool IsDisposed { get; private set; }

        public float Volume
        {
            get
            {
                if (IsDisposed)
                {
                    throw new ObjectDisposedException(nameof(AudioGroup));
                }

                return manager.Soloud.getVolume(SourceId);
            }
            set
            {
                if (IsDisposed)
                {
                    throw new ObjectDisposedException(nameof(AudioGroup));
                }

                manager.Soloud.setVolume(SourceId, value);
            }
        }

        public AudioGroup(AudioManager manager)
        {
            this.manager = manager;

            SourceId = manager.Soloud.createVoiceGroup();
        }

        internal void AddInstance(AudioObject audioObject)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(AudioGroup));
            }

            if (!audioObject.HasSourceId)
            {
                throw new Exception("The object must be played");
            }

            manager.Soloud.addVoiceToGroup(SourceId, audioObject.SourceId);
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                manager.Soloud.destroyVoiceGroup(SourceId);
            }
        }
    }
}
