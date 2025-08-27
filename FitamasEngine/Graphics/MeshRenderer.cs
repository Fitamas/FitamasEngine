using Fitamas.Math;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Fitamas.ECS;

namespace Fitamas.Graphics
{
    public class MeshRenderer : Renderer
    {
        private Transform transform;
        private MeshRendererComponent meshRendererComponent;
        private MeshComponent meshComponent;

        public MeshRenderer(Transform transform, MeshRendererComponent meshRendererComponent, MeshComponent meshComponent)
        {
            this.transform = transform;
            this.meshRendererComponent = meshRendererComponent;
            this.meshComponent = meshComponent;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (meshComponent.Ind.Length == 0)
            {
                return;
            }

            GraphicsDevice graphicsDevice = spriteBatch.GraphicsDevice;
            Material material = meshRendererComponent.Matireal ?? Material.DefaultMaterial;
            Effect effect = material.Effect;

            Matrix view = Camera.Current.GetViewMatrix();
            Matrix projection = Camera.Current.GetProjectionMatrix();
            Matrix.Multiply(ref view, ref projection, out var result);
            effect.Parameters["WorldViewProj"]?.SetValue(result);

            graphicsDevice.BlendState = material.BlendState;
            graphicsDevice.DepthStencilState = material.DepthStencilState;
            graphicsDevice.SamplerStates[0] = material.SamplerState;
            graphicsDevice.RasterizerState = RasterizerState.CullNone;

            VertexPositionColorTexture[] vertex = new VertexPositionColorTexture[meshComponent.Vertices.Length];
            float layer = (float)meshRendererComponent.Layer / Settings.LayersCount;

            for (int i = 0; i < vertex.Length; i++)
            {
                vertex[i].Position = new Vector3(transform.ToAbsolutePosition(meshComponent.Vertices[i]), layer);
                vertex[i].Color = Color.White;
                Vector2 texturePos = meshComponent.Vertices[i];
                texturePos.X /= meshRendererComponent.TextureBounds.Width;
                texturePos.Y /= meshRendererComponent.TextureBounds.Height;
                vertex[i].TextureCoordinate = texturePos;
            }

            foreach (EffectPass effectPass in effect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
                graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList,
                    vertex, 0, meshComponent.Vertices.Length, meshComponent.Ind, 0, meshComponent.Ind.Length / 3);
            }
        }
    }
}
