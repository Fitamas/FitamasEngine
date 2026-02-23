using Fitamas.ECS;
using Fitamas.Serialization;
using System;

namespace Fitamas.Scenes
{
    public class SceneSystem : ISystem
    {
        private static EntityManager entityManager;
        private static Scene activeScene;

        public void Initialize(GameWorld world)
        {
            entityManager = world.EntityManager;
        }

        public void Dispose()
        {

        }

        public static void LoadScene(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            Debug.Log("Open Scene: " + name);

            Scene scene = GetSceneByPath(name);

            LoadScene(scene);
        }

        public static void LoadScene(Scene scene)
        {
            if (scene == null)
            {
                throw new ArgumentNullException(nameof(scene));
            }

            activeScene = scene;

            ClearScene();

            if (scene.Entities == null)
            {
                return;
            }

            foreach (var entityData in scene.Entities)
            {
                Entity entity = entityManager.Create(entityData.Name);

                foreach (var component in entityData.Components)
                {
                    entity.Attach(component.GetType(), component);
                }
            }
        }

        private static void ClearScene()
        {
            foreach (var entity in entityManager.Entities)
            {
                entityManager.Destroy(entity);
            }
        }

        public static Scene GetSceneByPath(string name)
        {
            return Resources.Load<Scene>(name);
        }

        public static Scene GetActiveScene()
        {
            return activeScene;
        }
    }
}
