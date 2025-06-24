using Fitamas;
using Fitamas.Animation;
using Fitamas.Core;
using Fitamas.ECS;
using Fitamas.Graphics;
using Fitamas.Input;
using Fitamas.Serialization.Json.Converters;
using Fitamas.Tweening;
using Fitamas.UserInterface;
using Fitamas.UserInterface.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Animation
{
    public class AnimationGame : GameEngine
    {
        protected override void Initialize()
        {
            base.Initialize();

            World.CreateMainCamera();

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

            Entity entity = World.CreateEntity();
            entity.Attach(new SpriteRender()
            {
                Sprite = Sprite.Create("Pumpkin"),
            });
            entity.Attach(new Transform()
            {
                Position = new Vector2(4, 0)
            });
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

            entity = World.CreateEntity();
            entity.Attach(new SpriteRender()
            {
                Sprite = Sprite.Create("TestBox", [new Rectangle(0, 0, 40, 40), new Rectangle(32, 32, 20, 20)]),
            });
            entity.Attach(new Transform()
            {
                Position = new Vector2(4, 0)
            });
            entity.Attach(new AnimationBone()
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


            manager = new TweenManager();

            tween = TweenHelper.ColorTween(Color.White, Color.Black, 1, 0, x => Debug.LogError(x));
            tween.Repeat(4, 0.5f)
                 .OnBegin((c) => Debug.LogError("begin"))
                 .OnPlay((c) => Debug.LogError("play"))
                 .OnPause((c) => Debug.LogError("pause"))
                 .OnComplete((c) => Debug.LogError("complete"))
                 .OnStepComplete((c) => Debug.LogError("step complete"))
                 .OnEnd((c) => Debug.LogError("end"))
                 /*.AutoReverse()*/;

            manager.AddActive(tween);
            //TODO TWEEN
        }

        TweenManager manager;
        Color color = Color.White;
        Tween tween;

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            manager.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            GUIDebug.Active = true;

            FontManager.DefaultFont = Content.Load<SpriteFont>("Font\\Pixel_20");
            ResourceDictionary dictionary = ResourceDictionary.DefaultResources;
            dictionary[CommonResourceKeys.DefaultFont] = FontManager.DefaultFont;
        }
    }
}
