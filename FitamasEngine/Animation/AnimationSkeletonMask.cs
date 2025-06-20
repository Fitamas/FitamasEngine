using Fitamas.Serialization;
using System.Collections.Generic;

namespace Fitamas.Animation
{
    public class AnimationSkeletonMask : MonoContentObject
    {
        [SerializeField] private Dictionary<string, bool> bones;

        public AnimationSkeletonMask(Dictionary<string, bool> bones)
        {
            this.bones = bones;
        }

        public AnimationSkeletonMask()
        {
            bones = new Dictionary<string, bool>();
        }

        public bool IsActive(string name)
        {
            return bones.TryGetValue(name, out bool value) && value;
        }
    }
}
