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
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Fitamas.Tweening
{
    public class Tweener
    {
        
    }

    public class Sequence : Tween
    {
        private List<Tween> tweens;

        public Sequence(float duration, float delay) : base(duration, delay)
        {
        }

        protected override void Interpolate(float n)
        {
            throw new NotImplementedException();
        }

        protected override void OnInitialized()
        {
            throw new NotImplementedException();
        }

        protected override void Swap()
        {
            throw new NotImplementedException();
        }
    }

    public class TweenManager
    {
        private List<Tween> activeTweens;

        public MonoAction<Tween> OnAddTween;
        public MonoAction<Tween> OnRemoveTween;

        public TweenManager()
        {
            activeTweens = new List<Tween>();
        }

        public void AddActive(Tween tween)
        {
            if (!activeTweens.Contains(tween) && tween.IsAlive)
            {
                activeTweens.Add(tween);
                OnAddTween?.Invoke(tween);
            }
        }

        public void RemoveActive(Tween tween)
        {
            if (activeTweens.Remove(tween))
            {
                OnRemoveTween?.Invoke(tween);
            }
        }

        public void Update(float elapsedSeconds)
        {
            for (var i = activeTweens.Count - 1; i >= 0; i--)
            {
                Tween tween = activeTweens[i];

                tween.Update(elapsedSeconds);

                if (!tween.IsAlive)
                {
                    activeTweens.RemoveAt(i);
                    OnRemoveTween?.Invoke(tween);
                }
            }
        }
    }

    public static class TweenHelper
    {
        public static Tween<float> FloatTween(float from, float to, float duration, float delay, MonoAction<float> setVelueFunction)
        {
            return FloatTween(() => from, setVelueFunction, to, duration, delay);
        }

        public static Tween<float> FloatTween(MonoCallBack<float> getValueFunction, MonoAction<float> setVelueFunction, float endValue, float duration, float delay)
        {
            Tween<float> tween = new Tween<float>(float.Lerp, getValueFunction, setVelueFunction, endValue, duration, delay);
            return tween;
        }

        public static Tween<Vector2> Vector2Tween(Vector2 from, Vector2 to, float duration, float delay, MonoAction<Vector2> setVelueFunction)
        {
            Tween<Vector2> tween = new Tween<Vector2>(Vector2.Lerp, () => from, setVelueFunction, to, duration, delay);
            return tween;
        }

        public static Tween<Color> ColorTween(Color from, Color to, float duration, float delay, MonoAction<Color> setVelueFunction)
        {
            Tween<Color> tween = new Tween<Color>(Color.Lerp, () => from, setVelueFunction, to, duration, delay);
            return tween;
        }
    }
}
