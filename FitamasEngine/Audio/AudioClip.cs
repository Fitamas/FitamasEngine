using Fitamas.Serialization;
using SoLoud;
using System;

namespace Fitamas.Audio
{
    public class AudioClip : MonoObject
    {
        internal Wav Wav { get; }

        public AudioClip(string path)
        {
            Wav = new Wav();
            Wav.load(path);
        }
    }
}
