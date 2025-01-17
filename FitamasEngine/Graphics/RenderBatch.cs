using Fitamas.Entities;
using Fitamas.Math2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using System;

namespace Fitamas.Graphics
{
    public static class RenderBatch
    {
        private const int maxSpriteLayers = 10;

        private static SpriteBatch spriteBatch;
        private static GraphicsDevice graphics;
        private static BasicEffect basicEffect;

        private static void Init(GraphicsDevice graphicsDevice)
        {
            graphics = graphicsDevice;

            basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.TextureEnabled = true;

            spriteBatch = new SpriteBatch(graphicsDevice);
        }

        public static void Begin(GraphicsDevice graphicsDevice, Camera camera)
        {
            if (graphics == null)
            {
                Init(graphicsDevice);
            }

            graphics.Clear(camera.Color);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap,
                              DepthStencilState.DepthRead, RasterizerState.CullClockwise, null, camera.TranslationVirtualMatrix);
        }

        public static void End()
        {
            spriteBatch.End();
        }

        public static void Render(Transform transform, SpriteRender spriteRender)
        {
            if (spriteRender.sprite == null || spriteRender.sprite.Texture2D == null || !spriteRender.isVisible)
            {
                return;
            }

            Vector2 position = transform.ToAbsolutePosition(spriteRender.spriteOffset);
            float angle = transform.Rotation + MathF.PI;
            Vector2 scale = transform.Scale / spriteRender.sprite.PixelInUnit;
            float layer = (float)spriteRender.spriteLayer / maxSpriteLayers;

            spriteBatch.Draw(
                spriteRender.GetRegion(), position, spriteRender.color, angle,
                spriteRender.Origin, scale, spriteRender.SpriteEffect, layer);
        }

        public static void Render(Transform transform, MeshRender render, Mesh mesh)
        {
            if (mesh.Ind.Length == 0)
            {
                return;
            }

            basicEffect.View = Camera.Current.GetViewMatrix();
            basicEffect.Projection = Camera.GetProjectionMatrix();

            SetStates();

            if (render.matireal != null)
            {
                basicEffect.Texture = render.matireal.Texture;
            }            

            foreach (EffectPass effectPass in basicEffect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
                graphics.DrawUserIndexedPrimitives(PrimitiveType.TriangleList,
                    render.GetAbsolutVertices(transform, mesh), 0, mesh.Vertices.Length, mesh.Ind, 0, mesh.Ind.Length / 3);
            }

            void SetStates()
            {
                graphics.DepthStencilState = DepthStencilState.Default;
                graphics.SamplerStates[0] = SamplerState.PointWrap;
                graphics.RasterizerState = RasterizerState.CullCounterClockwise;
            }
        }
    }
}
