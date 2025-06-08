using Fitamas.Entities;
using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Fitamas.Animation
{
    public class AnimationTree : MonoObject
    {
        public string Name;
        public List<AnimationLayerInfo> Layers;

        public AnimationTree(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            Layers = new List<AnimationLayerInfo>();
        }
    }

    public enum AnimationBlendType
    {
        Blend1D = 0,
        Blend2D = 1,
        BlendDirect = 2,
    }

    public class AnimationLayerInfo
    {
        public string Name;
        public AnimationSkeletonMask Mask;
        public List<AnimationBlendInfo> Blends;
        public List<AnimationClipInfo> Clips;

        public AnimationLayerInfo()
        {
            Clips = new List<AnimationClipInfo>();
            Blends = new List<AnimationBlendInfo>();
        }
    }

    public class AnimationClipInfo
    {
        public string Name;
        public AnimationClip Clip;
        public float TimeScale;
        public bool IsMirror;
        public bool Loop;
    }

    public class AnimationBlendInfo
    {
        public string Name;
        public AnimationBlendType BlendType;
        public List<Motion> Motions;

        public AnimationBlendInfo()
        {
            Motions = new List<Motion>();
        }
    }

    public class Motion
    {
        public AnimationClip Clip;
        public float Value1;
        public float Value2;
        public float TimeScale;
        public bool IsMirror;
        public bool Loop;
    }
}
