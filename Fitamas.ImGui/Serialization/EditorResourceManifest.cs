using Fitamas.Serialization;
using Fitamas.UserInterface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace Fitamas.ImGuiNet.Serialization
{
    public class EditorResourceManifest : IResourceManifest
    {
        [SerializeField] private HashSet<ResourceInfo> resources = new HashSet<ResourceInfo>();

        public IEnumerable<ResourceInfo> Resources => resources;

        public bool Contains(object key)
        {
            
            return HasResource((Guid)key);
        }

        public bool HasResource(Guid guid)
        {
            return resources.Any(e => e.Guid == guid);
        }

        public bool HasResource(string path)
        {
            return resources.Any(e => e.Path == path);
        }

        public ResourceInfo GetResource(Guid guid)
        {
            return resources.FirstOrDefault(e => e.Guid == guid);
        }

        public ResourceInfo GetResource(string path)
        {
            return resources.FirstOrDefault(e => e.Path == path);
        }

        public bool AddResource(ResourceInfo resource)
        {
            if (resource.Guid == Guid.Empty)
            {
                throw new ArgumentNullException();
            }

            return resources.Add(resource);
        }

        public void RemoveResource(string path)
        {
            resources.RemoveWhere(e => e.Path == path);
        }

        public string GetPath(object key)
        {
            Guid guid = (Guid)key;

            if (guid == Guid.Empty)
            {
                return null;
            }

            if (HasResource(guid))
            {
                ResourceInfo info = GetResource(guid);
                return info.Path;
            }

            return null;
        }
    }

    public struct ResourceInfo : IEquatable<ResourceInfo>
    {
        public Guid Guid = Guid.Empty;
        public string Path = "";
        public string Type = "";

        public ResourceInfo()
        {

        }

        private static bool CompareBaseObject(ResourceInfo a, ResourceInfo b)
        {
            bool flag1 = (object)a == null;
            bool flag2 = (object)b == null;

            if (flag1 && flag2)
            {
                return true;
            }
            else if (flag1 && !flag2)
            {
                return false;
            }
            else if (!flag1 && flag2)
            {
                return false;
            }

            return string.Equals(a.Path, b.Path, StringComparison.OrdinalIgnoreCase);
        }

        public static bool operator ==(ResourceInfo a, ResourceInfo b)
        {
            return CompareBaseObject(a, b);
        }

        public static bool operator !=(ResourceInfo a, ResourceInfo b)
        {
            return !CompareBaseObject(a, b);
        }

        public override bool Equals(object other)
        {
            if (other != null && other is ResourceInfo obj)
            {
                return CompareBaseObject(this, obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Path);
        }

        public bool Equals(ResourceInfo other)
        {
            return CompareBaseObject(this, other);
        }
    }
}
