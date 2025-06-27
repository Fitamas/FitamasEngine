/*
    The MIT License (MIT)

    Copyright (c) 2015-2024:
    - Dylan Wilson (https://github.com/dylanwilson80)
    - Lucas Girouard-Stranks (https://github.com/lithiumtoast)
    - Christopher Whitley (https://github.com/aristurtledev)

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/

using Fitamas.Events;
using System;

namespace Fitamas.Tweening
{
    public abstract class Tween
    {
        private bool isStarted;
        private float elapsedDuration;
        private float repeatDelay;
        private float remainingDelay;
        private int repeats;
        private int remainingRepeats;
        private MonoAction<Tween> onStart;
        private MonoAction<Tween> onPlay;
        private MonoAction<Tween> onPause;
        private MonoAction<Tween> onComplete;
        private MonoAction<Tween> onStepComplete;
        private MonoAction<Tween> onKill;

        public float Delay { get; private set; }
        public bool IsInitialized => isStarted;
        public bool IsRepeating => remainingRepeats != 0;
        public bool IsRepeatingForever => remainingRepeats < 0;
        public bool IsPaused { get; private set; }
        public bool IsAutoReverse { get; private set; }
        public bool IsFinished { get; private set; }
        public bool IsComplete { get; private set; }
        public float RepeatDelay => repeatDelay;
        public float ElapsedDuration => elapsedDuration;
        public int RemainingRepeats => remainingRepeats;

        public Tween OnStart(MonoAction<Tween> action)
        {
            onStart = action;
            return this;
        }

        public Tween OnPlay(MonoAction<Tween> action)
        {
            onPlay = action;
            return this;
        }

        public Tween OnPause(MonoAction<Tween> action)
        {
            onPause = action;
            return this;
        }

        public Tween OnComplete(MonoAction<Tween> action)
        {
            onComplete = action;
            return this;
        }

        public Tween OnStepComplete(MonoAction<Tween> action)
        {
            onStepComplete = action;
            return this;
        }

        public Tween OnKill(MonoAction<Tween> action)
        {
            onKill = action;
            return this;
        }

        public Tween Reset()
        {
            elapsedDuration = 0;
            IsFinished = false;
            remainingRepeats = repeats;
            OnReset();
            return this;
        }

        protected virtual void OnReset()
        {

        }

        public Tween SetDelay(float delay)
        {
            Delay = delay;
            remainingDelay = delay;
            return this;
        }

        public Tween Play()
        {
            IsPaused = false;
            elapsedDuration = 0;
            onPlay?.Invoke(this);
            return this;
        }

        public Tween Pause()
        {
            IsPaused = true;
            onPause?.Invoke(this);
            return this;
        }

        public Tween Repeat(int count, float repeatDelay = 0f)
        {
            repeats = remainingRepeats = count;
            this.repeatDelay = repeatDelay;
            return this;
        }

        public Tween RepeatForever(float repeatDelay = 0f)
        {
            remainingRepeats = -1;
            this.repeatDelay = repeatDelay;
            return this;
        }

        public Tween AutoReverse(bool isAutoReverse = true)
        {
            IsAutoReverse = isAutoReverse;
            return this;
        }

        public void Kill()
        {
            remainingRepeats = 0;
            IsFinished = true;
            IsPaused = true;
            onKill?.Invoke(this);
        }

        public void KillAndComplete()
        {
            if (!IsFinished)
            {
                onComplete?.Invoke(this);
            }

            Kill();
        }

        protected void Complete()
        {
            if (remainingRepeats != 0)
            {
                if (remainingRepeats > 0)
                {
                    remainingRepeats--;
                }

                remainingDelay = repeatDelay;
            }
            else if (remainingRepeats == 0)
            {
                IsFinished = true;
            }

            IsComplete = true;
        }

        internal void Update(float deltaTime)
        {
            if (IsPaused || IsFinished)
            {
                return;
            }

            if (remainingDelay > 0)
            {
                remainingDelay -= deltaTime;

                if (remainingDelay > 0)
                {
                    return;
                }
            }

            if (!isStarted)
            {
                isStarted = true;
                onStart?.Invoke(this);
                OnStart();
                onPlay?.Invoke(this);
                OnPlay();
            }

            if (IsComplete)
            {
                elapsedDuration = 0;
                IsComplete = false;
                onPlay?.Invoke(this);
                OnPlay();

                if (IsAutoReverse)
                {
                    Swap();
                }
            }

            elapsedDuration += deltaTime;

            OnUpdate(deltaTime);

            if (IsComplete)
            {
                onStepComplete?.Invoke(this);
                OnStepComplete();

                if (IsFinished)
                {
                    onComplete?.Invoke(this);
                    OnComplete();
                }
            }
        }

        protected virtual void OnStart()
        {

        }

        protected virtual void OnUpdate(float deltaTime)
        {

        }

        protected virtual void OnPlay()
        {

        }

        protected virtual void OnComplete()
        {

        }

        protected virtual void OnStepComplete()
        {

        }

        protected virtual void Swap()
        {

        }
    }
}
