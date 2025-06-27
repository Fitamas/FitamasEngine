using Fitamas.Core;
using Microsoft.Xna.Framework;
using SoLoud;
using System;

namespace Fitamas.Audio
{
    public class AudioManager : GameComponent
    {
        internal static readonly SoloudObject NullObject = new SoloudObject() { objhandle = IntPtr.Zero };

        internal Soloud Soloud { get; }

        public bool IsDisposed { get; private set; }

        public AudioManager(Game game) : base(game)
        {
            Soloud = new Soloud();
            Soloud.init();
        }

        ~AudioManager()
        {
            Dispose();
        }

        public override void Update(GameTime gameTime)
        {
            Soloud.update3dAudio();
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
}
