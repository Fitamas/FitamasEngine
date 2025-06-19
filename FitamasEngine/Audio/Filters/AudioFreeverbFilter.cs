using SoLoud;
using System;

namespace Fitamas.Audio.Filters
{
    public class AudioFreeverbFilter : AudioFilter
    {
        private float roomSize;
        private float damp;
        private float wigth;

        internal FreeverbFilter Filter;

        internal override SoloudObject SoloudObject => Filter;

        public float RoomSize
        {
            get
            {
                return roomSize;
            }
            set
            {
                roomSize = value;
                SetParameter(FreeverbFilter.ROOMSIZE, roomSize);
            }
        }

        public float Damp
        {
            get
            {
                return damp;
            }
            set
            {
                damp = value;
                SetParameter(FreeverbFilter.DAMP, damp);
            }
        }

        public float Width
        {
            get
            {
                return wigth;
            }
            set
            {
                wigth = value;
                SetParameter(FreeverbFilter.WIDTH, wigth);
            }
        }

        public AudioFreeverbFilter()
        {
            Filter = new FreeverbFilter();

            roomSize = 0;
            damp = 0;
            wigth = 0;
        }

        protected override void OnSetSource(AudioSourceInstance source)
        {
            SetParameter(FreeverbFilter.ROOMSIZE, roomSize);
            SetParameter(FreeverbFilter.DAMP, damp);
            SetParameter(FreeverbFilter.WIDTH, wigth);
        }
    }
}
