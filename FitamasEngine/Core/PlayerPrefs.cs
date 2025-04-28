using System;
using System.Collections.Generic;
using System.IO;

namespace Fitamas.Core
{
    public static class PlayerPrefs
    {
        public static readonly string RootDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DirectoryName);

        public static string DirectoryName = "FitamasEngine";

        private static Dictionary<string, string> dataMap = new Dictionary<string, string>();

        public static void Save()
        {

        }

        public static void DeleteAll()
        {

        }

        public static void DeleteKey(string key)
        {

        }

        public static bool HasKey(string key)
        {
            return false;
        }

        public static void SetString(string key, string value)
        {

        }

        public static string GetString(string key)
        {
            return null;
        }
    }
}
