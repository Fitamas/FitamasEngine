using System;

namespace Fitamas.Serializeble
{
    public abstract class SerializebleObject : MonoObject
    {
        public string Name;

        public SerializebleObject(string name = "new object")
        {
            this.Name = name;


            //TODO
        }
    }
}
