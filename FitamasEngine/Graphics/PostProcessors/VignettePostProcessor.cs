using Fitamas.Core;
using Fitamas.Serialization;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Fitamas.Graphics.PostProcessors
{
    public class VignettePostProcessor : PostProcessor
    {
        private float power = 1f;
        private float radius = 1.25f;
        private EffectParameter powerParam;
        private EffectParameter radiusParam;

        public float Power
        {
            get => power;
            set
            {
                if (power != value)
                {
                    power = value;

                    if (Material.Effect != null)
                    {
                        powerParam.SetValue(power);
                    }
                }
            }
        }

        public float Radius
        {
            get => radius;
            set
            {
                if (radius != value)
                {
                    radius = value;

                    if (Material.Effect != null)
                    {
                        radiusParam.SetValue(radius);
                    }
                }
            }
        }

        public VignettePostProcessor(int executionOrder) : base(executionOrder)
        {

        }

        public override void Create()
        {
            Effect effect = Resources.Load<Effect>("Effects\\Vignette");
            Material = new Material(effect);
            Material.BlendState = BlendState.Opaque;

            powerParam = Material.Effect.Parameters["power"];
            radiusParam = Material.Effect.Parameters["radius"];
            powerParam.SetValue(power);
            radiusParam.SetValue(radius);
        }
    }
}
