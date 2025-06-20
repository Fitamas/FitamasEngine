using Fitamas.ECS;
using Fitamas.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fitamas.Graphics
{
    public class MeshRender : Component
    {
        public RectangleF TextureBounds = new RectangleF(Vector2.Zero, Vector2.One);
        public Matireal Matireal;
        public int Layer;

        public MeshRender()
        {

        }

        public MeshRender(Matireal matireal, RectangleF textureBounds)
        {
            Matireal = matireal;
            TextureBounds = textureBounds;
        }
    }
}
