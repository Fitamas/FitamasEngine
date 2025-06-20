using Fitamas.ECS;
using System;

namespace Fitamas.Animation
{
    public enum AnimationPhase
    {
        None,
        Play,
        Pause,
        Resume,
        Stop, 
    }

    public struct AnimationInfo
    {
        public bool IsPlaying { get; set; }
        public AnimationClip Clip { get; set; }
        public float Weight { get; set; }
        public double StartTime { get; set; }
        public double AllTime { get; set; }
        public double ClipLenght => Clip.Lenght;
        public double Time => AllTime % ClipLenght;
        public double NormolizeTime => Time / ClipLenght;
        public double NormolizedDeltaTime => Time / ClipLenght;
    }

    public struct FrameData<TComponent>
    {
        public Entity Entity { get; set; }
        public TComponent Component { get; set; }
        public int FrameId { get; set; }
    }

    public struct KeyFrame<T>
    {
        public double NormolizeTime { get; }
        public T Value { get; }

        public KeyFrame(double normolizeTime, T value)
        {
            NormolizeTime = normolizeTime;
            Value = value;
        }
    }
}
