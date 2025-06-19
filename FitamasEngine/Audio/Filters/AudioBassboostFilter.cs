using SoLoud;
using System;

namespace Fitamas.Audio.Filters
{
    public class AudioBassboostFilter : AudioFilter
    {
        private float boost;

        internal BassboostFilter Filter;

        internal override SoloudObject SoloudObject => Filter;

        public float Boost
        {
            get
            {
                return boost;
            }
            set
            {
                boost = value;
                SetParameter(BassboostFilter.BOOST, boost);
            }
        }

        public AudioBassboostFilter()
        {
            Filter = new BassboostFilter();

            boost = 0;
        }

        protected override void OnSetSource(AudioSourceInstance source)
        {
            manager.Soloud.setFilterParameter(sourceId, filterId, BassboostFilter.BOOST, boost);
        }
    }
}
