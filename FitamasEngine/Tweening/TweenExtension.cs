using Fitamas.ECS;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Tweening
{
    public static class TweenExtension
    {
        public static TweenComponent ToComponent(this Tween tween)
        {
            return new TweenComponent(tween);
        }

        public static Tweener<Vector2> Move(this Transform transform, Vector2 to, float duration)
        {
            return FTween.To(() => transform.Position, x => transform.Position = x, to, duration);
        }

        public static Tweener<float> MoveX(this Transform transform, float to, float duration)
        {
            return FTween.To(() => transform.Position.X, x => transform.Position = new Vector2(x, transform.Position.Y), to, duration);
        }

        public static Tweener<float> MoveY(this Transform transform, float to, float duration)
        {
            return FTween.To(() => transform.Position.Y, x => transform.Position = new Vector2(transform.Position.X, x), to, duration);
        }

        public static Tweener<float> Rotate(this Transform transform, float to, float duration)
        {
            return FTween.To(() => transform.Rotation, x => transform.Rotation = x, to, duration);
        }
    }
}
