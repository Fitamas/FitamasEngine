using Fitamas.DebugTools;
using Fitamas.ECS;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Graphics
{
    public class SpriteDrawSystem : EntitySystem, IDrawGizmosSystem
    {
        private RenderManager renderManager;

        private ComponentMapper<Transform> transformMapper;
        private ComponentMapper<SpriteRendererComponent> spriteMapper;
        private ComponentMapper<TagsComponent> tagsMapper;

        public SpriteDrawSystem(RenderManager renderManager) : base(Aspect.All(typeof(Transform), typeof(SpriteRendererComponent)))
        {
            this.renderManager = renderManager;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform>();
            transformMapper.OnPut += PutRenderer;
            transformMapper.OnDelete += DeleteRenderer;
            spriteMapper = mapperService.GetMapper<SpriteRendererComponent>();
            spriteMapper.OnPut += PutRenderer;
            spriteMapper.OnDelete += DeleteRenderer;
            tagsMapper = mapperService.GetMapper<TagsComponent>();
            tagsMapper.OnPut += PutTags;
        }

        private void PutRenderer(int entityId)
        {
            if (transformMapper.Has(entityId) && spriteMapper.Has(entityId))
            {
                renderManager.Renderers[entityId] = new SpriteRenderer(transformMapper.Get(entityId), spriteMapper.Get(entityId));
            }
        }

        private void DeleteRenderer(int entityId)
        {
            renderManager.Renderers[entityId] = null;
        }

        private void PutTags(int entityId)
        {
            if (renderManager.Renderers[entityId] != null)
            {
                renderManager.Renderers[entityId].Tags = tagsMapper.Get(entityId).Tags;
            }
        }

        public void DrawGizmos()
        {
            foreach (var entityId in ActiveEntities)
            {
                Transform transform = transformMapper.Get(entityId);
                SpriteRendererComponent spriteRender = spriteMapper.Get(entityId);

                if (spriteRender.Sprite == null)
                {
                    continue;
                }

                Gizmos.DrawRectangle(transform.Position, transform.Rotation, spriteRender.RenderSize, Color.Blue);
            }
        }
    }
}
