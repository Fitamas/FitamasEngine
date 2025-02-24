using System;

namespace Fitamas.Time
{
    public class Timer
    {
        private TimeSpan startTime;
        private float time;

        public Timer() 
        {
            
        }

        public Timer(float time)
        {
            this.time = time;
        }

        public void Start(TimeSpan startTime)
        {
            this.startTime = startTime;
        }

        public void Start(TimeSpan startTime, float time)
        {
            this.startTime = startTime;
            this.time = time;
        }

        public bool IsTimerEnd(TimeSpan currentTime)
        {
            return currentTime.TotalSeconds > startTime.TotalSeconds + time;
        }

        public void SetTime(float time)
        {
            this.time = time;
        }
    }
}
