using System;

namespace Fitamas.Audio
{
    public enum AudioAttenuationModel : uint
    {
        NoAttenuation = 0,
        InverseDistance = 1,
        LinearDistance = 2,
        ExponentialDistance = 3,
    }
}
