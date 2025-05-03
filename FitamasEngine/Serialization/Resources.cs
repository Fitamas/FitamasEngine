using Fitamas.Core;
using Microsoft.Xna.Framework.Content;
using System;

namespace Fitamas.Serialization
{
    public static class Resources
    {
        private static ContentManager manager => GameEngine.Instance.Content;

        public static string RootDirectory => manager.RootDirectory;

        public static T Load<T>(string name) where T : MonoObject
        {
            return manager.Load<T>(name);
        }

        public static void UnloadAll()
        {
            manager.Unload();
        }

        public static void Unload(string name)
        {
            manager.UnloadAsset(name);
        }
    }
}
