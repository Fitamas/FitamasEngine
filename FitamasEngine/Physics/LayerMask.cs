using Fitamas.Serialization;
using nkast.Aether.Physics2D.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fitamas.Physics
{
    public enum Layer
    {
        Defoult = Category.Cat1,
        Interactive = Category.Cat2,
        Effects = Category.Cat3,
        Players = Category.Cat4,
        Projectile = Category.Cat5,
    }

    public struct LayerMask
    {
        [SerializableField] private List<Layer> mask = new List<Layer>();

        public Layer[] Mask => mask.ToArray();

        public LayerMask(IEnumerable<Layer> layers)
        {
            mask = layers.ToList();
        }

        public void SetMask(IEnumerable<Layer> layers)
        {
            mask = layers.ToList();
        }

        public void IgnoreMask(Layer[] layers)
        {
            for (int i = 0; i < layers.Length; i++) 
            { 
                if (mask.Contains(layers[i]))
                {
                    mask.Remove(layers[i]);
                }
            }
        }

        public Category GetCategory()
        {
            Category result = Category.None;

            if (mask != null)
            {
                foreach (Layer layer in mask)
                {
                    result = result | (Category)layer;
                }
            }
            else
            {
                return Category.All;
            }

            return result;
        }
    }
}
