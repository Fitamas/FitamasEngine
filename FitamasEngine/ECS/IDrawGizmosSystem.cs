using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitamas.ECS
{
    public interface IDrawGizmosSystem : ISystem
    {
        void DrawGizmos();
    }
}
