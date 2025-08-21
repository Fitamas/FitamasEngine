using Fitamas.ECS;
using Fitamas.Graphics;
using Fitamas.Math;
using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Animation
{
    public abstract class TimeLine<TComponent, TValue> : ITimeLine where TComponent : class
    {
        [SerializeField] private string boneName;
        [SerializeField] private KeyFrame<TValue>[] keys;

        public KeyFrame<TValue>[] Keys => keys;
        public string BoneName => boneName;

        public TimeLine(string boneName, KeyFrame<TValue>[] keys)
        {
            this.keys = keys;
            this.boneName = boneName;
        }

        public int GetFrame(double time)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i].NormolizeTime >= time)
                {
                    return i - 1;
                }
            }

            return keys.Length - 1;
        }

        public void Step(Entity entity, AnimationInfo info)
        {
            if (!entity.Has<TComponent>())
            {
                return;
            }

            FrameData<TComponent> frameData;
            frameData = new FrameData<TComponent>();
            frameData.Entity = entity;
            frameData.Component = entity.Get<TComponent>();
            frameData.FrameId = GetFrame(info.NormolizeTime);

            if (frameData.FrameId < 0)
            {
                return;
            }

            KeyFrame<TValue> currentFrame = Keys[frameData.FrameId];
            KeyFrame<TValue> nextFrame = frameData.FrameId + 1 < Keys.Length ? Keys[frameData.FrameId + 1] : Keys[frameData.FrameId];

            Step(info, frameData, currentFrame, nextFrame);
        }

        protected abstract void Step(AnimationInfo info, FrameData<TComponent> frameData, KeyFrame<TValue> currentFrame, KeyFrame<TValue> nextFrame);
    }

    public interface ITimeLine
    {
        string BoneName { get; }
        void Step(Entity entity, AnimationInfo info);
    }

    public class TransformPositionTimeLine : TimeLine<Transform, Vector2>
    {
        public TransformPositionTimeLine(string boneName, KeyFrame<Vector2>[] keys) : base(boneName, keys)
        {

        }

        protected override void Step(AnimationInfo info, FrameData<Transform> frameData, KeyFrame<Vector2> currentFrame, KeyFrame<Vector2> nextFrame)
        {
            double normolizeTime = (info.NormolizeTime - currentFrame.NormolizeTime)
                / (nextFrame.NormolizeTime - currentFrame.NormolizeTime);

            if (normolizeTime < 0)
            {
                return;
            }

            Vector2 result = Vector2.Lerp(currentFrame.Value, nextFrame.Value, (float)normolizeTime * info.Weight);
            frameData.Component.LocalPosition = result;
        }
    }

    public class SpriteTimeLine : TimeLine<SpriteRendererComponent, int>
    {
        public SpriteTimeLine(string boneName, KeyFrame<int>[] keys) : base(boneName, keys)
        {

        }

        protected override void Step(AnimationInfo info, FrameData<SpriteRendererComponent> frameData, KeyFrame<int> currentFrame, KeyFrame<int> nextFrame)
        {
            frameData.Component.RectangleIndex = currentFrame.Value;
        }
    }
}
