using Fitamas.ECS;

namespace Fitamas.Tweening
{
    public class TweenComponent : Component
    {
        public Tween Tween { get; }

        public TweenComponent (Tween tween)
        {
            Tween = tween;
        }
    }
}
