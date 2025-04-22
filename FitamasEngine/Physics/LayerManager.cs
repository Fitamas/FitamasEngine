using System;

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
                    layers = [Layer.Defoult, Layer.Interactive, Layer.Effects];
                    break;
                case Layer.Interactive:
                    layers = [Layer.Defoult, Layer.Interactive];
                    break;
                case Layer.Effects:
                    layers = [Layer.Defoult];
                    break;
                case Layer.Players:
                    layers = [Layer.Defoult];
                    break;
                case Layer.Projectile:
                    layers = [];
                    break;
                default:
                    layers = [];
                    break;
            }
            return new LayerMask(layers);
        }
    }
}
