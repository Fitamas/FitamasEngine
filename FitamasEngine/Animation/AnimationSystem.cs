using Fitamas.Entities;
using Fitamas.Physics.Characters;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Fitamas.Animation
{
    public class AnimationSystem : EntityUpdateSystem
    {
        private ComponentMapper<Animator> animatorMapper;

        public AnimationSystem() : base(Aspect.All(typeof(Animator)))
        {

        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            animatorMapper = mapperService.GetMapper<Animator>();
        }

        protected override void OnEntityAdded(int entityId)
        {
            InitAnimator(entityId);
        }

        protected override void OnEntityChanged(int entityId)
        {
            InitAnimator(entityId);
        }

        private void InitAnimator(int entityId)
        {
            if (ActiveEntities.Contains(entityId))
            {
                Entity entity = GetEntity(entityId);
                Animator animator = animatorMapper.Get(entityId);

                animator.Init(entity);

                if (entity.TryGet(out Character character))
                {
                    //animator.SetAvatar(character.Avatar);
                }
            }
        }

        protected override void OnEntityRemoved(int entityId)
        {
            if (ActiveEntities.Contains(entityId))
            {
                Animator animator = animatorMapper.Get(entityId);

                animator.Destroy();
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var id in ActiveEntities)
            {
                Animator animator = animatorMapper.Get(id);

                animator.Update(gameTime);
            }
        }
    }
}
