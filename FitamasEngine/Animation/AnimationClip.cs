using Fitamas.Serialization;
using Microsoft.Xna.Framework;

namespace Fitamas.Animation
{
    public class AnimationClip : MonoContentObject
    {
        [SerializeField] private string name;
        [SerializeField] private float time;
        [SerializeField] private ITimeLine[] timeLines;

        public string Name => name;
        public float Lenght => time;
        public ITimeLine[] TimeLines => timeLines;

        public AnimationClip(string name, float time, ITimeLine[] timeLines)
        {
            this.name = name;
            this.time = time;
            this.timeLines = timeLines;
        }
    }
}
