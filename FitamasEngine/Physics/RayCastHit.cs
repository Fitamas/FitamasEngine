using Fitamas.Entities;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;
using System;

namespace Fitamas.Physics
{
    public struct RayCastHit
    {
        public Vector2 Point { get; set; }
        public Vector2 Normal {  get; set; }
        public float Distance { get; set; }
        public Entity Entity { get; set; }
        public Collider Collider { get; set; }

        internal Body Body { get; set; }
    }
}
