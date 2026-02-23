using Fitamas.Core;
using System;

namespace Fitamas.DebugTools
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AssetTypeAttribute : Attribute, ITypeAttribute
    {
        public Type TargetType { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Extension { get; set; }

        public AssetTypeAttribute(string fileName = "NewFile", string title = "CreateAsset", string extension = ".json")
        {
            FileName = fileName;
            Title = title;
            Extension = extension;
        }
    }
}
