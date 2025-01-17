using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitamas.Physics
{
    public static class LayerManager
    {
        public static LayerMask GetMask(Layer layer)
        {
            Layer[] layers;
            switch (layer)
            {
                case Layer.Defoult:
                    layers = new Layer[] { Layer.Defoult, Layer.Interactive, Layer.Effects };
                    break;
                case Layer.Interactive:
                    layers = new Layer[] { Layer.Defoult, Layer.Interactive };
                    break;
                case Layer.Effects:
                    layers = new Layer[] { Layer.Defoult };
                    break;
                case Layer.Players:
                    layers = new Layer[] { Layer.Defoult };
                    break;
                case Layer.Projectile:
                    layers = new Layer[0];
                    break;
                default:
                    layers = new Layer[0];
                    break;
            }
            return new LayerMask(layers);
        }
    }
}
