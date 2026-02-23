using Fitamas.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fitamas.Serialization
{
    public class ResourceManifest : IResourceManifest
    {
        public bool Contains(object key)
        {
            string path = (string)key;
            return File.Exists(Path.Combine(GameEngine.Instance.Content.RootDirectory, Path.ChangeExtension(path, ".xnb")));
        }

        public string GetPath(object key)
        {
            return (string)key;
        }
    }

    public interface IResourceManifest
    {
        bool Contains(object key);
        string GetPath(object key);
    }
}
