using Fitamas.Serializeble;
using System;

namespace Fitamas.UserInterface.Serializeble
{
    public static class GUIUtility
    {
        public static SerializebleLayout Load(string path)
        {
            try
            {
                GUISerializer serializer = new GUISerializer();
                SerializebleLayout layout = serializer.Load(path);
                return layout;
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            return null;
        }
    }
}
