using Fitamas.ECS;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Fitamas.Math;
using Fitamas.DebugTools;

namespace Fitamas.Graphics
{
    public class MeshRenderSystem : EntityDrawSystem, IDrawGizmosSystem
    {
        private GraphicsDevice graphics;
        private AlphaTestEffect effect;

        private ComponentMapper<Transform> transformMapper;
        private ComponentMapper<MeshRender> renderMapper;
        private ComponentMapper<Mesh> meshMapper;

        public MeshRenderSystem(GraphicsDevice graphicsDevice) : base(Aspect.All(typeof(Transform), typeof(MeshRender), typeof(Mesh)))
        {
            graphics = graphicsDevice;

            effect = new AlphaTestEffect(graphicsDevice);
            //basicEffect.TextureEnabled = true;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform>();
            renderMapper = mapperService.GetMapper<MeshRender>();
            meshMapper = mapperService.GetMapper<Mesh>();
        }

        public override void Draw(GameTime gameTime)
        {
            if (Camera.Current == null)
            {
                return;
            }

            foreach (var entityId in ActiveEntities)
            {
                MeshRender render = renderMapper.Get(entityId);
                Mesh mesh = meshMapper.Get(entityId);
                Transform transform = transformMapper.Get(entityId);

                Render(transform, render, mesh);
            }
        }

        public void Render(Transform transform, MeshRender render, Mesh mesh)
        {
            if (Camera.Current == null)
            {
                return;
            }

            if (mesh.Ind.Length == 0)
            {
                return;
            }

            effect.View = Camera.Current.GetViewMatrix();
            effect.Projection = Camera.Current.GetProjectionMatrix();
            graphics.DepthStencilState = DepthStencilState.Default;
            graphics.SamplerStates[0] = SamplerState.PointWrap;
            graphics.RasterizerState = RasterizerState.CullCounterClockwise;
            graphics.BlendState = BlendState.AlphaBlend;

            if (render.Matireal != null)
            {
                effect.Texture = render.Matireal.Texture;
            }

            VertexPositionTexture[] vertex = new VertexPositionTexture[mesh.Vertices.Length];
            float layer = (float)render.Layer / Settings.LayersCount;

            for (int i = 0; i < vertex.Length; i++)
            {
                vertex[i].Position = new Vector3(transform.ToAbsolutePosition(mesh.Vertices[i]), layer);
                Vector2 texturePos = mesh.Vertices[i];
                texturePos.X /= render.TextureBounds.Width;
                texturePos.Y /= render.TextureBounds.Height;
                vertex[i].TextureCoordinate = texturePos;
            }

            foreach (EffectPass effectPass in effect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
                graphics.DrawUserIndexedPrimitives(PrimitiveType.TriangleList,
                    vertex, 0, mesh.Vertices.Length, mesh.Ind, 0, mesh.Ind.Length / 3);
            }
        }

        public void DrawGizmos()
        {
            foreach (var entityId in ActiveEntities)
            {
                Mesh mesh = meshMapper.Get(entityId);
                Transform transform = transformMapper.Get(entityId);

                Gizmos.DrawPolygon(transform.Position, transform.Rotation, mesh, Color.Black);
            }
        }
    }
}
