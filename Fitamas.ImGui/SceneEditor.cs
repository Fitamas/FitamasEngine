using Fitamas.Core;
using Fitamas.ECS;
using Fitamas.ImGuiNet.Assets;
using Fitamas.ImGuiNet.Windows;
using Fitamas.Scenes;
using System;
using System.Collections.Generic;

namespace Fitamas.ImGuiNet
{
    public static class SceneEditor
    {
        private static Scene selectScene;
        private static bool isDirty = false;
        private static bool isInitialized = false;

        public static Scene SelectScene => selectScene;
        public static bool IsDirty => isDirty;

        internal static void Initialize()
        {
            if (isInitialized)
            {
                return;
            }

            isInitialized = true;

            EntityManager entityManager = GameEngine.Instance.GameWorld.EntityManager;
            entityManager.EntityAdded += SetDirty;
            entityManager.EntityRemoved += SetDirty;
            entityManager.EntityChanged += SetDirty;

            if (AssetDatabase.Contains(Scene.MainScene))
            {
                selectScene = AssetDatabase.LoadAsset<Scene>(Scene.MainScene);
            }
            else
            {
                Asset asset = AssetDatabase.CreateAsset(Scene.MainScene, typeof(Scene));
                asset.TryLoad(out selectScene);
            }

            OpenScene(selectScene);

            AssetDatabase.OnSaveProject += UpdateData;
        }

        public static void SetDirty(int id)
        {
            isDirty = true;
        }

        public static void OpenScene(Scene scene)
        {
            if (scene == null)
            {
                throw new ArgumentNullException(nameof(scene));
            }

            selectScene = scene;
            SceneSystem.LoadScene(selectScene);
        }

        public static void LoadScene(string path)
        {
            OpenScene(AssetDatabase.LoadAsset<Scene>(path));
        }

        public static void SaveScene()
        {
            UpdateData();
            AssetDatabase.SaveAsset(selectScene);
        }

        public static void SaveAsNewScene(string path)
        {
            UpdateData();
            AssetDatabase.CreateAsset(path, selectScene);
        }

        private static void UpdateData()
        {
            EntityManager entityManager = GameEngine.Instance.GameWorld.EntityManager;
            ComponentManager componentManager = GameEngine.Instance.GameWorld.ComponentManager;

            selectScene.Entities.Clear();

            foreach (var entityId in entityManager.Entities)
            {
                List<Component> components = new List<Component>();
                foreach (var mapper in componentManager.ComponentMappers)
                {
                    if (mapper != null && mapper.ComponentType.IsAssignableFrom(typeof(Component)) && mapper.Has(entityId))
                    {
                        components.Add(mapper.GetObject(entityId) as Component);
                    }
                }

                Entity entity = entityManager.Get(entityId);
                EntityData entityData = new EntityData(entity.Name, components);
                selectScene.Entities.Add(entityData);
            }
        }
    }
}
