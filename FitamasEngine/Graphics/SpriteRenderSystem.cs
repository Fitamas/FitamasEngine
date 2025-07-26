using Fitamas.DebugTools;
using Fitamas.ECS;
using Fitamas.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Fitamas.Graphics
{
    public class SpriteRenderSystem : EntityDrawSystem, IDrawGizmosSystem
    {
        private GraphicsDevice graphicsDevice;
        private Renderer renderer;
        private SpriteBatch spriteBatch;
        private AlphaTestEffect effect;

        private ComponentMapper<Transform> transformMapper;
        private ComponentMapper<SpriteRender> spriteMapper;

        public SpriteRenderSystem(GraphicsDevice graphicsDevice, Renderer renderer) : base(Aspect.All(typeof(Transform), typeof(SpriteRender)))
        {
            this.graphicsDevice = graphicsDevice;
            this.renderer = renderer;

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

            foreach (var entityId in ActiveEntities)
            {
                Transform transform = transformMapper.Get(entityId);
                SpriteRender spriteRender = spriteMapper.Get(entityId);

                Render(gameTime, transform, spriteRender);
            }
        }

        public void Render(GameTime gameTime, Transform transform, SpriteRender spriteRender)
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

            Effect effect;

            if (spriteRender.Effect != null)
            {
                effect = spriteRender.Effect;
                Matrix view = Camera.Current.GetViewMatrix();
                Matrix projection = Camera.Current.GetProjectionMatrix();
                Matrix.Multiply(ref view, ref projection, out var result);
                effect.Parameters["WorldViewProj"]?.SetValue(result);
                //effect.Parameters["Texture"]?.SetValue(sprite.Texture);
                effect.Parameters["Time"]?.SetValue((float)gameTime.TotalGameTime.TotalSeconds);

                //textureParam = base.Parameters["Texture"];
                //diffuseColorParam = base.Parameters["DiffuseColor"];
                //alphaTestParam = base.Parameters["AlphaTest"];

                //TextureCube textureCube = new TextureCube(graphics, 80, false, SurfaceFormat.Color);

                //textureCube.SetData(CubeMapFace.PositiveX, )

                //Texture2D texture;
                //texture.SetData()
            }
            else
            {
                effect = this.effect;
                this.effect.Alpha = spriteRender.Alpha;
                this.effect.View = Camera.Current.GetViewMatrix();
                this.effect.Projection = Camera.Current.GetProjectionMatrix();
            }

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointWrap,
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

                if (spriteRender.Sprite == null)
                {
                    continue;
                }

                Gizmos.DrawRectangle(transform.Position, transform.Rotation, spriteRender.RenderSize, Color.Blue);
            }
        }
    }
}
