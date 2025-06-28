using R3;
using System;

namespace Fitamas.ECS
{
    public class EntityReference : IDisposable
    {
        private Entity entity;
        private ReactiveProperty<bool> isAlive;

        public Entity Entity => entity;
        public ReadOnlyReactiveProperty<bool> IsAlive => isAlive;

        public EntityReference(Entity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            this.entity = entity;
            entity.EntityManager.EntityRemoved += OnEntityRemoved;
            isAlive = new ReactiveProperty<bool>(true);
        }

        private void OnEntityRemoved(int entityId)
        {
            if (entity.Id == entityId)
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            entity.EntityManager.EntityRemoved -= OnEntityRemoved;
            entity = null;
            isAlive.Value = false;
            isAlive.Dispose();
        }
    }
}
