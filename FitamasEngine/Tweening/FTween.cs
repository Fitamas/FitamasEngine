using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Tweening
{
    public static class FTween
    {
        public static Tweener<float> To(Setter<float> setVelueFunction, float from, float to, float duration)
        {
            return To(() => from, setVelueFunction, to, duration);
        }

        public static Tweener<float> To(Getter<float> getValueFunction, Setter<float> setVelueFunction, float endValue, float duration)
        {
            Tweener<float> tween = new Tweener<float>(float.Lerp, getValueFunction, setVelueFunction, endValue, duration);
            return tween;
        }

        public static Tweener<Vector2> To(Setter<Vector2> setVelueFunction, Vector2 from, Vector2 to, float duration)
        {
            return To(() => from, setVelueFunction, to, duration);
        }

        public static Tweener<Vector2> To(Getter<Vector2> getValueFunction, Setter<Vector2> setVelueFunction, Vector2 endValue, float duration)
        {
            Tweener<Vector2> tween = new Tweener<Vector2>(Vector2.Lerp, getValueFunction, setVelueFunction, endValue, duration);
            return tween;
        }

        public static Tweener<Color> To(Setter<Color> setVelueFunction, Color from, Color to, float duration)
        {
            return To(() => from, setVelueFunction, to, duration);
        }

        public static Tweener<Color> To(Getter<Color> getValueFunction, Setter<Color> setVelueFunction, Color endValue, float duration)
        {
            Tweener<Color> tween = new Tweener<Color>(Color.Lerp, getValueFunction, setVelueFunction, endValue, duration);
            return tween;
        }
    }
}
