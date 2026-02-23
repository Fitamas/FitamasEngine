using Fitamas.Core;
using Fitamas.ImGuiNet.Serialization;
using Fitamas.Serialization;
using Fitamas.Serialization.Json;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Reflection.Metadata;

namespace Fitamas.ImGuiNet.Assets
{
    public class Asset
    {
        public string Name { get; protected set; }
        public string Extension { get; protected set; }
        public string Path { get; protected set; }
        public string AbsolutePath { get; protected set; }
        public Guid Guid { get; protected set; }
        public Type Type { get; protected set; }
        public DateTime LastModified { get; protected set; }
        public bool IsDeleted { get; protected set; }
        public FileInfo FileInfo { get; protected set; }
        public bool IsDirty { get; set; }

        //public abstract List<Asset> GetReferences(bool deep);
        //public abstract List<Asset> GetDependants(bool deep);

        public string MetaData
        {
            get
            {
                return Path + ".meta";
            }
        }

        public Asset(string name, string extension, string path, string absolutePath, Guid guid, Type type)
        {
            Name = name;
            Extension = extension;
            Path = path;
            AbsolutePath = absolutePath;
            Guid = guid;
            Type = type;

            FileInfo = new FileInfo(absolutePath);
            IsDeleted = !FileInfo.Exists;
            LastModified = FileInfo.LastWriteTime;
        }

        public bool TryLoad<T>(out T value, bool allowCreate = false) where T : class
        {
            if (TryLoad(out var obj, allowCreate))
            {
                value = (T)obj;
                return true;
            }

            value = null;
            return false;
        }

        public bool TryLoad(out object value, bool allowCreate = false)
        {
            value = null;

            FitamasContentManager content = GameEngine.Instance.Content as FitamasContentManager;
            value = content.LoadObject(Path, Type);

            if (value != null)
            {
                return true;
            }

            if (allowCreate)
            {
                value = Activator.CreateInstance(Type, true);
                content.Register(Path, value);

                if (value is GraphicsResource graphicsResource)
                {
                    graphicsResource.Name = Path;
                }
                else if (value is MonoContentObject monoObject)
                {
                    monoObject.Guid = Guid;
                    monoObject.Name = Path;
                }

                return true;
            }

            return false;
        }

        public bool Save(MonoContentObject monoObject)
        {
            if (monoObject == null || monoObject.GetType() != Type || monoObject.Guid != Guid)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(AbsolutePath))
            {
                return false;
            }

            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(AbsolutePath));

            JsonUtility.Save(AbsolutePath, monoObject);

            FitamasContentManager content = GameEngine.Instance.Content as FitamasContentManager;
            content.Register(Path, monoObject);

            return true;
        }

        public void Move(string path)
        {
            string oldAssetPath = AbsolutePath;
            string newAssetPath = System.IO.Path.Combine(GameEngine.Instance.Content.RootDirectory, path);

            if (File.Exists(oldAssetPath))
            {
                File.Move(oldAssetPath, newAssetPath);
            }

            //TODO move meta data

            AbsolutePath = System.IO.Path.Combine(GameEngine.Instance.Content.RootDirectory, path);
            Name = System.IO.Path.GetFileNameWithoutExtension(path);
            Extension = System.IO.Path.GetExtension(path).ToLower();
            FileInfo = new FileInfo(AbsolutePath);
        }

        internal void UpdateInternals()
        {
            if (IsDeleted)
            {
                return;
            }

            FileInfo.Refresh();
            IsDeleted = !FileInfo.Exists;
            LastModified = FileInfo.LastWriteTime;
        }

        public void Delete()
        {
            if (!IsDeleted)
            {
                IsDeleted = true;
                FileInfo.Delete();

                //TODO delete meta data
            }
        }
    }
}
