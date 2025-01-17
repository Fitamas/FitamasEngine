using Fitamas.Entities;
using Fitamas.Physics.Characters;
using System;
using System.Collections.Generic;

namespace Fitamas.Animation
{
    public class RuntimeAnimatorController
    {
        public const string DefoultAnimation = "idle";

        private AnimationNode[] nodes;
        private TimeData timeData = new TimeData();

        public Avatar Avatar { get; set; }

        public RuntimeAnimatorController(AnimationTree animationTree, Avatar avatar)
        {
            Avatar = avatar;

            nodes = animationTree.CreateNodes();
        }

        public void Init()
        {
            foreach (var node in nodes)
            {
                node.InitNode(this);
            }

            foreach (var node in nodes)
            {
                if (node.Name == DefoultAnimation)
                {
                    node.Play();
                }
            }
        }

        public void Step(float deltaTime)
        {
            timeData.allTime += deltaTime;
            timeData.deltaTime = deltaTime;
            foreach (var node in nodes) 
            {
                node.Step(timeData);
            }
        }

        public Entity GetEntity(string name)
        {
            return Avatar.GetElement(name);
        }
    }

    public class AnimationNode
    {
        private string name;
        private List<AnimationNode> nodes = new List<AnimationNode>();
        private AnimationNode root;
        private RuntimeAnimatorController controller;

        public string Name => name;
        public RuntimeAnimatorController Controller => controller;

        public AnimationNode(string name)
        {
            this.name = name;
        }

        public void InitNode(RuntimeAnimatorController controller)
        {
            this.controller = controller;
            OnInit();
        }

        public void AddNode(AnimationNode node)
        {
            if (this != node)
            {
                nodes.Add(node);
                node.root = this;
            }
        }

        public virtual void OnInit() { }
        public virtual void Play() { }
        public virtual void Stop() { }
        public virtual void Step(TimeData data) { }
    }

    public class AnimationClipNode : AnimationNode
    {
        private AnimationClip clip;
        private IJobController[] jobs;
        private AnimationInfo info;
        private TimeData timeData;
        private bool isPlay = false;

        public AnimationClipNode(string name, AnimationClip clip) : base(name)
        {
            this.clip = clip;
        }

        public override void OnInit()
        {
            jobs = new IJobController[clip.timeLines.Length];
            for (int i = 0; i < clip.timeLines.Length; i++)
            {
                jobs[i] = clip.timeLines[i].CreateJob();
            }
            timeData = new TimeData();
            info = new AnimationInfo();
            info.animationLenght = clip.Time;
        }

        public override void Play()
        {
            isPlay = true;
            info.startTime = timeData.allTime;
        }

        public override void Step(TimeData data)
        {
            timeData = data;
            info.allTime = timeData.allTime - info.startTime;
            if (info.animationLenght > 0)
            {
                info.time = info.allTime % info.animationLenght;
                info.normolizeTime = info.time / info.animationLenght;
                info.normolizedDeltaTime = data.deltaTime / info.animationLenght;
            }

            if (isPlay)
            {
                foreach (var job in jobs)
                {
                    Entity entity = Controller.GetEntity(job.EntityName);
                    job.Step(entity, info);
                }
            }
        }

        public override void Stop()
        {
            isPlay = false;
        }
    }

    public class AnimationMixerNode : AnimationNode
    {
        public AnimationMixerNode(string name) : base(name)
        {

        }
    }
}
