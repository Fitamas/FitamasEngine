using nkast.Aether.Physics2D.Dynamics;
using System;

namespace Fitamas.Physics
{
    public struct CollisionFilter
    {
        private static readonly CollisionFilter defaultFilter = new CollisionFilter(Category.Cat1, Category.All);

        private static readonly CollisionFilter emptyFilter = new CollisionFilter(Category.None, Category.None);

        public static CollisionFilter DefaultFilter => defaultFilter;

        public static CollisionFilter EmptyFilter => emptyFilter;

        internal Category CollisionCategories;
        internal Category CollidesWith;

        public bool IsEmpty => CollisionCategories == Category.None && CollidesWith == Category.None;

        public uint CollisionCategoriesHash
        {
            get
            {
                return (uint)CollisionCategories;
            }
            set
            {
                CollisionCategories = (Category)value;
            }
        }

        public uint CollidesWithHash
        {
            get
            {
                return (uint)CollidesWith;
            }
            set
            {
                CollidesWith = (Category)value;
            }
        }

        internal CollisionFilter(Category collisionCategories, Category collidesWith)
        {
            CollisionCategories = collisionCategories;
            CollidesWith = collidesWith;
        }

        public CollisionFilter(uint collisionCategories, uint collidesWith)
        {
            CollisionCategoriesHash = collisionCategories;
            CollidesWithHash = collidesWith;
        }
    }
}