using Fitamas.Collections;
using Fitamas.ECS;
using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Fitamas.Animation
{
    public class Animator : Component
    {
        public static readonly AnimationTree DefaultTree = new AnimationTree(nameof(DefaultTree));

        [SerializeField] private AnimationTree animationTree;

        private Dictionary<string, float> properies;
        private List<AnimationLayerState> layers;

        public AnimationTree AnimationTree => animationTree;

        public Animator(AnimationTree animationTree)
        {
            properies = new Dictionary<string, float>();
            layers = new List<AnimationLayerState>();
            this.animationTree = animationTree;
        }

        private Animator() : this(DefaultTree)
        {

        }

        internal void Init()
        {
            layers.Clear();
            foreach (var layer in AnimationTree.Layers)
            {
                layers.Add(new AnimationLayerState(this, layer));
            }
        }

        internal void Step(GameTime gameTime, Entity entity, AnimationBone bone)
        {
            foreach (var layer in layers)
            {
                layer.Step(gameTime, entity, bone);
            }
        }

        public float GetValue(string name)
        {
            if (properies.TryGetValue(name, out float value))
            {
                return value;
            }

            return default;
        }

        public void SetValue(string name, float value)
        {
            properies[name] = value;
        }

        public void Play(string name)
        {
            foreach (var layer in layers)
            {
                layer.Play(name);
            }
        }

        public void Play(string name, float fade)
        {
            foreach (var layer in layers)
            {
                layer.Play(name, fade);
            }
        }
    }
}
