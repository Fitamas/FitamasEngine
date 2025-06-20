using Microsoft.Xna.Framework;
using Fitamas.Math;
using Fitamas.Serialization;
using Fitamas.ECS;
using System.Linq;

namespace Fitamas.Physics
{
    public enum ColliderType
    {
        Box,
        Circle,
        Polygon,
        Capsule
    }

    public class PhysicsCollider : Component
    {
        [SerializeField] private ColliderType colliderType;
        [SerializeField] private Vector2 offset;
        [SerializeField] private Vector2 size;
        [SerializeField] private float radius;
        [SerializeField] private Vector2[][] colliderShapes;

        public ColliderType ColliderType => colliderType;
        public Vector2 Offset => offset;
        public Vector2 Size => size;
        public float Radius => radius;
        public Vector2[][] ColliderShapes => colliderShapes;

        public PhysicsCollider()
        {

        }

        public static PhysicsCollider CreateBox(Vector2 size, Vector2 offset = default)
        {
            PhysicsCollider collider = new PhysicsCollider();
            collider.colliderType = ColliderType.Box;
            collider.offset = offset;
            collider.size = size;
            return collider;
        }

        public static PhysicsCollider CreateCircle(float radius, Vector2 offset = default)
        {
            PhysicsCollider collider = new PhysicsCollider();
            collider.colliderType = ColliderType.Circle;
            collider.offset = offset;
            collider.radius = radius;
            return collider;
        }

        public static PhysicsCollider CreatePolygon(Vector2[][] colliderShapes, Vector2 offset = default)
        {
            PhysicsCollider collider = new PhysicsCollider();
            collider.colliderType = ColliderType.Polygon;
            collider.offset = offset;
            collider.colliderShapes = colliderShapes;
            return collider;
        }
    }
}
