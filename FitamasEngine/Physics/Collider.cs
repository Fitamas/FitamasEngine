using Microsoft.Xna.Framework;
using MonoGame.Extended;
using nkast.Aether.Physics2D.Common;
using nkast.Aether.Physics2D.Dynamics;
using System.Collections.Generic;
using Fitamas.Math2D;
using nkast.Aether.Physics2D.Common.PolygonManipulation;
using nkast.Aether.Physics2D.Collision.Shapes;
using Fitamas.Serializeble;
using Fitamas.Entities;

namespace Fitamas.Physics
{
    public enum ColliderType
    {
        Box,
        Circle,
        Polygon,
        Capsule
    }

    public class Collider : Component
    {
        [SerializableField] private Layer layer = Layer.Defoult;
        [SerializableField] private BodyType bodyType = BodyType.Static;
        [SerializableField] private ColliderType colliderType = ColliderType.Box;
        [SerializableField] private float density = 1;
        [SerializableField] private bool isRotate = true;
        [SerializableField] private Vector2 offset = Vector2.Zero;
        //BOX
        [SerializableField] private Vector2 scale = Vector2.One;
        //[SerializableField] private RectangleF rectangle = new RectangleF(Vector2.Zero, Vector2.One);
        //CIRCLE
        [SerializableField] private float radius = 1;
        //POLYGON
        [SerializableField] private Vector2[][] colliderShapes;

        private List<Vertices> compositeShape;
        private LayerMask layerMask;
        private World world;
        private Body body;
        private Vector2 position = Vector2.Zero;
        private float rotation = 0;

        public Layer Layer => layer;
        public ColliderType ColliderType => colliderType;
        public Vector2 Offset => offset;
        public Vector2 Scale => scale;
        public RectangleF Rectangle => new RectangleF(position + offset, Scale);
        public float Radius => radius;
        public List<Vertices> CompositeShape => compositeShape; 
        public Body Body => body;

        public bool IsReady
        {
            get
            {
                return world != null && body != null;
            }
        }

        public BodyType BodyType
        {
            get
            {
                return bodyType;
            }
            set
            {
                if (body != null)
                {
                    body.BodyType = value;
                }
                bodyType = value;
            }
        }

        public Vector2 BodyPosition
        {
            get
            {
                if (body == null)
                {
                    return position;
                }

                return body.Position;
            }
            set
            {
                position = value;

                if (body != null)
                {
                    body.Position = value;
                }
            }
        }

        public Vector2 Position
        {
            get
            {
                if (body == null)
                {
                    return position - MathV.Rotate(offset, rotation);
                }

                return body.GetWorldPoint(-offset);
            }
            set
            {
                position = value + MathV.Rotate(offset, rotation);

                if (body != null)
                {
                    body.Position = position;
                }
            }
        }

        public float Rotation
        {
            get
            {
                if (body == null)
                {
                    return rotation;
                }

                return body.Rotation;
            }
            set
            {
                rotation = value;
                if (body != null)
                {
                    body.Rotation = value;
                }  
            }
        }

        public Collider()
        {

        }

        public Collider(ColliderType colliderType, Layer layer, BodyType bodyType)
        {
            this.layer = layer;
            this.bodyType = bodyType;
            this.colliderType = colliderType;
        }

        public Collider(ColliderType colliderType, Vector2 scale, Vector2 offset, float density = 1, Layer layer = Layer.Defoult, BodyType bodyType = BodyType.Static) 
            : this(colliderType, layer, bodyType)
        {
            this.scale = scale;
            this.bodyType = bodyType;
            this.density = density;
            this.offset = offset;
        }

        public Collider(float radius, Vector2 offset, float density = 1, Layer layer = Layer.Defoult, BodyType bodyType = BodyType.Static)
            : this(ColliderType.Circle, layer, bodyType)
        {
            this.radius = radius;
            this.bodyType = bodyType;
            this.density = density;
            this.offset = offset;
        }

        public Collider(Vector2[][] compositeShape, float density = 1, Layer layer = Layer.Defoult, BodyType bodyType = BodyType.Static) 
            : this(ColliderType.Polygon, layer, bodyType)
        {
            this.bodyType = bodyType;
            this.density = density;
            colliderShapes = compositeShape;

            Triangulator.Process(compositeShape, out Vector2[] verts, out int[] ind);
            CreatePolygonShape(verts, ind);
        }

        public Collider(Vector2[] verts, int[] ind, float density = 1, Layer layer = Layer.Defoult, BodyType bodyType = BodyType.Static) 
            : this(ColliderType.Polygon, layer, bodyType)
        {
            this.bodyType = bodyType;
            this.density = density;

            CreatePolygonShape(verts, ind);
        }

        public Vector2 ToAbsolutePosition(Vector2 local)
        {
            if (body == null)
            {
                return local;
            }

            return body.GetWorldPoint(local);
        }

        public void SetPoligons(Vector2[] verts, int[] ind)
        {
            CreatePolygonShape(verts, ind);

            if (body != null)
            {
                List<Fixture> delete = new List<Fixture>();
                foreach (var fixture in body.FixtureList)
                {
                    delete.Add(fixture);
                }

                foreach (var fixture in delete)
                {
                    body.Remove(fixture);
                }

                foreach (var shape in compositeShape)
                {
                    PolygonShape polygon = new PolygonShape(shape, 1);
                    body.CreateFixture(polygon);
                }
            }
        }

        private void CreatePolygonShape(Vector2[] verts, int[] ind)
        {
            List<Vertices> triangles = new List<Vertices>();
            int count = ind.Length / 3;

            for (int i = 0; i < count; i++)
            {
                Vector2[] polygon = new Vector2[3];
                for (int j = 0; j < 3; j++)
                {
                    int index = ind[i * 3 + j];
                    polygon[j] = verts[index];
                }
                Vertices triangle = new Vertices(polygon);
                triangle.ForceCounterClockWise();
                triangles.Add(triangle);
            }

            compositeShape = SimpleCombiner.PolygonizeTriangles(triangles);
        }

        public Body CreateBody(World _world, Entities.Transform transform)
        {
            Position = transform.Position;
            Rotation = transform.Rotation;

            return CreateBody(_world);
        }

        public Body CreateBody(World _world)
        {
            if (IsReady)
            {
                return body;
            }

            world = _world;
            layerMask = LayerManager.GetMask(layer);

            Category category = (Category)layer;
            Category collidesWith = layerMask.GetCategory();

            switch (colliderType)
            {
                case ColliderType.Box:
                {
                        body = world.CreateRectangle(scale.X, scale.Y, density,
                               position, rotation, bodyType);
                        break;
                }
                case ColliderType.Circle: 
                {
                        body = world.CreateCircle(radius, density, position, bodyType);
                        body.Rotation = rotation;
                        break;    
                }
                case ColliderType.Polygon:
                {
                        if (compositeShape == null)
                        {
                            if (colliderShapes != null)
                            {
                                Triangulator.Process(colliderShapes, out Vector2[] verts, out int[] ind);
                                CreatePolygonShape(verts, ind);
                            }
                            else
                            {
                                return null;
                            }
                        }

                        body = world.CreateBody(position, rotation, bodyType);

                        foreach (var shape in compositeShape)
                        {
                            PolygonShape polygon = new PolygonShape(shape, 1);
                            body.CreateFixture(polygon);
                        }

                        break;
                }
                case ColliderType.Capsule:
                {
                        body = world.CreateCapsule(scale.Y, scale.X, density, position, rotation, bodyType);
                        break;
                }
            }

            foreach (var fixture in body.FixtureList)
            {
                fixture.CollisionCategories = category;
                fixture.CollidesWith = collidesWith;
            }
            body.FixedRotation = !isRotate;

            return body;
        }

        public void RemoveBody()
        {
            if (IsReady)
            {
                world.Remove(body);
                world = null;
                body = null;
            }
        }
    }
}
