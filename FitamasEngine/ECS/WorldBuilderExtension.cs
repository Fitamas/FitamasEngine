using Fitamas.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitamas.ECS
{
    public static class WorldBuilderExtension
    {
        public static WorldBuilder AddSystemAndRegister<T>(this WorldBuilder builder, T system, DIContainer container) where T : class, ISystem
        {
            container.RegisterInstance(system);
            return builder.AddSystem(system);
        }
    }
}
