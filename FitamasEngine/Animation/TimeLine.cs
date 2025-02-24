using Fitamas.Entities;
using Fitamas.Serialization;
using System;

namespace Fitamas.Animation
{
    public class TimeLine<O, V> : ITimeLine where O : struct, IJob<V>
    {
        [SerializableField] private string entityName;
        [SerializableField] private KeyFrame<V>[] keys;

        public TimeLine(string entityName, KeyFrame<V>[] keys)
        {
            this.keys = keys;
            this.entityName = entityName;
        }

        public KeyFrame<V>[] Keys => keys;
        public string EntityName => entityName;

        public IJobController CreateJob()
        {
            return new JobController<O, V>(this);
        }

        public int GetFrame(float time)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i].normolizeTime > time)
                {
                    return i;
                }
            }

            return -1;
        }
    }

    public interface ITimeLine
    {
        IJobController CreateJob();
    }
}
