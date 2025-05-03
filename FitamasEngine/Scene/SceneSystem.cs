using Fitamas.Entities;
using Fitamas.Serialization;

namespace Fitamas.Scene
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
                return;
            }

            Debug.Log("Open Scene: " + name);

            Scene scene = GetSceneByPath(name);

            LoadScene(scene);
        }

        public static void LoadScene(Scene scene)
        {
            if (scene == null)
            {
                return;
            }

            activeScene = scene;

            ClearScene();

            if (scene.GameObjects == null)
            {
                return;
            }

            foreach (var gameObject in scene.GameObjects)
            {
                Entity entity = entityManager.Create();
                entity.Name = gameObject.Name;

                foreach (var component in gameObject.Components)
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
