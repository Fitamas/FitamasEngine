using Fitamas.ECS;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Collision;
using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Common;
using nkast.Aether.Physics2D.Dynamics;
using System.Collections.Generic;
using Fitamas.Math;
using System;
using System.Linq;
using Fitamas.Core;
using nkast.Aether.Physics2D.Common.PhysicsLogic;


namespace Fitamas.Physics
{
    public static class PhysicsExtension
    {
        internal const int MaxCastDepth = 10;

        public static void Explosion(Vector2 position, float radius, float force)
        {
            //RealExplosion realExplosion;

            //realExplosion.Activate(position, radius, force);
        }

        public static RayCastHit TestPoint(this PhysicsWorldSystem physicsWorld, Vector2 position, CollisionFilter layers = default)
        {
            Fixture fixture = physicsWorld.World.TestPoint(position);

            if (fixture != null)
            {
                Entity entity = physicsWorld.GetEntity(fixture.Body);
                if (entity != null)
                {
                    return new RayCastHit()
                    {
                        Point = position,
                        Entity = entity,
                        Collider = entity.Get<PhysicsCollider>(),
                        Body = fixture.Body,
                    };
                }
            }

            return default;
        }

        public static RayCastHit[] RayCast(this PhysicsWorldSystem physicsWorld, Vector2 position1, Vector2 position2, CollisionFilter layers = default)
        {
            List<RayCastHit> hits = new List<RayCastHit>();
            Category category = layers.CollisionCategories;
            physicsWorld.World.RayCast(delegate (Fixture fixture, Vector2 point, Vector2 normal, float fraction)
            {
                if (category.HasFlag(fixture.CollisionCategories))
                {
                    RayCastHit hit = new RayCastHit();
                    hit.Point = point;
                    hit.Normal = normal;
                    hit.Body = fixture.Body;
                    hit.Distance = fraction;
                    hits.Add(hit);
                }
                return 1;
            }, position1, position2);

            return hits.ToArray();
        }

        public static RayCastHit[] OverlapCircle(this PhysicsWorldSystem physicsWorld, Vector2 position, float radius, CollisionFilter layers = default)
        {
            AABB aabb = default;
            aabb.LowerBound = position + new Vector2(-radius, -radius);
            aabb.UpperBound = position + new Vector2(radius, radius);

            CircleShape circle = new CircleShape(radius, 1);
            DistanceProxy circleProxy = new DistanceProxy(circle, 0);
            List<RayCastHit> hits = new List<RayCastHit>();
            Category category = layers.CollisionCategories;

            physicsWorld.World.QueryAABB(delegate (Fixture fixture)
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
                        hit.Normal = (output.PointA - output.PointB).NormalizeF();
                        hit.Distance = output.Distance;
                        hit.Point = output.PointB;
                        hit.Body = fixture.Body;
                        hits.Add(hit);
                    }
                }

                return true;
            }, ref aabb);

            return hits.ToArray();
        }

        public static RayCastHit[] CircleCast(this PhysicsWorldSystem physicsWorld, Vector2 origin, Vector2 direction, float distance, float radius, CollisionFilter layer = default)
        {
            if (distance <= 0 || direction == Vector2.Zero) 
            { 
                return OverlapCircle(physicsWorld, origin, radius);
            }

            direction = direction.NormalizeF();

            RayCastHit[] hits = new RayCastHit[0];
            int depth = 0;
            while (depth < MaxCastDepth)
            {
                Vector2 position = origin + direction * distance;
                RayCastHit[] hitsCurrent = OverlapCircle(physicsWorld, position, radius, layer);
                if (hitsCurrent.Length == 0 || distance <= 0 || direction == Vector2.Zero)
                {
                    break;
                }

                hits = hitsCurrent;
                float minDistance = distance;

                for (int i = 0; i < hits.Length; i++)
                {
                    Vector2 point = hits[i].Point;
                    Vector2 result = MathV.ProjectOnTo(origin, origin + direction, point);
                    float a = Vector2.Distance(point, result);
                    float b = MathF.Sqrt(MathF.Abs(radius * radius - a * a));
                    float currentDistance = Vector2.Distance(origin, result) - b;

                    if (currentDistance <= 0.001f)
                    {
                        currentDistance = 0;
                        hits[i].Distance = 0;
                    }
                    else
                    {
                        hits[i].Distance = currentDistance;
                        
                    }
                    hits[i].Normal = (origin + direction * currentDistance - point).NormalizeF();
                    if (minDistance > currentDistance)
                    {
                        minDistance = currentDistance;
                    }
                }

                distance = minDistance;
                depth++;
            }
            var sortedValues = hits.OrderBy(x => x.Distance).ToArray();

            return sortedValues;
        }

        public static RayCastHit[] OverlapBox(this PhysicsWorldSystem physicsWorld, Vector2 position, float width, float height, float angle,  CollisionFilter layers = default)
        {
            float halfWidth = width / 2;
            float halfHeight = height / 2;
            AABB aabb = default;
            aabb.LowerBound = position + new Vector2(-halfWidth, -halfHeight);
            aabb.UpperBound = position + new Vector2(halfWidth, halfHeight);

            Vertices vert = new Vertices(
                [
                    new Vector2(-halfWidth, halfHeight),
                    new Vector2(halfWidth, halfHeight),
                    new Vector2(halfWidth, -halfHeight),
                    new Vector2(-halfWidth, -halfHeight),
                ]);

            PolygonShape polygonShape = new PolygonShape(vert, 1);
            DistanceProxy circleProxy = new DistanceProxy(polygonShape, 0);
            List<RayCastHit> hits = new List<RayCastHit>();
            Category category = layers.CollisionCategories;

            physicsWorld.World.QueryAABB((fixture) =>
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
                        hit.Point = point;
                        hit.Distance = minDistance;
                        hit.Normal = normal;
                        hit.Body = fixture.Body;
                        hits.Add(hit);
                    }
                }

                return true;
            }, ref aabb);

            return hits.ToArray();
        }

        public static RayCastHit[] BoxCast(this PhysicsWorldSystem physicsWorld, Vector2 position, Vector2 direction, float distance, float width, float height, float angle,  CollisionFilter layers = default)
        {
            position += direction.NormalizeF() * distance;




            //TODO box cast




            RayCastHit[] hits = OverlapBox(physicsWorld, position, width, height, angle, layers);


            return hits;
        }

        public static RayCastHit[] CapsuleCast(this PhysicsWorldSystem physicsWorld, Vector2 position, Vector2 direction, float distance, Vector2 scale, CollisionFilter layers = default)
        {
            float radius = scale.X;
            float width = scale.X * 2;
            float hight = scale.Y;
            Vector2 up = position + new Vector2(0, hight/2);
            Vector2 down = position + new Vector2(0, -hight/2);
            List<RayCastHit> hits = new List<RayCastHit>();
            hits.AddRange(CircleCast(physicsWorld, up, direction, distance, radius, layers));
            hits.AddRange(CircleCast(physicsWorld, down, direction, distance, radius, layers));
            hits.AddRange(BoxCast(physicsWorld, position, direction, distance, width, hight, 0, layers));
            return hits.OrderBy(x => x.Distance).ToArray();
        }

        public static RayCastHit[] CapsuleCast(this PhysicsWorldSystem physicsWorld, PhysicsCollider collider, Vector2 position, Vector2 direction, float distance, CollisionFilter layers = default)
        {
            RayCastHit[] castHits = CapsuleCast(physicsWorld, position, direction, distance, collider.Size, layers);
            List<RayCastHit> hits = new List<RayCastHit>();

            foreach (var hit in castHits)
            {
                if (hit.Collider != collider)
                {
                    hits.Add(hit);
                }
            }

            return hits.OrderBy(x => x.Distance).ToArray();
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
