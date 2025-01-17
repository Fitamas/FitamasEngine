using Fitamas.Entities;
using Fitamas.Graphics;
using Fitamas.Serializeble;
using Microsoft.Xna.Framework;

namespace Fitamas.Animation
{
    public class AnimationClip : MonoObject
    {
        [SerializableField] private float time;
        [SerializableField] public ITimeLine[] timeLines;

        public string Name;

        public ITimeLine[] TimeLines => timeLines;
        public float Time => time;

        public AnimationClip(ITimeLine[] timeLines, float time, string name)
        {
            this.timeLines = timeLines;
            this.time = time;
            Name = name;
        }
    }

    public struct TransformPositionJob : IJob<Vector2>
    {
        public void Step(FrameData frameData, KeyFrame<Vector2> lastFrame, KeyFrame<Vector2> currentFrame)
        {
            if (frameData.frameId == 0)
            {
                return;
            }

            Transform transform = frameData.entity.Get<Transform>();

            float normolizeTime = (frameData.normolizeTime - lastFrame.normolizeTime) 
                / (currentFrame.normolizeTime - lastFrame.normolizeTime);

            if (normolizeTime < 0)
            {
                return;
            }
            Vector2 result = Vector2.Lerp(lastFrame.value, currentFrame.value, normolizeTime);
            transform.LocalPosition = result;
        }
    }

    public struct SpriteJob : IJob<int>
    {
        public void Step(FrameData frameData, KeyFrame<int> lastFrame, KeyFrame<int> currentFrame)
        {
            if (frameData.entity.TryGet(out SpriteRender spriteRender))
            {
                spriteRender.selectRegion = currentFrame.value;
            }
        }
    }

    public interface IJob<V>
    {
        void Step(FrameData frameData, KeyFrame<V> lastFrame, KeyFrame<V> currentFrame);
    }
}
