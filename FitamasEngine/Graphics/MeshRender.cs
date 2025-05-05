using Fitamas.Entities;
using Fitamas.Math2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fitamas.Graphics
{
    public class MeshRender : Component
    {
        public RectangleF TextureBounds = new RectangleF(Vector2.Zero, Vector2.One);
        public Matireal Matireal;

        public MeshRender()
        {

        }

        public MeshRender(Matireal matireal, RectangleF textureBounds)
        {
            this.Matireal = matireal;
            TextureBounds = textureBounds;
        }

        public VertexPositionTexture[] GetAbsolutVertices(Transform transform, Mesh mesh)
        {
            VertexPositionTexture[] result = new VertexPositionTexture[mesh.Vertices.Length];

            for (int i = 0; i < result.Length; i++)
            {
                result[i].Position = new Vector3(transform.ToAbsolutePosition(mesh.Vertices[i]), 0);
                Vector2 texturePos = mesh.Vertices[i];
                texturePos.X /= TextureBounds.Width;
                texturePos.Y /= TextureBounds.Height;
                result[i].TextureCoordinate = texturePos;
            }
            return result;
        }
    }
}
