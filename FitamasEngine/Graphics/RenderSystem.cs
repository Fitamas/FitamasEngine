using Fitamas.Entities;
using Fitamas.Math2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Fitamas.Graphics
{
    public class RenderSystem : EntityDrawSystem
    {
        private GraphicsDevice graphics;

        private ComponentMapper<Transform> transformMapper;
        private ComponentMapper<SpriteRender> spriteMapper;
        private ComponentMapper<MeshRender> renderMapper;
        private ComponentMapper<Mesh> meshMapper;

        public RenderSystem(GraphicsDevice graphicsDevice) : base(Aspect.All(typeof(Transform)).One(typeof(SpriteRender), typeof(MeshRender), typeof(Mesh)))
        {
            graphics = graphicsDevice;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform>();
            spriteMapper = mapperService.GetMapper<SpriteRender>();
            renderMapper = mapperService.GetMapper<MeshRender>();
            meshMapper = mapperService.GetMapper<Mesh>();
        }

        public override void Draw(GameTime gameTime)
        {
            if (Camera.Current == null)
            {
                graphics.Clear(Color.Black);
                return;
            }

            RenderBatch.Begin(graphics, Camera.Current);

            foreach (var entityId in ActiveEntities)
            {
                if(spriteMapper.Has(entityId))
                {
                    Transform transform = transformMapper.Get(entityId);
                    SpriteRender spriteRender = spriteMapper.Get(entityId);

                    RenderBatch.Render(transform, spriteRender);
                }
            }

            RenderBatch.End();

            foreach (var entityId in ActiveEntities)
            {
                if (renderMapper.Has(entityId) && meshMapper.Has(entityId))
                {
                    MeshRender render = renderMapper.Get(entityId);
                    Mesh mesh = meshMapper.Get(entityId);
                    Transform transform = transformMapper.Get(entityId);

                    RenderBatch.Render(transform, render, mesh);
                }
            }
        }
    }
}
