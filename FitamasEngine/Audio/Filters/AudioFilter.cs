using Newtonsoft.Json.Linq;
using SoLoud;
using System;

namespace Fitamas.Audio.Filters
{
    public abstract class AudioFilter
    {
        protected AudioSourceInstance source;
        private float wet = 1;

        internal abstract SoloudObject SoloudObject { get; }

        protected AudioManager manager => source.Manager;
        protected uint sourceId => source.SourceId;
        protected uint filterId => (uint)source.filters.IndexOf(this);

        public float Wet
        {
            get
            {
                return wet;
            }
            set
            {
                wet = value;
                SetParameter(0, wet);
            }
        }

        internal void SetSource(AudioSourceInstance source)
        {
            this.source = source;
            SetParameter(0, wet);
            OnSetSource(source);
        }

        protected virtual void OnSetSource(AudioSourceInstance source)
        {

        }

        protected void SetParameter(uint attributeId, float value)
        {
            if (source != null)
            {
                manager.Soloud.setFilterParameter(sourceId, filterId, attributeId, value);
            }
        }
    }


    //TODO filters

    public class AudioBiquadResonantFilter : AudioFilter
    {
        internal BiquadResonantFilter Filter;

        internal override SoloudObject SoloudObject => Filter;

        public AudioBiquadResonantFilter()
        {
            Filter = new BiquadResonantFilter();
        }
    }

    public class AudioDCRemovalFilter : AudioFilter
    {
        internal DCRemovalFilter Filter;

        internal override SoloudObject SoloudObject => Filter;

        public AudioDCRemovalFilter()
        {
            Filter = new DCRemovalFilter();
        }
    }

    public class AudioEchoFilter : AudioFilter
    {
        internal EchoFilter Filter;

        internal override SoloudObject SoloudObject => Filter;

        public AudioEchoFilter()
        {
            Filter = new EchoFilter();
        }
    }

    public class AudioFlangerFilter : AudioFilter
    {
        internal FlangerFilter Filter;

        internal override SoloudObject SoloudObject => Filter;

        public AudioFlangerFilter()
        {
            Filter = new FlangerFilter();
        }
    }

    public class AudioRobotizeFilter : AudioFilter
    {
        internal RobotizeFilter Filter;

        internal override SoloudObject SoloudObject => Filter;

        public AudioRobotizeFilter()
        {
            Filter = new RobotizeFilter();
        }
    }

    public class AudioWaveShaperFilter : AudioFilter
    {
        internal WaveShaperFilter Filter;

        internal override SoloudObject SoloudObject => Filter;

        public AudioWaveShaperFilter()
        {
            Filter = new WaveShaperFilter();
        }
    }

    public class AudioLofiFilter : AudioFilter
    {
        internal LofiFilter Filter;

        internal override SoloudObject SoloudObject => Filter;

        public AudioLofiFilter()
        {
            Filter = new LofiFilter();
        }
    }
}
