using Fitamas.Entities;
using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Fitamas.Animation
{
    public class Animator
    {
        [SerializeField] private AnimationTree animationTree;
        [SerializeField] private AnimationController animationController;

        private RigController rigController;
        private RuntimeAnimatorController controller;
        private Avatar avatar;
        private bool isInit = false;

        public AnimationTree AnimationTree => animationTree;
        public AnimationController AnimationController => animationController;
        public RigController RigController => rigController;
        public RuntimeAnimatorController Controller => controller;
        public Avatar Avatar => avatar;

        public Animator()
        {

        }

        public Animator(AnimationTree animationTree, AnimationController animationController = null) 
        { 
            this.animationTree = animationTree;
            this.animationController = animationController;
        }

        public void SetAvatar(Avatar avatar)
        {
            if (avatar != null && this.avatar != avatar)
            {
                this.avatar = avatar;
                
                if (controller != null)
                {
                    controller.Avatar = avatar;
                }

                animationController.SetAvatar(avatar);

                rigController.SetAvatar(avatar);
            }
        }

        public void Init(Entity entity)
        {
            if (!isInit)
            {
                avatar = new DefoultAvatar(entity);

                if (controller == null && animationTree != null)
                {
                    controller = new RuntimeAnimatorController(animationTree, avatar);
                    controller.Init();
                }

                rigController = new RigController();
                rigController.SetAvatar(avatar);

                animationController?.Init(entity, this);

                isInit = true;
            }
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            animationController?.Update(gameTime);

            controller?.Step(deltaTime);

            rigController?.Update(deltaTime);
        }

        public AnimationClip GetAnimation(string animation)
        {
            return animationTree.GetAnimation(animation);
        }

        public void Destroy()
        {
            if (isInit)
            {
                controller = null;
                rigController = null;
                avatar = null;

                isInit = false;
            }
        }
    }
}
