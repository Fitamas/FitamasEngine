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
using Fitamas.Math;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Tweening
{
    public class Tween<T> : Tween  where T : struct
    {
        private Func<T, T, float, T> interpolateFunction;
        private MonoCallBack<T> getValueFunction;
        private MonoAction<T> setVelueFunction;

        internal Tween(Func<T, T, float, T> interpolateFunction, MonoCallBack<T> getValueFunction, MonoAction<T> setVelueFunction, T endValue, float duration, float delay) : base(duration, delay)
        {
            this.interpolateFunction = interpolateFunction;
            this.getValueFunction = getValueFunction;
            this.setVelueFunction = setVelueFunction;
            this.endValue = endValue;
        }

        protected T startValue;
        protected T endValue;

        protected override void OnInitialized()
        {
            startValue = getValueFunction.Invoke();
        }

        protected override void Interpolate(float n)
        {
            T value = interpolateFunction.Invoke(startValue, endValue, n);
            setVelueFunction.Invoke(value);
        }

        protected override void Swap()
        {
            endValue = startValue;
            OnInitialized();
        }

        public Tween From(T value)
        {
            startValue = value;
            return this;
        }
    }

    public abstract class Tween
    {
        private Func<float, float> easingFunction;
        private bool isInitialized;
        private float completion;
        private float elapsedDuration;
        private float remainingDelay;
        private float repeatDelay;
        private int remainingRepeats;
        private MonoAction<Tween> onBegin;
        private MonoAction<Tween> onPlay;
        private MonoAction<Tween> onPause;
        private MonoAction<Tween> onComplete;
        private MonoAction<Tween> onStepComplete;
        private MonoAction<Tween> onEnd;

        public float Duration { get; }
        public float Delay { get; }
        public bool IsPaused { get; set; }
        public bool IsRepeating => remainingRepeats != 0;
        public bool IsRepeatingForever => remainingRepeats < 0;
        public bool IsAutoReverse { get; private set; }
        public bool IsAlive { get; private set; }
        public bool IsComplete { get; private set; }
        public float TimeRemaining => Duration - elapsedDuration;
        public float Completion => MathV.Clamp01(completion);

        internal Tween(float duration, float delay)
        {
            Duration = duration;
            Delay = delay;
            IsAlive = true;
            remainingDelay = delay;
        }

        public Tween Easing(Func<float, float> easingFunction) 
        { 
            this.easingFunction = easingFunction; 
            return this; 
        }

        public Tween OnBegin(MonoAction<Tween> action) 
        { 
            onBegin = action; 
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

        public Tween OnEnd(MonoAction<Tween> action)
        {
            onEnd = action;
            return this;
        }

        public Tween Play()
        {
            IsPaused = false;
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
            remainingRepeats = count;
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

        protected abstract void OnInitialized();

        protected abstract void Interpolate(float n);

        protected abstract void Swap();

        public void Cancel()
        {
            remainingRepeats = 0;
            IsAlive = false;
            onEnd?.Invoke(this);
        }

        public void CancelAndComplete()
        {
            if (IsAlive)
            {
                completion = 1;

                Interpolate(1);
                IsComplete = true;
                onComplete?.Invoke(this);
            }

            Cancel();
        }

        internal void Update(float elapsedSeconds)
        {
            if (IsPaused || !IsAlive)
            {
                return;
            }

            if (remainingDelay > 0)
            {
                remainingDelay -= elapsedSeconds;

                if (remainingDelay > 0)
                {
                    return;
                }  
            }

            if (!isInitialized)
            {
                isInitialized = true;
                OnInitialized();
                onBegin?.Invoke(this);
            }

            if (IsComplete)
            {
                IsComplete = false;
                elapsedDuration = 0;
                onPlay?.Invoke(this);

                if (IsAutoReverse)
                {
                    Swap();
                }
            }

            elapsedDuration += elapsedSeconds;

            var n = completion = elapsedDuration / Duration;

            if (easingFunction != null)
            {
                n = easingFunction.Invoke(n);
            }

            if (elapsedDuration >= Duration)
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
                    IsAlive = false;
                }

                n = completion = 1;
                IsComplete = true;
            }

            Interpolate(n);

            if (IsComplete)
            {
                onStepComplete?.Invoke(this);

                if (!IsAlive)
                {
                    onComplete?.Invoke(this);
                }
            } 
        }
    }
}
