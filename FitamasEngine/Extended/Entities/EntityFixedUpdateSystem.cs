using Fitamas.Entities;

namespace Fitamas.Extended.Entities
{
    public abstract class EntityFixedUpdateSystem : EntitySystem, IFixedUpdateSystem
    {
        public EntityFixedUpdateSystem(AspectBuilder aspectBuilder) : base(aspectBuilder)
        {

        }

        public abstract void FixedUpdate(float deltaTime);
    }
}
