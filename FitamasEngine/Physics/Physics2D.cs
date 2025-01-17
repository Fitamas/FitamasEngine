using Fitamas.Entities;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Collision;
using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Common;
using nkast.Aether.Physics2D.Common.PhysicsLogic;
using nkast.Aether.Physics2D.Dynamics;
using System.Collections.Generic;
using MonoGame.Extended;
using Fitamas.Math2D;
using System;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;


namespace Fitamas.Physics
{
    public static class Physics2D
    {
        private static PhysicsSystem system;

        private static RealExplosion realExplosion;

        public static World world => system.physicsWorld;
        public const float FixedTimeStep = 0.02f;
        public const int MaxCastDepth = 10;

        public static void Init(PhysicsSystem m_system)
        {
            system = m_system;

            realExplosion = new RealExplosion(world);
        }

        public static void Explosion(Vector2 position, float radius, float force)
        {
            //realExplosion.Activate(position, radius, force);
        }

        public static RayCastHit[] RayCast(Vector2 position1, Vector2 position2, LayerMask layers = default)
        {
            List<RayCastHit> hits = new List<RayCastHit>();
            Category category = layers.GetCategory();
            world.RayCast(delegate (Fixture fixture, Vector2 point, Vector2 normal, float fraction)
            {
                if (category.HasFlag(fixture.CollisionCategories))
                {
                    RayCastHit hit = new RayCastHit();
                    hit.point = point;
                    hit.normal = normal;
                    hit.body = fixture.Body;
                    hit.distance = fraction;
                    hits.Add(hit);
                }
                return 1;
            }, position1, position2);

            return hits.ToArray();
        }

        public static RayCastHit[] OverlapCircle(Vector2 position, float radius, LayerMask layers = default)
        {
            AABB aabb = default;
            aabb.LowerBound = position + new Vector2(-radius, -radius);
            aabb.UpperBound = position + new Vector2(radius, radius);

            CircleShape circle = new CircleShape(radius, 1);
            DistanceProxy circleProxy = new DistanceProxy(circle, 0);
            List<RayCastHit> hits = new List<RayCastHit>();
            Category category = layers.GetCategory();

            world.QueryAABB(delegate (Fixture fixture)
            {
                if (category.HasFlag(fixture.CollisionCategories))
                {
                    DistanceInput input = new DistanceInput();
                    input.ProxyA = circleProxy;
                    input.TransformA = new nkast.Aether.Physics2D.Common.Transform(position, 0);
                    input.ProxyB = new DistanceProxy(fixture.Shape, 0);
                    input.TransformB = fixture.Body.GetTransform();

                    Distance.ComputeDistance(out DistanceOutput output, out SimplexCache cache, input);

                    if (output.Distance <= radius)
                    {
                        RayCastHit hit = new RayCastHit();
                        hit.normal = (output.PointA - output.PointB).NormalizeF();
                        hit.distance = output.Distance;
                        hit.point = output.PointB;
                        hit.body = fixture.Body;
                        hits.Add(hit);
                    }
                }

                return true;
            }, ref aabb);

            return hits.ToArray();
        }

        public static RayCastHit[] CircleCast(Vector2 origin, Vector2 direction, float distance, float radius, LayerMask layer = default)
        {
            if (distance <= 0 || direction == Vector2.Zero) 
            { 
                return OverlapCircle(origin, radius);
            }

            direction = direction.NormalizeF();

            RayCastHit[] hits = new RayCastHit[0];
            int depth = 0;
            while (depth < MaxCastDepth)
            {
                Vector2 position = origin + direction * distance;
                RayCastHit[] hitsCurrent = OverlapCircle(position, radius, layer);
                if (hitsCurrent.Length == 0 || distance <= 0 || direction == Vector2.Zero)
                {
                    break;
                }

                hits = hitsCurrent;
                float minDistance = distance;

                for (int i = 0; i < hits.Length; i++)
                {
                    Vector2 point = hits[i].point;
                    Vector2 result = MathV.ProjectOnTo(origin, origin + direction, point);
                    float a = Vector2.Distance(point, result);
                    float b = MathF.Sqrt(MathF.Abs(radius * radius - a * a));
                    float currentDistance = Vector2.Distance(origin, result) - b;

                    if (currentDistance <= 0.001f)
                    {
                        currentDistance = 0;
                        hits[i].distance = 0;
                    }
                    else
                    {
                        hits[i].distance = currentDistance;
                        
                    }
                    hits[i].normal = (origin + direction * currentDistance - point).NormalizeF();
                    if (minDistance > currentDistance)
                    {
                        minDistance = currentDistance;
                    }
                }

                distance = minDistance;
                depth++;
            }
            var sortedValues = hits.OrderBy(x => x.distance).ToArray();

            return sortedValues;
        }

        public static RayCastHit[] OverlapBox(Vector2 position, float width, float height, float angle,  LayerMask layers = default)
        {
            float halfWidth = width / 2;
            float halfHeight = height / 2;
            AABB aabb = default;
            aabb.LowerBound = position + new Vector2(-halfWidth, -halfHeight);
            aabb.UpperBound = position + new Vector2(halfWidth, halfHeight);

            Vertices vert = new Vertices(
                new Vector2[4]
                {
                    new Vector2(-halfWidth, halfHeight),
                    new Vector2(halfWidth, halfHeight),
                    new Vector2(halfWidth, -halfHeight),
                    new Vector2(-halfWidth, -halfHeight),
                });

            PolygonShape polygonShape = new PolygonShape(vert, 1);
            DistanceProxy circleProxy = new DistanceProxy(polygonShape, 0);
            List<RayCastHit> hits = new List<RayCastHit>();
            Category category = layers.GetCategory();

            world.QueryAABB(delegate (Fixture fixture)
            {
                if (category.HasFlag(fixture.CollisionCategories))
                {
                    DistanceInput input = new DistanceInput();
                    input.ProxyA = circleProxy;
                    input.TransformA = new nkast.Aether.Physics2D.Common.Transform(position, angle);
                    input.ProxyB = new DistanceProxy(fixture.Shape, 0);
                    input.TransformB = fixture.Body.GetTransform();

                    Distance.ComputeDistance(out DistanceOutput output, out SimplexCache cache, input);

                    if (output.Distance <= 0)
                    {
                        Vector2 point = output.PointB;
                        Vector2 normal = (point - position).NormalizeF();
                        float minDistance = 0;
                        if (fixture.Shape is PolygonShape shape)
                        {
                            Vector2 localPoint = fixture.Body.GetLocalPoint(ref point);
                            minDistance = float.MaxValue;

                            for (int i = 0; i < shape.Vertices.Count; i++)
                            {

                                Vector2 normalSegment = shape.Normals[i];
                                Vector2 vector = shape.Vertices[i];
                                float C = -normalSegment.X * vector.X - normalSegment.Y * vector.Y;
                                float distance = MathF.Abs(normalSegment.X * localPoint.X + normalSegment.Y * localPoint.Y + C)
                                                / (normalSegment.X * normalSegment.X + normalSegment.Y * normalSegment.Y);

                                if (distance < minDistance)
                                {
                                    normal = normalSegment;
                                    minDistance = distance;
                                }
                            }
                        }

                        RayCastHit hit = new RayCastHit();
                        hit.point = point;
                        hit.distance = minDistance;
                        hit.normal = normal;
                        hit.body = fixture.Body;
                        hits.Add(hit);
                    }
                }

                return true;
            }, ref aabb);

            return hits.ToArray();
        }

        public static RayCastHit[] BoxCast(Vector2 position, Vector2 direction, float distance, float width, float height, float angle,  LayerMask layers = default)
        {
            position += direction.NormalizeF() * distance;




            //TODO box cast




            RayCastHit[] hits = OverlapBox(position, width, height, angle, layers);


            return hits;
        }

        public static RayCastHit[] CapsuleCast(Vector2 position, Vector2 direction, float distance, Vector2 scale, LayerMask layers = default)
        {
            float radius = scale.X;
            float width = scale.X * 2;
            float hight = scale.Y;
            Vector2 up = position + new Vector2(0, hight/2);
            Vector2 down = position + new Vector2(0, -hight/2);
            List<RayCastHit> hits = new List<RayCastHit>();
            hits.AddRange(CircleCast(up, direction, distance, radius, layers));
            hits.AddRange(CircleCast(down, direction, distance, radius, layers));
            hits.AddRange(BoxCast(position, direction, distance, width, hight, 0, layers));
            return hits.OrderBy(x => x.distance).ToArray();
        }

        public static RayCastHit[] CapsuleCast(this Collider collider, Vector2 position, Vector2 direction, float distance, LayerMask layers = default)
        {
            RayCastHit[] castHits = CapsuleCast(position, direction, distance, collider.Scale, layers);
            List<RayCastHit> hits = new List<RayCastHit>();

            foreach (var hit in castHits)
            {
                if (hit.body != collider.Body)
                {
                    hits.Add(hit);
                }
            }

            return hits.OrderBy(x => x.distance).ToArray();
        }

        public static bool TryGetEntity(Body body, out Entity entity)
        {
            return system.TryGetEntity(body, out entity);
        }

        public static bool ClosestPoint(Vector2 position, out Vector2 point, out Vector2 normal) //TODO
        {
            point = new Vector2();
            normal = new Vector2();


            //a = dist(startp, endp)
            //b = dist(startp, p)
            //c = dist(endp, p)
            //s = (a + b + c) / 2
            //distance = 2 * sqrt(s(s - a)(s - b)(s - c)) / a


            //for (int i = 0; i < _vertices.Count; i++)
            //{
            //    int index = ((i + 1 < _vertices.Count) ? (i + 1) : 0);
            //    Vector2 vector = _vertices[index] - _vertices[i];
            //    Vector2 item = new Vector2(vector.Y, 0f - vector.X);
            //    item.Normalize();
            //    _normals.Add(item);
            //}




            //fixture.Shape.TestPoint();
            //for (int i = 0; i < Vertices.Count; i++)
            //{
            //    if (Vector2.Dot(Normals[i], vector - Vertices[i]) > 0f)
            //    {
            //        return false;
            //    }
            //}

            //PolygonShape 
            return false;
        }
    }
}
