using Fitamas.ImGuiNet.Assets;
using Fitamas.ImGuiNet.Windows;
using ImGuiNET;
using System;

namespace Fitamas.ImGuiNet
{
    internal class ImGuiMainMenuItems
    {
        [MenuItem("File/Save", ImGuiKey.ModCtrl, ImGuiKey.S)]
        internal static void SaveProject()
        {
            Debug.Log("TODO SAVE");

            AssetDatabase.SaveProject();
        }

        [MenuItem("File/Open Scene")]
        internal static void OpenScene()
        {

        }

        [MenuItem("Window/Console")]
        internal static void OpenConsole()
        {
            EditorWindow.OpenWindow(new ConsoleWindow());
        }

        [MenuItem("Window/Game world")]
        internal static void OpenGameWorld()
        {
            EditorWindow.OpenWindow(new GameWorldWindow());
        }

        [MenuItem("Window/Scene editor")]
        internal static void OpenSceneEditor()
        {
            EditorWindow.OpenWindow(new SceneEditorWindow());
        }

        [MenuItem("Window/Hierarchy")]
        internal static void OpeHierarchy()
        {
            EditorWindow.OpenWindow(new HierarchyWindow());
        }

        [MenuItem("Window/Inspector")]
        internal static void OpenInspector()
        {
            EditorWindow.OpenWindow(new InspectorWindow());
        }

        [MenuItem("Window/GUI editor")]
        internal static void OpenGUIEditor()
        {
            EditorWindow.OpenWindow(new GUIEditorWindow());
        }

        [MenuItem("Window/Resources")]
        internal static void OpenResources()
        {
            EditorWindow.OpenWindow(new ResourcesWindow());
        }
    }
}
