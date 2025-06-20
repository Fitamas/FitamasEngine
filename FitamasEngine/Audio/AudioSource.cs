using Fitamas.ECS;
using System;

namespace Fitamas.Audio
{
    public class AudioSource : Component
    {
        public AudioClip Clip;
        public bool PlayOnAwake = false;
        public float Volume = 1;
        public bool Looping = false;
        public double LoopPoint = 0;
        public float Pan = 0;
        public float Speed = 1;
        public bool Is3d = false;
        public float MinDistance = 1;
        public float MaxDistance = 10;
        public AudioAttenuationModel AttenuationModel = AudioAttenuationModel.NoAttenuation;
        public float AttenuationRolloffFactor = 0;
        public float DopplerFactor = 1;

        public AudioSourceBusInstance Group { get; set; }
        public AudioClipInstance Instance { get; set; }
        public AudioReverbZone ReverbZone { get; set; }
    }
}
