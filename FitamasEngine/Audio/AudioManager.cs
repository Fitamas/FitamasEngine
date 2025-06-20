using Fitamas.Core;
using Microsoft.Xna.Framework;
using SoLoud;
using System;

namespace Fitamas.Audio
{
    public class AudioManager : IDisposable
    {
        internal static readonly SoloudObject NullObject = new SoloudObject() { objhandle = IntPtr.Zero };

        internal Soloud Soloud { get; }

        public bool IsDisposed { get; private set; }

        public AudioManager(GameEngine game)
        {
            Soloud = new Soloud();
            Soloud.init();

            game.Components.Add(new AudioComponent(game, this));
        }

        ~AudioManager()
        {
            Dispose();
        }

        public void Play(AudioClip clip)
        {
            Soloud.play(clip.Wav);
        }

        public void Play3d(AudioClip clip, Vector3 position)
        {
            Soloud.play3d(clip.Wav, position.X, position.Y, position.Z);
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                Soloud.deinit();
            }
        }
    }

    public class AudioComponent : GameComponent
    {
        private AudioManager manager;

        public AudioComponent(Game game, AudioManager manager) : base(game)
        {
            this.manager = manager;
        }

        public override void Update(GameTime gameTime)
        {
            manager.Soloud.update3dAudio();
        }
    }
}
