using Fitamas;
using Fitamas.Events;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace WDL.Gameplay.DigitalLogic
{
    public class LogicComponentManager
    {
        public const string RootDirectory = "Saves";
        public const string FileExtension = ".lc";

        private List<LogicComponentDescription> components;

        public MonoAction<LogicComponentDescription> OnAddComponent;
        public MonoAction<LogicComponentDescription> OnRemoveComponent;

        public LogicComponentDescription[] Components => components.ToArray();

        public LogicComponentManager()
        {
            components = new List<LogicComponentDescription>();
            CreateDefoultNode();
        }

        public void AddComponent(LogicComponentDescription component)
        {
            if (!components.Contains(component))
            {
                OnAddComponent?.Invoke(component);
                components.Add(component);
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

            //return Load(path); TODO convert full name to path
        }

        private void CreateDefoultNode()
        {
            var and = new LogicComponentDescription(typeof(LogicAND))
            {
                Name = "And",
                Namespace = "Defoult",
                PinInputName = { "InA", "InB" },
                PinOutputName = { "Out" }
            };
            AddComponent(and);
            var not = new LogicComponentDescription(typeof(LogicNOT))
            {
                Name = "Not",
                Namespace = "Defoult",
                PinInputName = { "In" },
                PinOutputName = { "Out" }
            };
            AddComponent(not);
            var input = new LogicComponentDescription(typeof(LogicINPUT))
            {
                Name = "Input",
                Namespace = "Defoult",
                PinOutputName = { "Out" }
            };
            AddComponent(input);
            var output = new LogicComponentDescription(typeof(LogicOUTPUT))
            {
                Name = "Output",
                Namespace = "Defoult",
                PinInputName = { "In" }
            };
            AddComponent(output);
            //var nand = new LogicComponentDescription(typeof(LogicCircuit))
            //{
            //    Name = "Nand",
            //    PinInputName = { "InA", "InB" },
            //    PinOutputName = { "Out" },
            //    Components = [output, input, input, and, not],
            //    Connections = [
            //            new Connection() { OutputComponentId = 1, InputComponentId = 3, InputIndex = 0 },
            //            new Connection() { OutputComponentId = 2, InputComponentId = 3, InputIndex = 1 },
            //            new Connection() { OutputComponentId = 3, InputComponentId = 4 },
            //            new Connection() { OutputComponentId = 4, InputComponentId = 0 }]
            //};
            //AddComponent(nand);
        }

        public void Save(LogicComponentDescription description)
        {
            string contentPath = Path.Combine(RootDirectory, description.FullName) + FileExtension;

            Directory.CreateDirectory(Path.GetDirectoryName(contentPath));

            var serializer = new DigitalLogicJsonSerializer(this);
            using (var sw = new StreamWriter(contentPath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, description);
            }
        }

        public void SaveAll()
        {
            foreach (var item in components)
            {
                if (!item.IsBase)
                {
                    Save(item);
                }                
            }
        }

        public LogicComponentDescription Load(string path)
        {
            using (var stream = TitleContainer.OpenStream(path))
            using (var reader = new StreamReader(stream))
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

                if (description != null)
                {
                    string localPath = Path.GetRelativePath(RootDirectory, path);

                    int lastSeparatorIndex = localPath.LastIndexOf('\\');
                    if (lastSeparatorIndex != -1)
                    {
                        description.Namespace = localPath.Substring(0, lastSeparatorIndex);
                    }

                    Debug.Log("Load component description: " + description.FullName);
                }
                return description;
            }
        }

        public void LoadAll()
        {
            if (!Directory.Exists(RootDirectory))
            {
                Directory.CreateDirectory(RootDirectory);
                return;
            }

            string filter = "*" + FileExtension;
            string[] files = Directory.GetFiles(RootDirectory, filter, SearchOption.AllDirectories);

            foreach (string file in files)
            {
                LogicComponentDescription description = Load(file);
                AddComponent(description);
            }
        }

        public void Delete(string path)
        {
            LogicComponentDescription description = GetComponent(path);
            Delete(description);
        }

        public void Delete(LogicComponentDescription description)
        {
            if (ContainComponent(description))
            {
                string path = Path.Combine(RootDirectory, description.FullName) + FileExtension;
                File.Delete(path);

                components.Remove(description);

                foreach (var component in components)
                {
                    component.Remove(description);
                }
                OnRemoveComponent?.Invoke(description);
            }
        }

        public void Import(string path)
        {
            string target;

            if (File.Exists(path) || Directory.Exists(path))
            {
                target = Path.Combine(RootDirectory, Path.GetFileName(path));
                Directory.CreateDirectory(target);
            }
            else
            {
                Debug.LogError("File or directory not exists");
                return;
            }

            Debug.Log("Copy files from: " + path + " to: " + target);
            CopyFilesRecursively(path, target);

            Debug.Log("Import");
            LoadAll();
        }

        private static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }
    }
}
