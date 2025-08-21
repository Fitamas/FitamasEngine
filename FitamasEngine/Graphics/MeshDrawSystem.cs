using Fitamas.ECS;
using Microsoft.Xna.Framework;
using System;
using Fitamas.Math;
using Fitamas.DebugTools;

namespace Fitamas.Graphics
{
    public class MeshDrawSystem : EntitySystem, IDrawGizmosSystem
    {
        private RenderManager renderManager;

        private ComponentMapper<Transform> transformMapper;
        private ComponentMapper<MeshRendererComponent> meshRenderMapper;
        private ComponentMapper<MeshComponent> meshMapper;
        private ComponentMapper<TagsComponent> tagsMapper;

        public MeshDrawSystem(RenderManager renderManager) : base(Aspect.All(typeof(Transform), typeof(MeshRendererComponent), typeof(MeshComponent)))
        {
            this.renderManager = renderManager;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform>();
            transformMapper.OnPut += PutRenderer;
            transformMapper.OnDelete += DeleteRenderer;
            meshRenderMapper = mapperService.GetMapper<MeshRendererComponent>();
            meshRenderMapper.OnPut += PutRenderer;
            meshRenderMapper.OnDelete += DeleteRenderer;
            meshMapper = mapperService.GetMapper<MeshComponent>();
            meshMapper.OnPut += PutRenderer;
            meshMapper.OnDelete += DeleteRenderer;
            tagsMapper = mapperService.GetMapper<TagsComponent>();
            tagsMapper.OnPut += PutTags;
        }

        private void PutRenderer(int entityId)
        {
            if (transformMapper.Has(entityId) && meshRenderMapper.Has(entityId) && meshMapper.Has(entityId))
            {
                renderManager.Renderers[entityId] = new MeshRenderer(transformMapper.Get(entityId), meshRenderMapper.Get(entityId), meshMapper.Get(entityId));
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
                MeshComponent mesh = meshMapper.Get(entityId);
                Transform transform = transformMapper.Get(entityId);

                Gizmos.DrawPolygon(transform.Position, transform.Rotation, mesh, Color.Black);
            }
        }
    }
}
