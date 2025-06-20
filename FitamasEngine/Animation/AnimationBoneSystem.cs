using Fitamas.ECS;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Animation
{
    public class AnimationBoneSystem : EntityUpdateSystem
    {
        private ComponentMapper<AnimationBone> boneMapper;

        public AnimationBoneSystem() : base(Aspect.All(typeof(AnimationBone)))
        {

        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            boneMapper = mapperService.GetMapper<AnimationBone>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entityId in ActiveEntities)
            {
                Entity entity = GetEntity(entityId);
                AnimationBone bone = boneMapper.Get(entityId);

                bone.Animator?.Step(gameTime, entity, bone);
            }
        }
    }
}
