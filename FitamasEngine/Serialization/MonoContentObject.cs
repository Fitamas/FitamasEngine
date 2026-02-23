using Fitamas.Serialization.Json;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Fitamas.Serialization
{
    public abstract class MonoContentObject : MonoObject
    {
        public string Name { get; set; }

        public MonoContentObject()
        {

        }

        public virtual void LoadData(string path)
        {
            JsonUtility.LoadToObject(path, this);
        }

        public virtual void OnPostLoad()
        {

        }
    }
}
