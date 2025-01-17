using Fitamas.Entities;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;
using System;

namespace Fitamas.Physics
{
    public struct RayCastHit
    {
        public Vector2 point { get; set; }
        public Vector2 normal {  get; set; }
        public Entity entity { get; set; }
        public Body body { get; set; }
        public float distance { get; set; }
    }
}
