﻿using Fitamas;
using Fitamas.Animation;
using Fitamas.Core;
using Fitamas.ECS;
using Fitamas.Graphics;
using Fitamas.Input;
using Fitamas.Tweening;
using Fitamas.UserInterface;
using Fitamas.UserInterface.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using R3;

namespace Animation
{
    public class AnimationGame : GameEngine
    {
        protected override void Initialize()
        {
            base.Initialize();

            GameWorld.CreateMainCamera();

            AnimationClip clip = new AnimationClip("test", 4, [
                    new TransformPositionTimeLine("BONE1",
                    [
                        new KeyFrame<Vector2>(0, new Vector2(0, 1)),
                        new KeyFrame<Vector2>(1, new Vector2(1, -1))
                    ]),
                    new TransformPositionTimeLine("BONE2",
                    [
                        new KeyFrame<Vector2>(0, new Vector2(0, 1)),
                        new KeyFrame<Vector2>(1, new Vector2(1, 2))
                    ]),
                    new SpriteTimeLine("BONE2",
                    [
                        new KeyFrame<int>(0, 0),
                        new KeyFrame<int>(0.5, 1)
                    ])
                ]);
            AnimationTree tree = new AnimationTree("testTree");
            AnimationClipInfo info = new AnimationClipInfo();
            info.Name = "test";
            info.Clip = clip;
            //node.BlendType = AnimationBlendType.BlendDirect;
            //node.Motions.Add(new Motion() { Clip = clip });
            tree.Layers.Add(new AnimationLayerInfo()
            {
                Mask = new AnimationSkeletonMask(new System.Collections.Generic.Dictionary<string, bool> { { "BONE2", true } }),
                Clips = [info]
            });

            Animator animator;

            Entity entity = CreatePumpkin();
            entity.Attach(animator = new Animator(tree)
            {

            });
            entity.Attach(new AnimationBone()
            {
                Animator = animator,
                Name = "BONE1",
            });
            entity.Attach(new AnimationRequest()
            {
                Animation = "test"
            });

            Entity entity1 = GameWorld.CreateEntity();
            entity1.Attach(new SpriteRender()
            {
                Sprite = Sprite.Create("TestBox", [new Rectangle(0, 0, 40, 40), new Rectangle(32, 32, 20, 20)]),
            });
            entity1.Attach(new Transform()
            {
                Position = new Vector2(4, 0)
            });
            entity1.Attach(new AnimationBone()
            {
                Animator = animator,
                Name = "BONE2",
            });

            GUISystem system = MainContainer.Resolve<GUISystem>(ApplicationKey.GUISystem);
            GUISlider slider = GUI.CreateSlider(new Point(50, 150), GUISliderDirection.LeftToRight, 200);
            slider.Pivot = new Vector2(0, 0);
            slider.Track.OnValueChanged.AddListener((s, v) =>
            {
                animator.SetValue("test", v);
            });
            system.AddComponent(slider);

            TweenComponent component;
            entity.Attach(component = entity.Get<Transform>().Move(new Vector2(4, 4), 1)
                                   .Easing(EasingFunctions.CircleInOut).SetDelay(1).Repeat(1) //RepeatForever()
                                   .OnStart((c) => Debug.Log("start"))
                                   .OnPlay((c) => Debug.Log("play"))
                                   .OnPause((c) => Debug.Log("pause"))
                                   .OnComplete((c) => Debug.Log("complete"))
                                   .OnStepComplete((c) => Debug.Log("step complete"))
                                   .OnKill((c) => Debug.Log("killp"))
                                   .AutoReverse().ToComponent());

            //entity.Attach(new SequenceTween().Append(entity.Get<Transform>().Move(new Vector2(4, 4), 1)
            //                              .Easing(EasingFunctions.BounceOut)
            //                              .RepeatForever().AutoReverse().Repeat(1, 1))

            //                              .Join(entity.Get<Transform>().Rotate(4, 1)
            //                              .RepeatForever().AutoReverse().Repeat(4, 0))

            //                              .Append(entity.Get<Transform>().Move(new Vector2(4, 4), 1)
            //                              .RepeatForever().AutoReverse().Repeat(1, 1))

            //                              .ToInstance());


        }

        protected override void LoadContent()
        {
            base.LoadContent();

            GUIDebug.Active = true;

            FontManager.DefaultFont = Content.Load<SpriteFont>("Font\\Pixel_20");
            ResourceDictionary dictionary = ResourceDictionary.DefaultResources;
            dictionary[CommonResourceKeys.DefaultFont] = FontManager.DefaultFont;
        }

        public Entity CreatePumpkin()
        {
            Entity entity = GameWorld.CreateEntity();
            entity.Attach(new SpriteRender()
            {
                Sprite = Sprite.Create("Pumpkin"),
            });
            entity.Attach(new Transform()
            {
                Position = new Vector2(4, 0)
            });
            return entity;
        }
    }
}
