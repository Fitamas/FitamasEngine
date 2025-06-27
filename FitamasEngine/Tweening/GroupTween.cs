using System;
using System.Collections.Generic;

namespace Fitamas.Tweening
{
    public class GroupTween : Tween
    {
        private List<Tween> tweens;
        private List<Tween> playingTweens;

        public IReadOnlyList<Tween> Tweens => tweens;

        public GroupTween()
        {
            tweens = new List<Tween>();
            playingTweens = new List<Tween>();
        }

        protected override void OnPlay()
        {
            playingTweens.Clear();
            playingTweens.AddRange(tweens);

            for (int i = playingTweens.Count - 1; i >= 0; --i)
            {
                Tween tween = playingTweens[i];

                tween.Reset();

                if (tween.IsFinished)
                {
                    playingTweens.RemoveAt(i);
                }
            }

            if (playingTweens.Count == 0)
            {
                Complete();
            }
        }

        protected override void OnUpdate(float deltaTime)
        {
            for (int i = playingTweens.Count - 1; i >= 0; --i)
            {
                Tween tween = playingTweens[i];

                tween.Update(deltaTime);

                if (tween.IsFinished)
                {
                    playingTweens.RemoveAt(i);
                }
            }

            if (playingTweens.Count == 0)
            {
                Complete();
            }
        }

        protected override void OnReset()
        {
            foreach (var tween in tweens)
            {
                tween.Reset();
            }
        }

        public GroupTween Append(Tween tween)
        {
            tweens.Add(tween);

            return this;
        }

        public GroupTween Remove(Tween tween)
        {
            tweens.Remove(tween);

            return this;
        }
    }
}
