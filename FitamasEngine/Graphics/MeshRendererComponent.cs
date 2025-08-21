using Fitamas.ECS;
using Fitamas.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fitamas.Graphics
{
    public class MeshRendererComponent : Component
    {
        public RectangleF TextureBounds = new RectangleF(Vector2.Zero, Vector2.One);
        public Material Matireal;
        public int Layer;

        public MeshRendererComponent()
        {

        }

        public MeshRendererComponent(Material matireal, RectangleF textureBounds)
        {
            Matireal = matireal;
            TextureBounds = textureBounds;
        }
    }
}
