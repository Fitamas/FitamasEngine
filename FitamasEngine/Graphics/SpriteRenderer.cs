using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Fitamas.ECS;
using System.Collections.Generic;

namespace Fitamas.Graphics
{
    public class SpriteRenderer : Renderer
    {
        public Transform transform;
        public SpriteRendererComponent spriteRendererComponent;

        public SpriteRenderer(Transform transform, SpriteRendererComponent spriteRendererComponent)
        {
            this.transform = transform;
            this.spriteRendererComponent = spriteRendererComponent;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (spriteRendererComponent.Sprite == null)
            {
                return;
            }

            Sprite sprite = spriteRendererComponent.Sprite;
            Material material = spriteRendererComponent.Material ?? Material.DefaultMaterial;
            Effect effect = material.Effect;
            Vector2 position = transform.ToAbsolutePosition(spriteRendererComponent.SpriteOffset);
            float angle = transform.Rotation + MathF.PI;
            Vector2 scale = transform.Scale / spriteRendererComponent.Sprite.PixelInUnit;
            float layer = (float)spriteRendererComponent.Layer / Settings.LayersCount;
            Rectangle sourceRectangle = spriteRendererComponent.RectangleIndex >= 0 &&
                                           spriteRendererComponent.RectangleIndex < sprite.FramesCount ?
                                           sprite.Frames[spriteRendererComponent.RectangleIndex].Bounds : sprite.DefaultFrame.Bounds;


            Matrix view = Camera.Current.GetViewMatrix();
            Matrix projection = Camera.Current.GetProjectionMatrix();
            Matrix.Multiply(ref view, ref projection, out var result);
            effect.Parameters["WorldViewProj"]?.SetValue(result);
            effect.Parameters["Time"]?.SetValue((float)gameTime.TotalGameTime.TotalSeconds);

            spriteBatch.Begin(SpriteSortMode.Deferred, material.BlendState, material.SamplerState,
                  material.DepthStencilState, RasterizerState.CullNone, effect, Camera.Current.TranslationVirtualMatrix);

            spriteBatch.Draw(sprite, position, sourceRectangle, spriteRendererComponent.Color, angle,
                             spriteRendererComponent.Origin, scale, spriteRendererComponent.SpriteEffect, layer);

            spriteBatch.End();
        }
    }
}
