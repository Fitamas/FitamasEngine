using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Fitamas.Graphics.PostProcessors
{
    public abstract class PostProcessor
    {
        public bool Enabled;
        public readonly int ExecutionOrder;

        public Material Material;

        public PostProcessor(int executionOrder, Material material = null)
        {
            Enabled = true;
            ExecutionOrder = executionOrder;
            Material = material;
        }

        public virtual void Create()
        {

        }

        public virtual void OnCameraSetup(ref RenderingData renderingData)
        {

        }

        public virtual void Process(RenderContext context, RenderingData renderingData)
        {
            context.Blit(renderingData.Source, renderingData.Destination, Material);
        }
    }
}
