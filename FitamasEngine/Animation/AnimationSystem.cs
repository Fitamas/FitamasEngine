using Fitamas.Animation.Rigging;
using Fitamas.Collections;
using Fitamas.ECS;
using Microsoft.Xna.Framework;

namespace Fitamas.Animation
{
    public class AnimationSystem : EntityUpdateSystem
    {
        private ComponentMapper<Animator> animatorMapper;
        private ComponentMapper<AnimationRequest> requestMapper;

        public AnimationSystem() : base(Aspect.All(typeof(Animator), typeof(AnimationRequest)))
        {

        }

        public override void Initialize(GameWorld world)
        {
            world.RegisterSystem(new AnimationBoneSystem());
            world.RegisterSystem(new RiggingSystem());

            base.Initialize(world);
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            animatorMapper = mapperService.GetMapper<Animator>();
            animatorMapper.OnPut += PutAnimator;
            requestMapper = mapperService.GetMapper<AnimationRequest>();
        }

        private void PutAnimator(int entityId)
        {
            Animator animator = animatorMapper.Get(entityId);
            animator.Init();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entityId in ActiveEntities)
            {
                Animator animator = animatorMapper.Get(entityId);
                AnimationRequest request = requestMapper.Get(entityId);

                animator.Play(request.Animation);

                requestMapper.Delete(entityId);
            }
        }
    }
}
