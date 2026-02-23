using Fitamas.Serialization;
using Microsoft.Xna.Framework;

namespace Fitamas.Animation
{
    public class AnimationClip : MonoContentObject
    {
        [SerializeField] private float time;
        [SerializeField] private ITimeLine[] timeLines;

        public float Lenght => time;
        public ITimeLine[] TimeLines => timeLines;

        public AnimationClip(string name, float time, ITimeLine[] timeLines)
        {
            this.time = time;
            this.timeLines = timeLines;
        }
    }
}
