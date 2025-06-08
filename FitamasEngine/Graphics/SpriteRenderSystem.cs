using Assimp;
using Fitamas.DebugTools;
using Fitamas.Entities;
using Fitamas.Math2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Fitamas.Graphics
{
    public class SpriteRenderSystem : EntityDrawSystem, IDrawGizmosSystem
    {
        private GraphicsDevice graphics;
        private SpriteBatch spriteBatch;
        private AlphaTestEffect effect;

        private ComponentMapper<Transform> transformMapper;
        private ComponentMapper<SpriteRender> spriteMapper;

        public SpriteRenderSystem(GraphicsDevice graphicsDevice) : base(Aspect.All(typeof(Transform), typeof(SpriteRender)))
        {
            graphics = graphicsDevice;

            spriteBatch = new SpriteBatch(graphicsDevice);
            effect = new AlphaTestEffect(graphicsDevice);
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform>();
            spriteMapper = mapperService.GetMapper<SpriteRender>();
        }

        public override void Draw(GameTime gameTime)
        {
            if (Camera.Current == null)
            {
                return;
            }

            effect.View = Camera.Current.GetViewMatrix();
            effect.Projection = Camera.Current.GetProjectionMatrix();

            foreach (var entityId in ActiveEntities)
            {
                Transform transform = transformMapper.Get(entityId);
                SpriteRender spriteRender = spriteMapper.Get(entityId);

                Render(transform, spriteRender);
            }
        }

        public void Render(Transform transform, SpriteRender spriteRender)
        {
            if (spriteRender.Sprite == null || spriteRender.Sprite.Texture == null)
            {
                return;
            }

            Sprite sprite = spriteRender.Sprite;
            Vector2 position = transform.ToAbsolutePosition(spriteRender.SpriteOffset);
            float angle = transform.Rotation + MathF.PI;
            Vector2 scale = transform.Scale / spriteRender.Sprite.PixelInUnit;
            float layer = (float)spriteRender.Layer / Settings.LayersCount;
            Rectangle sourceRectangle = spriteRender.RectangleIndex >= 0 && 
                                           spriteRender.RectangleIndex < sprite.Rectangles.Length ?
                                           sprite.Rectangles[spriteRender.RectangleIndex] : sprite.Bounds;

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap,
                  DepthStencilState.Default, RasterizerState.CullNone, effect, Camera.Current.TranslationVirtualMatrix);

            spriteBatch.Draw(sprite, position, sourceRectangle, spriteRender.Color, angle, 
                             spriteRender.Origin, scale, spriteRender.SpriteEffect, layer);

            spriteBatch.End();
        }

        public void DrawGizmos()
        {
            foreach (var entityId in ActiveEntities)
            {
                Transform transform = transformMapper.Get(entityId);
                SpriteRender spriteRender = spriteMapper.Get(entityId);

                Gizmos.DrawRectangle(transform.Position, transform.Rotation, spriteRender.RenderSize, Color.Blue);
            }
        }
    }
}
