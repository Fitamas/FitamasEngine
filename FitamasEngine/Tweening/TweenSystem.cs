using Fitamas.Collections;
using Fitamas.ECS;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Tweening
{
    class TweenSystem : EntitySystem
    {
        private TweenManager manager;

        private Bag<Tween> tweens;

        private ComponentMapper<TweenComponent> tweenerMapper;

        public TweenSystem(TweenManager manager) : base(Aspect.All(typeof(TweenComponent)))
        {
            this.manager = manager;
            tweens = new Bag<Tween>();
            manager.OnRemoveTween += tween =>
            {
                int entityId = tweens.IndexOf(tween);
                if (entityId != -1)
                {
                    tweenerMapper.Delete(entityId);
                }
            };
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            tweenerMapper = mapperService.GetMapper<TweenComponent>();
            tweenerMapper.OnPut += PutTween;
            tweenerMapper.OnDelete += DeleteTween;
        }

        private void PutTween(int entityId)
        {
            TweenComponent tween = tweenerMapper.Get(entityId);
            manager.AddActive(tween.Tween);
            tweens[entityId] = tween.Tween;
        }

        private void DeleteTween(int entityId)
        {
            Tween tween = tweens[entityId];

            if (tween != null)
            {
                tweens[entityId] = null;
                manager.RemoveActive(tween);
            }
        }
    }
}
