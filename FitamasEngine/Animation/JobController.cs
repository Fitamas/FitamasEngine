using Fitamas.Entities;
using System;

namespace Fitamas.Animation
{
    public struct JobController<O, V> : IJobController where O : struct, IJob<V>
    {
        private TimeLine<O, V> timeLine;
        private FrameData frameData;
        private IJob<V> job;

        public string EntityName => timeLine.EntityName;

        public JobController(TimeLine<O, V> timeLine)
        {
            frameData = new FrameData();
            frameData.frameId = -1;

            this.timeLine = timeLine;
            job = new O();
        }

        public void Step(Entity entity, AnimationInfo info)
        {
            frameData.entity = entity;
            frameData.frameId = timeLine.GetFrame(info.normolizeTime);
            frameData.normolizedDeltaTime = info.normolizedDeltaTime;
            frameData.normolizeTime = info.normolizeTime;

            if (frameData.frameId < 0)
            {
                return;
            }
            else if (frameData.frameId == 0)
            {
                KeyFrame<V> lastFrame = new KeyFrame<V>();
                KeyFrame<V> currentFrame = timeLine.Keys[frameData.frameId];

                job.Step(frameData, lastFrame, currentFrame);
            }
            else
            {
                KeyFrame<V> lastFrame = timeLine.Keys[frameData.frameId - 1];
                KeyFrame<V> currentFrame = timeLine.Keys[frameData.frameId];

                job.Step(frameData, lastFrame, currentFrame);
            }
        }
    }

    public interface IJobController
    {
        string EntityName { get; }
        void Step(Entity entity, AnimationInfo info);
    }
}
