using Fitamas.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Fitamas.Animation
{
    public abstract class AnimationState
    {
        public string Name { get; }
        public Animator Animator { get; }

        public AnimationState(string name, Animator animator)
        {
            Name = name;
            Animator = animator;
        }

        public abstract void Step(GameTime gameTime, Entity entity, AnimationBone bone, float weight);

        public abstract void Play();

        public abstract void Stop();
    }

    public class AnimationLayerState
    {
        private AnimationLayerInfo layer;
        private Dictionary<string, AnimationState> states;
        private AnimationState currentState;
        private AnimationState nextState;
        private bool isTransition;

        public AnimationLayerState(Animator animator, AnimationLayerInfo layer)
        {
            this.layer = layer;
            states = new Dictionary<string, AnimationState>();

            foreach (var clip in layer.Clips)
            {
                states.Add(clip.Name, new AnimationClipState(animator, clip));
            }

            foreach (var blend in layer.Blends)
            {
                states.Add(blend.Name, new AnimationBlendState(animator, blend));
            }
        }

        public void Step(GameTime gameTime, Entity entity, AnimationBone bone)
        {
            if (layer.Mask != null && !layer.Mask.IsActive(bone.Name))
            {
                return;
            }

            currentState?.Step(gameTime, entity, bone, 1f);
        }

        public void Play(string name)
        {
            if (states.TryGetValue(name, out AnimationState state))
            {
                currentState?.Stop();
                currentState = state;
                currentState.Play();
            }
        }

        public void Play(string name, float fade)
        {
            throw new NotImplementedException(); //TODO
        }
    }

    public class AnimationBlendState : AnimationState
    {
        private AnimationBlendInfo blend;
        private List<AnimationClipState> animationStates;

        public AnimationBlendType BlendType => blend.BlendType;

        public AnimationBlendState(Animator animator, AnimationBlendInfo blend) : base(blend.Name, animator)
        {
            this.blend = blend;
            animationStates = new List<AnimationClipState>();

            foreach (var motion in blend.Motions)
            {
                animationStates.Add(new AnimationClipState(animator, new AnimationClipInfo() { Clip = motion.Clip }));
            }
        }

        public override void Step(GameTime gameTime, Entity entity, AnimationBone bone, float weight)
        {
            switch (BlendType)
            {
                case AnimationBlendType.Blend1D: //TODO
                    throw new NotImplementedException();
                    break;
                case AnimationBlendType.Blend2D: //TODO
                    throw new NotImplementedException();
                    break;
                case AnimationBlendType.BlendDirect:
                    foreach (var state in animationStates)
                    {
                        state.Step(gameTime, entity, bone, Animator.GetValue(Name));
                    }
                    break;
            }
        }

        public override void Play()
        {
            foreach (var state in animationStates)
            {
                state.Play();
            }
        }

        public override void Stop()
        {
            foreach (var state in animationStates)
            {
                state.Stop();
            }
        }
    }

    public class AnimationClipState : AnimationState
    {
        private AnimationInfo info;
        private AnimationPhase phase;

        public AnimationClipState(Animator animator, AnimationClipInfo clip) : base(clip.Name, animator)
        {
            info.Clip = clip.Clip;
        }

        public override void Step(GameTime gameTime, Entity entity, AnimationBone bone, float weight)
        {
            switch (phase)
            {
                case AnimationPhase.Play:
                    info.IsPlaying = true;
                    info.StartTime = gameTime.TotalGameTime.TotalSeconds;
                    break;
                case AnimationPhase.Pause:
                    info.IsPlaying = false;
                    break;
                case AnimationPhase.Resume:
                    info.IsPlaying = true;
                    break;
                case AnimationPhase.Stop:
                    info.IsPlaying = false;
                    info.StartTime = gameTime.TotalGameTime.TotalSeconds;
                    break;
            }

            phase = AnimationPhase.None;

            info.AllTime = gameTime.TotalGameTime.TotalSeconds - info.StartTime;
            info.Weight = weight;

            if (info.IsPlaying)
            {
                foreach (var timeLine in info.Clip.TimeLines)
                {
                    if (bone.Name == timeLine.BoneName)
                    {
                        timeLine.Step(entity, info);
                    }
                }
            }
        }

        public override void Play()
        {
            phase = AnimationPhase.Play;
        }

        public override void Stop()
        {
            phase = AnimationPhase.Stop;
        }
    }
}
