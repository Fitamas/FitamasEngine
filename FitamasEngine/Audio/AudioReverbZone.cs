using Fitamas.Audio.Filters;
using Fitamas.Entities;
using System;

namespace Fitamas.Audio
{
    public class AudioReverbZone : Component
    {
        public AudioFreeverbFilter Fiter { get; }

        public float MinDistance = 1;
        public float MaxDistance = 10;

        public AudioReverbZone()
        {
            Fiter = new AudioFreeverbFilter();
        }
    }
}
