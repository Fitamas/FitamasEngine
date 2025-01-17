using Fitamas.Entities;
using System;

namespace Fitamas.Animation
{
    public struct AnimationInfo
    {
        public float startTime { get; set; }
        public float animationLenght { get; set; }
        public float allTime { get; set; }
        public float time { get; set; }
        public float normolizeTime { get; set; }
        public float normolizedDeltaTime { get; set; }
    }

    public struct FrameData
    {
        public Entity entity { get; set; }
        public int frameId { get; set; }
        public float normolizedDeltaTime { get; set; }
        public float normolizeTime { get; set; }
    }

    public struct KeyFrame<T>
    {
        public T value;
        public float normolizeTime;
    }

    public struct TimeData
    {
        public float allTime { get; set; }
        public float deltaTime { get; set; }
    }
}
