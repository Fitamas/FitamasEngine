using SoLoud;
using System;

namespace Fitamas.Audio
{
    public class AudioClipInstance : AudioObject
    {
        internal AudioClip Clip;

        internal override SoloudObject SoloudObject => Clip.Wav;

        public AudioClipInstance(AudioManager manager, AudioClip clip) : base(manager)
        {
            Clip = clip;
        }
    }
}