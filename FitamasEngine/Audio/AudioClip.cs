using Fitamas.Serialization;
using SoLoud;
using System;
using System.IO;

namespace Fitamas.Audio
{
    public class AudioClip : MonoContentObject
    {
        internal Wav Wav { get; }

        private AudioClip(Wav wav)
        {
            Wav = wav;
        }

        public static AudioClip LoadWav(string path)
        {
            path = Path.Combine(Resources.RootDirectory, path);
            Wav wav = new Wav();
            wav.load(path);

            return new AudioClip(wav);
        }
    }
}
