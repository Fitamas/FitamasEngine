using System;

namespace Fitamas.DebugTools
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AssetMenuAttribute : Attribute
    {
        public string FileName { get; set; }
        public string Title { get; set; }

        public AssetMenuAttribute(string fileName = "NewFile", string title = "CreateAsset") 
        { 
            FileName = fileName;
            Title = title;
        }
    }
}
