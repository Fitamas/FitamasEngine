using SoLoud;
using System;

namespace Fitamas.Audio.Filters
{
    public class AudioFFTFilter : AudioFilter
    {
        internal FFTFilter Filter;

        internal override SoloudObject SoloudObject => Filter;

        public AudioFFTFilter()
        {
            Filter = new FFTFilter();
        }
    }
}
