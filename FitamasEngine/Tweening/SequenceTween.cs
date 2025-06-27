using System;
using System.Collections.Generic;

namespace Fitamas.Tweening
{
    public class SequenceTween : Tween
    {
        private List<Tween> tweens;
        private List<Tween> playingTweens;

        public IReadOnlyList<Tween> Tweens => tweens;

        public SequenceTween()
        {
            tweens = new List<Tween>();
            playingTweens = new List<Tween>();
        }

        protected override void OnPlay()
        {
            playingTweens.Clear();
            playingTweens.AddRange(tweens);

            if (playingTweens.Count > 0)
            {
                Tween tween = playingTweens[0];
                tween.Reset();
            }
            else
            {
                Complete();
            }
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (playingTweens.Count == 0)
            {
                Complete();
                return;
            }

            Tween tween = playingTweens[0];
            tween.Update(deltaTime);

            if (tween.IsFinished)
            {
                playingTweens.RemoveAt(0);

                if (playingTweens.Count > 0)
                {
                    Tween nextTween = playingTweens[0];
                    nextTween.Reset();
                }
            }
        }

        protected override void OnReset()
        {
            foreach (var tween in tweens)
            {
                tween.Reset();
            }
        }

        public SequenceTween Append(Tween tween)
        {
            tweens.Add(tween);

            return this;
        }

        public SequenceTween Join(Tween tween)
        {
            if (tweens.Count > 0)
            {
                if (tweens[tweens.Count - 1] is GroupTween group)
                {
                    group.Append(tween);
                }
                else
                {
                    Tween lastTween = tweens[tweens.Count - 1];
                    tweens.RemoveAt(tweens.Count - 1);
                    group = new GroupTween();
                    group.Append(lastTween);
                    group.Append(tween);
                    Append(group);
                }
            }
            else
            {
                tweens.Add(tween);
            }

            return this;
        }

        public SequenceTween Remove(Tween tween)
        {
            tweens.Remove(tween);

            return this;
        }
    }
}
