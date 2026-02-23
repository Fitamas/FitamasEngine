using Fitamas.Core;
using Fitamas.DebugTools;
using Fitamas.ImGuiNet.Importers;
using Fitamas.ImGuiNet.Serialization;
using Fitamas.Serialization;
using Fitamas.Serialization.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fitamas.ImGuiNet.Assets
{
    public static class AssetSystem
    {
        public const string ManifestFileName = "order.resource";

        private static DebugLogger logger = new DebugLogger("AssetSystem");
        private static ConcurrentDictionary<string, Asset> assetsByPath = new ConcurrentDictionary<string, Asset>(StringComparer.OrdinalIgnoreCase);
        private static HashSet<Asset> updateQueue = new HashSet<Asset>();
        private static EditorResourceManifest manifest;
        private static FileSystemWatcher watcher;
        private static bool hasChanges;
        private static bool needRefresh;
        private static bool isInitialized;

        private static List<IResourceImporter> importers;

        public static IEnumerable<Asset> All => assetsByPath.Values;

        public static void InitializeProject()
        {
            if (isInitialized)
            {
                return;
            }

            string path = Path.Combine(GameEngine.Instance.Content.RootDirectory, ManifestFileName);
            if (File.Exists(path))
            {
                manifest = JsonUtility.Load<EditorResourceManifest>(path);
            }
            else
            {
                manifest = new EditorResourceManifest();
            }
            Resources.Manifest = manifest;

            hasChanges = true;
            isInitialized = true;

            importers = new List<IResourceImporter>()
            {
                new AudionWavImporter(),
                new MonoObjectImporter(),
                new Textture2DImporter(),
                new FontAssetImporter(),
            };

            watcher = new FileSystemWatcher(GameEngine.Instance.Content.RootDirectory)
            {
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite |
                               NotifyFilters.CreationTime | NotifyFilters.Size,
                EnableRaisingEvents = true
            };

            watcher.Created += OnFileChanged;
            watcher.Changed += OnFileChanged;
            watcher.Renamed += OnFileChanged;
            watcher.Deleted += OnFileChanged;

            InternalRefresh();
            Update();
        }

        private static void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            needRefresh = true;
        }

        internal static void InternalRefresh()
        {
            foreach (var filePath in Directory.GetFiles(GameEngine.Instance.Content.RootDirectory, "*.*", SearchOption.AllDirectories))
            {
                string path = Path.GetRelativePath(GameEngine.Instance.Content.RootDirectory, filePath);
                Asset asset = RegisterAsset(path);
                if (asset != null)
                {
                    updateQueue.Add(asset);
                }
            }

            foreach (var resourceInfo in manifest.Resources)
            {
                Asset asset = RegisterAsset(resourceInfo.Path);
                if (asset != null)
                {
                    updateQueue.Add(asset);
                }
            }
        }

        internal static void SaveManifest()
        {
            string path = Path.Combine(GameEngine.Instance.Content.RootDirectory, ManifestFileName);
            JsonUtility.Save(path, manifest);
        }

        internal static void Update()
        {
            if (!hasChanges || !needRefresh || !isInitialized)
            {
                return;
            }

            if (needRefresh)
            {
                InternalRefresh();
                needRefresh = false;
            }

            hasChanges = false;

            foreach (var asset in updateQueue)
            {
                UpdateAsset(asset);
            }

            updateQueue.Clear();

            IEnumerable<Asset> assetsToRemove = assetsByPath.Values.Where(asset => asset.IsDeleted);

            foreach (Asset asset in assetsToRemove)
            {
                RemoveAsset(asset);
            }
        }

        public static Asset RegisterAsset(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (assetsByPath.TryGetValue(path, out Asset _asset))
            {
                return _asset;
            }

            try
            {
                if (manifest.HasResource(path))
                {
                    ResourceInfo resourceInfo = manifest.GetResource(path);
                    Asset asset = CreateAsset(resourceInfo.Guid, resourceInfo.Path, resourceInfo.Type);

                    if (asset != null)
                    {
                        assetsByPath[path] = asset;
                        logger.Log($"Registed asset: {asset.AbsolutePath}");
                    }

                    return asset;
                }

                string extension = Path.GetExtension(path).ToLower();

                foreach (var importer in importers)
                {
                    if (importer.SupportedExtensions.Contains(extension))
                    {
                        return importer.Import(path);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogExeption(ex);
            }

            return null;
        }

        public static Asset CreateAsset(string path, Type type)
        {
            AssetTypeAttribute assetType = type.GetTypeAttribute<AssetTypeAttribute>();
            if (assetType == null)
            {
                logger.LogWarning($"Couldn't find matching asset type for type: {type}");
                return null;
            }

            path = Path.ChangeExtension(path, assetType.Extension);

            Asset asset = CreateAsset(Guid.NewGuid(), path, type);
            AddAsset(asset);

            if (asset.TryLoad(out object value, true) && value is MonoContentObject monoObject)
            {
                asset.Save(monoObject);
            }

            return asset;
        }

        public static Asset CreateAsset(string path, MonoContentObject monoObject)
        {
            Type type = monoObject.GetType();
            AssetTypeAttribute assetType = type.GetTypeAttribute<AssetTypeAttribute>();
            if (assetType == null)
            {
                logger.LogWarning($"Couldn't find matching asset type for type: {type}");
                return null;
            }

            path = Path.ChangeExtension(path, assetType.Extension);

            Asset asset = CreateAsset(monoObject.Guid, path, type);
            AddAsset(asset);

            asset.Save(monoObject);

            return asset;
        }

        private static Asset CreateAsset(Guid guid, string path, string type)
        {
            return CreateAsset(guid, path, MonoJsonSerializer.DefaultSerializationBinder.BindToType(type));
        }

        private static Asset CreateAsset(Guid guid, string path, Type type)
        {
            if (type == null)
            {
                return null;
            }

            string absolutePath = Path.Combine(GameEngine.Instance.Content.RootDirectory, path);
            string name = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path).ToLower();
            Asset asset = new Asset(name, extension, path, absolutePath, guid, type);

            return asset;
        }

        public static Asset ImportAsset(Guid guid, string path, string type)
        {
            return ImportAsset(guid, path, MonoJsonSerializer.DefaultSerializationBinder.BindToType(type));
        }

        public static Asset ImportAsset(Guid guid, string path, Type type)
        {
            if (type == null)
            {
                return null;
            }

            Asset asset = CreateAsset(guid, path, type);

            if (!asset.FileInfo.Exists)
            {
                return null;
            }

            AddAsset(asset);

            return asset;
        }

        public static Asset GetAsset(string path)
        {
            return assetsByPath[path];
        }

        public static bool TryGetAsset(Guid guid, out Asset asset)
        {
            asset = assetsByPath.Values.FirstOrDefault(x => x.Guid == guid);
            return asset != null;
        }

        public static bool TryGetAsset(string path, out Asset asset)
        {
            return assetsByPath.TryGetValue(path, out asset) && !asset.IsDeleted;
        }

        internal static void UpdateAsset(Asset asset)
        {
            asset.UpdateInternals();

            if (asset.IsDeleted)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(asset.Path))
            {
                return;
            }

            assetsByPath[asset.Path] = asset;
        }

        internal static void AddAsset(Asset asset)
        {
            assetsByPath[asset.Path] = asset;
            ResourceInfo info = new ResourceInfo()
            {
                Guid = asset.Guid,
                Path = asset.Path,
            };
            MonoJsonSerializer.DefaultSerializationBinder.BindToName(asset.Type, out info.Type);
            manifest.RemoveResource(info.Path);
            manifest.AddResource(info);

            logger.Log($"Added asset: {asset.AbsolutePath}");

            updateQueue.Add(asset);
            hasChanges = true;
        }

        internal static void RemoveAsset(Asset asset)
        {
            if (string.IsNullOrWhiteSpace(asset.Path))
                return;

            assetsByPath.Remove(asset.Path, out _);
            manifest.RemoveResource(asset.Path);

            logger.Log($"Removed asset: {asset.AbsolutePath}");

            updateQueue.Remove(asset);
            hasChanges = true;
        }
    }
}
