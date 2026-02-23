using Fitamas.Events;
using Fitamas.Math;
using System;

namespace Fitamas.Tweening
{
    public delegate T Getter<out T>();

    public delegate void Setter<T>(T arg);

    public class Tweener<T> : Tween  where T : struct
    {
        private Func<float, float> easingFunction;
        private Func<T, T, float, T> interpolateFunction;
        private Getter<T> getValueFunction;
        private Setter<T> setVelueFunction;

        private float completion;

        public float Duration { get; }

        public float TimeRemaining => Duration - ElapsedDuration;
        public float Completion => FMath.Clamp01(completion);

        public Tweener(Func<T, T, float, T> interpolateFunction, Getter<T> getValueFunction, Setter<T> setVelueFunction, T endValue, float duration)
        {
            this.interpolateFunction = interpolateFunction;
            this.getValueFunction = getValueFunction;
            this.setVelueFunction = setVelueFunction;
            this.endValue = endValue;
            Duration = duration;
        }

        protected T startValue;
        protected T endValue;

        protected override void OnStart()
        {
            startValue = getValueFunction.Invoke();
        }

        protected void Interpolate(float n)
        {
            T value = interpolateFunction.Invoke(startValue, endValue, n);
            setVelueFunction.Invoke(value);
        }

        protected override void Swap()
        {
            endValue = startValue;
            OnStart();
        }

        public Tweener<T> From(T value)
        {
            startValue = value;
            return this;
        }

        public Tweener<T> Easing(Func<float, float> easingFunction)
        {
            this.easingFunction = easingFunction;
            return this;
        }

        protected override void OnComplete()
        {
            completion = 1;
            Interpolate(1);
        }

        protected override void OnUpdate(float deltaTime)
        {
            var n = completion = ElapsedDuration / Duration;

            if (easingFunction != null)
            {
                n = easingFunction.Invoke(n);
            }

            if (ElapsedDuration >= Duration)
            {
                n = completion = 1;
                Complete();
            }

            Interpolate(n);
        }
    }
}
