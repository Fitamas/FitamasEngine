using Fitamas.Core;
using Fitamas.Events;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Fitamas.Tweening
{
    public class TweenManager : Core.IUpdateable
    {
        private List<Tween> activeTweens;

        public MonoAction<Tween> OnAddTween;
        public MonoAction<Tween> OnRemoveTween;

        public TweenManager(Game game)
        {
            activeTweens = new List<Tween>();
        }

        public void AddActive(Tween tween)
        {
            if (!activeTweens.Contains(tween))
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

        public void Update(GameTime gameTime)
        {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (var i = activeTweens.Count - 1; i >= 0; i--)
            {
                Tween tween = activeTweens[i];

                tween.Update(elapsedSeconds);

                if (tween.IsFinished)
                {
                    activeTweens.RemoveAt(i);
                    OnRemoveTween?.Invoke(tween);
                }
            }
        }
    }
}
