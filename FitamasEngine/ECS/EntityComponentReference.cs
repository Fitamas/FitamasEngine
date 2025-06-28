using R3;
using System;

namespace Fitamas.ECS
{
    public class EntityComponentReference<T> : IDisposable where T : class
    {
        private Entity entity;
        private T component;
        private ReactiveProperty<bool> isAlive;

        public Entity Entity => entity;
        public T Component => component;
        public ReactiveProperty<bool> IsAlive => isAlive;

        public EntityComponentReference(Entity entity, T component)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (component == null)
            {
                throw new ArgumentNullException(nameof(component));
            }

            if (entity.Get<T>() != component)
            {
                throw new ArgumentException();
            }

            this.entity = entity;
            this.component = component;
            ComponentMapper<T> mapper = entity.ComponentManager.GetMapper<T>();
            mapper.OnDelete += OnComponentDelete;
            isAlive = new ReactiveProperty<bool>(true);
        }

        private void OnComponentDelete(int entityId)
        {
            if (entity.Id == entityId)
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            ComponentMapper<T> mapper = entity.ComponentManager.GetMapper<T>();
            mapper.OnDelete -= OnComponentDelete;
            entity = null;
            component = null;
            isAlive.Value = false;
            isAlive.Dispose();
        }
    }
}
