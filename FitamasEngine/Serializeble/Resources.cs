using Fitamas.Main;
using System;

namespace Fitamas.Serializeble
{
    public static class Resources
    {
        public static string RootDirectory => GameEngine.Instance.ObjectManager.RootDirectory;

        public static MonoObject Load(string name)
        {
            return GameEngine.Instance.ObjectManager.LoadAsset(name);
        }

        public static T Load<T>(string name) where T : MonoObject
        {
            return GameEngine.Instance.ObjectManager.LoadAsset<T>(name);
        }

        public static void Unload(string name)
        {
            GameEngine.Instance.ObjectManager.UnloadAsset(name);
        }
    }
}
