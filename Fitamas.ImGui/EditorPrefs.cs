using Fitamas.Core;
using System;
using System.IO;

namespace Fitamas.ImGuiNet
{
    public static class EditorPrefs
    {
        public static readonly string RootDirectory = Path.Combine(Application.DataPath, "EditorData");

        static EditorPrefs()
        {
            Directory.CreateDirectory(RootDirectory);


        }
    }
}
