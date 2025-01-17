using System;

namespace Fitamas.Serializeble
{
    public static class Resources
    {
        private static ObjectManager objectManager;

        public static string RootDirectory => objectManager.RootDirectory;

        public static void LoadContent(ObjectManager objectManager)
        {
            Resources.objectManager = objectManager;
        }

        public static MonoObject LoadObject(string name)
        {
            return objectManager.LoadAsset(name);
        }

        public static T Load<T>(string name) where T : MonoObject
        {
            return objectManager.LoadAsset<T>(name);
        }

        public static void Unload(string name)
        {
            objectManager.UnloadAsset(name);
        }
    }
}
