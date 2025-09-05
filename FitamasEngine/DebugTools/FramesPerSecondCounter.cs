using Microsoft.Xna.Framework;
using System;

namespace Fitamas.DebugTools
{
    public class FramesPerSecondCounter
    {
        public static FramesPerSecondCounter Instance { get; private set; }

        private static readonly TimeSpan _oneSecondTimeSpan = new TimeSpan(0, 0, 1);
        private int _framesCounter;
        private TimeSpan _timer = _oneSecondTimeSpan;

        public FramesPerSecondCounter()
        {
            Instance = this;
        }

        public int FramesPerSecond { get; private set; }

        public void Update(GameTime gameTime)
        {
            _timer += gameTime.ElapsedGameTime;
            if (_timer <= _oneSecondTimeSpan)
                return;

            FramesPerSecond = _framesCounter;
            _framesCounter = 0;
            _timer -= _oneSecondTimeSpan;
        }

        public void Draw(GameTime gameTime)
        {
            _framesCounter++;
        }
    }
}
