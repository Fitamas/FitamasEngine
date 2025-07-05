using Fitamas;
using Fitamas.Core;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ObservableCollections;
using System;
using System.Collections.Generic;
using System.IO;
using WDL.DigitalLogic.Serialization;

namespace WDL.DigitalLogic
{
    public class LogicComponentManager
    {
        public static readonly string RootDirectory = Path.Combine(Application.DataPath, "Saves");
        public const string FileExtension = "lc";

        private ObservableList<LogicComponentDescription> components;

        public IObservableCollection<LogicComponentDescription> Components => components;

        public LogicComponentManager()
        {
            components = new ObservableList<LogicComponentDescription>();

            AddComponent(LogicComponents.And);
            AddComponent(LogicComponents.Not);
            AddComponent(LogicComponents.Input);
            AddComponent(LogicComponents.Output);
        }

        public void AddComponent(LogicComponentDescription component)
        {
            if (!components.Contains(component))
            {
                components.Add(component);
            }
        }

        public void Remove(string path)
        {
            LogicComponentDescription description = GetComponent(path);
            Remove(description);
        }

        public void Remove(LogicComponentDescription description)
        {
            if (description.IsBase)
            {
                return;
            }

            if (ContainComponent(description))
            {
                components.Remove(description);

                foreach (var component in components)
                {
                    component.RemoveComponents(description);
                }
            }
        }

        public bool ContainComponent(string fullname)
        {
            foreach (LogicComponentDescription component in components)
            {
                if (component.FullName == fullname)
                {
                    return true;
                }
            }

            return false;
        }

        public bool ContainComponent(LogicComponentDescription component)
        {
            return components.Contains(component);
        }

        public LogicComponentDescription GetComponent(string fullName)
        {
            foreach (var component in components)
            {
                if (component.FullName == fullName)
                {
                    return component;
                }
            }

            return null;
        }

        public void SaveComponents()
        {
            Directory.Delete(RootDirectory, true);

            foreach (var item in components)
            {
                Save(item);
            }
        }

        private void Save(LogicComponentDescription description)
        {
            if (description.IsBase)
            {
                return;
            }

            string contentPath = Path.ChangeExtension(Path.Combine(RootDirectory, description.FullName), FileExtension);

            Directory.CreateDirectory(Path.GetDirectoryName(contentPath));

            var serializer = new DigitalLogicJsonSerializer(this);
            using (var sw = new StreamWriter(contentPath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, description);
            }
        }

        public void LoadAll()
        {
            if (!Directory.Exists(RootDirectory))
            {
                Directory.CreateDirectory(RootDirectory);
                return;
            }

            string filter = "*." + FileExtension;
            string[] files = Directory.GetFiles(RootDirectory, filter, SearchOption.AllDirectories);

            foreach (string file in files)
            {
                LogicComponentDescription description = Load(file);
                AddComponent(description);
            }
        }

        private LogicComponentDescription Load(string path)
        {
            using (var reader = new StreamReader(path))
            using (var jsonReader = new JsonTextReader(reader))
            {
                LogicComponentDescription description = null;

                try
                {
                    var serializer = new DigitalLogicJsonSerializer(this);
                    description = serializer.Deserialize<LogicComponentDescription>(jsonReader);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.Message);
                }

                Debug.Log("Load component description: " + description.FullName);

                return description;
            }
        }

        public void Import(IEnumerable<string> paths)
        {
            foreach (string path in paths)
            {
                string resultPath = Path.Combine(RootDirectory, Path.GetFileName(path));
                if (File.Exists(path) && !File.Exists(resultPath))
                {
                    File.Copy(path, resultPath, true);
                }
            }

            LoadAll();
        }

        //private static void CopyFilesRecursively(string sourcePath, string targetPath)
        //{
        //    foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
        //    {
        //        Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
        //    }

        //    foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
        //    {
        //        File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
        //    }
        //}
    }
}
