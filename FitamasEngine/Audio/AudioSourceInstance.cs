using Fitamas.Audio.Filters;
using Fitamas.Collections;
using SoLoud;
using System;
using System.Collections.Generic;

namespace Fitamas.Audio
{
    public abstract class AudioSourceInstance : AudioObject
    {
        internal Bag<AudioFilter> filters;

        public IEnumerable<AudioFilter> Filters => filters;

        protected AudioSourceInstance(AudioManager manager) : base(manager)
        {
            filters = new Bag<AudioFilter>();
        }

        protected abstract void SetFilter(uint filterId, SoloudObject soloudObject);

        public void AddFilter(AudioFilter filter)
        {
            if (!filters.Contains(filter))
            {
                for (int i = 0; i < filters.Count; i++)
                {
                    if (filters[i] == null)
                    {
                        SetFilter((uint)i, filter.SoloudObject);
                        filters[i] = filter;
                        return;
                    }
                }

                SetFilter((uint)filters.Count, filter.SoloudObject);
                filters.Add(filter);
                filter.SetSource(this);
            }
        }

        public void RemoveFilter(AudioFilter filter)
        {
            if (filters.Contains(filter))
            {
                SetFilter((uint)filters.IndexOf(filter), AudioManager.NullObject);
                filters.Remove(filter);
            }
        }
    }
}
