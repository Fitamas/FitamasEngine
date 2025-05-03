using Fitamas.Serialization.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fitamas.Core
{
    public static class PlayerPrefs
    {
        public static readonly string RootDirectory = Path.Combine(Application.DataPath, "PlayerData.json");

        private static Dictionary<string, string> dataMap = new Dictionary<string, string>();

        internal static void Load()
        {
            if (!File.Exists(RootDirectory))
            {
                return;
            }

            dataMap = JsonUtility.Load<Dictionary<string, string>>(RootDirectory);
        }

        public static void Save()
        {
            JsonUtility.Save(RootDirectory, dataMap);
        }

        public static void DeleteAll()
        {
            dataMap.Clear();
        }

        public static void DeleteKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw  new ArgumentNullException("key");
            }

            dataMap.Remove(key);
        }

        public static bool HasKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            return dataMap.ContainsKey(key);
        }

        public static void SetString(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            dataMap[key] = value;
        }

        public static string GetString(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            return dataMap[key];
        }
    }
}
