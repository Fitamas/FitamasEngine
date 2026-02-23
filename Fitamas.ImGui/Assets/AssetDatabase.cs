using Fitamas.Core;
using Fitamas.Events;
using Fitamas.ImGuiNet.Serialization;
using Fitamas.Serialization;
using R3;
using System;
using System.IO;
using System.Linq;

namespace Fitamas.ImGuiNet.Assets
{
    public static class AssetDatabase
    {
        private static readonly FitamasContentManager content = (FitamasContentManager)GameEngine.Instance.Content;

        public static MonoAction OnSaveProject;
 
        public static Guid GuidFromAssetPath(string path)
        {
            if (AssetSystem.TryGetAsset(path, out var asset))
            {
                return asset.Guid;
            }

            return Guid.Empty;
        }

        public static string AssetPathToGuid(Guid guid)
        {
            if (AssetSystem.TryGetAsset(guid, out var asset))
            {
                return asset.Path;
            }

            return string.Empty;
        }

        public static bool Contains(string path)
        {
            return AssetSystem.TryGetAsset(path, out _);
        }

        public static bool Contains(Guid guid)
        {
            return AssetSystem.TryGetAsset(guid, out _);
        }

        public static string GetAssetPath(MonoContentObject monoObject)
        {
            if (AssetSystem.TryGetAsset(monoObject.Guid, out Asset asset))
            {
                return asset.Path;
            }

            return string.Empty;
        }

        public static string[] GetAllAssetPaths()
        {
            return AssetSystem.All.Select(r => r.Path).ToArray();
        }

        public static string[] GetAllAssetPaths(string root)
        {
            return AssetSystem.All.Where(info => info.Path.StartsWith(root)).Select(r => r.Path).ToArray();
        }

        public static string[] GetAllFolderPaths(string path = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = content.RootDirectory;
            }

            return Directory.GetDirectories(path, "*.*", SearchOption.AllDirectories)
                            .Select(s => Path.GetRelativePath(content.RootDirectory, s))
                            .ToArray();
        }

        public static Asset CreateAsset(string path, Type type)
        {
            return AssetSystem.CreateAsset(path, type);
        }

        public static Asset CreateAsset(string path, MonoContentObject monoObject)
        {
            return AssetSystem.CreateAsset(path, monoObject);
        }

        public static void SaveProject()
        {
            OnSaveProject?.Invoke();

            AssetSystem.SaveManifest();

            foreach (var asset in content.InternalLoadedAssets)
            {
                if (asset is MonoContentObject monoObject)
                {
                    SaveAsset(monoObject);
                }
            }
        }

        public static bool SaveAsset(MonoContentObject monoObject)
        {
            if (AssetSystem.TryGetAsset(monoObject.Guid, out Asset asset))
            {
                if (!asset.Save(monoObject))
                {
                    Debug.LogError($"Cannot save this object: {monoObject.Name}");
                }

                return true;
            }

            return false;
        }

        public static void Refresh()
        {
            AssetSystem.InternalRefresh();
        }

        public static T LoadAsset<T>(string path)
        {
            return content.Load<T>(path);
        }

        public static object LoadAsset(string path)
        {
            if (AssetSystem.TryGetAsset(path, out Asset asset))
            {
                return content.LoadObject(asset.Path, asset.Type);
            }
 
            return null;
        }

        public static void DeleteAssets(string[] paths)
        {
            foreach (var path in paths)
            {
                DeleteAsset(path);
            }
        }

        public static void DeleteAsset(string path)
        {
            if (AssetSystem.TryGetAsset(path, out Asset asset))
            {
                asset.Delete();
            }
        }

        public static void CreateFolder(string path, string name)
        {
            string folderPath = Path.Combine(content.RootDirectory, path);
            if (File.Exists(folderPath))
            {
                path = Path.GetDirectoryName(path);
            }

            path = Path.Combine(path, name);
            folderPath = Path.Combine(content.RootDirectory, path);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        public static void RenameAsset(string path, string name)
        {
            string newPath = Path.Combine(Path.GetDirectoryName(path), name);
            MoveAsset(path, newPath);
        }

        public static void MoveAsset(string oldPath, string newPath)
        {
            if (AssetSystem.TryGetAsset(oldPath, out Asset asset))
            {
                asset.Move(newPath);
                AssetSystem.UpdateAsset(asset);
            }
        }
    }
}
