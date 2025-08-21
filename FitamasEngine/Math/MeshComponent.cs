using Fitamas.ECS;
using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Math
{
    public class MeshComponent : Component
    {
        [SerializeField] private Vector2[][] shapes = new Vector2[0][];

        private Vector2[] vertices;
        private int[] ind;

        public Vector2[] Vertices
        {
            get
            {
                CheckVerts();
                return vertices;
            }
        }

        public int[] Ind
        {
            get
            {
                CheckVerts();
                return ind;
            }
        }

        public int TriangleCount
        {
            get
            {
                CheckVerts();
                return ind.Length / 3;
            }
        }

        public Vector2[][] Shapes
        {
            get
            {
                return shapes;
            }
            set
            {
                shapes = value;

                Triangulator.Process(shapes, out Vector2[] verts, out int[] ind);

                vertices = verts;
                this.ind = ind;
            }
        }

        public MeshComponent()
        {

        }

        public MeshComponent(Vector2[][] shapes)
        {
            Shapes = shapes;
        }

        private void CheckVerts()
        {
            if (vertices == null || ind == null)
            {
                Triangulator.Process(shapes, out Vector2[] vertices, out int[] ind);
                this.vertices = vertices;
                this.ind = ind;
            }
        }

        //    public IEnumerable<Vector2> Points { get; private set; }
        //    public float Left => Points.Min(p => p.X);
        //    public float Top => Points.Min(p => p.Y);
        //    public float Right => Points.Max(p => p.X);
        //    public float Bottom => Points.Max(p => p.Y);

        //    public RectangleF BoundingRectangle
        //    {
        //        get
        //        {
        //            var minX = Left;
        //            var minY = Top;
        //            var maxX = Right;
        //            var maxY = Bottom;
        //            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        //        }
        //    }
    }
}
