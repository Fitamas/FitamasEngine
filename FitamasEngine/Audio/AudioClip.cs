using Fitamas.Serialization;
using SoLoud;
using System;
using System.IO;

namespace Fitamas.Audio
{
    public class AudioClip : MonoContentObject
    {
        private Wav wav;

        internal Wav Wav => wav;
        public double Lenght => wav.getLength();

        private AudioClip()
        {
            wav = new Wav();
        }

        public override void LoadData(string path)
        {
            wav = new Wav();;
            wav.load(path);
        }
    }
}
