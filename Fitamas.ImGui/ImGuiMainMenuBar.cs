using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Fitamas.ImGuiNet
{
    public static class ImGuiMainMenuBar
    {
        public static void MainMenuBar(ImGuiManager manager)
        {
            ImGui.BeginMainMenuBar();
            if (ImGui.BeginMenu("File"))
            {
                if (ImGui.MenuItem("Create new scene"))
                {
                    //SceneSelectorWindow.CreateScene();
                }
                if (ImGui.MenuItem("Open scene", "Ctrl+O"))
                {
                    //SceneSelectorWindow.OpenScene();
                }
                if (ImGui.MenuItem("Save project", "Ctrl+S"))
                {
                    Debug.Log("TODO save"); //TODO

                    //if (!AssetDatabase.Contains(openScene))
                    //{
                    //    AssetDatabase.CreateAsset(openScene, "NewScene.scene");
                    //}

                    //AssetDatabase.SaveAssets();
                }
                ImGui.EndMenu();
            }
            if (ImGui.BeginMenu("Edit"))
            {
                if (ImGui.MenuItem("Crete new entity"))
                {
                    //GameObject gameObject = new GameObject();
                    //SelectObject = gameObject;
                    //openScene.GameObjects.Add(gameObject);

                    //SelectObject = gameWorld.CreateEntity();
                    //SceneWindow.scene.Create();
                    //EditorSystem.SelectObject = SceneWindow.scene.Create();
                }
                ImGui.EndMenu();
            }
            if (ImGui.BeginMenu("Window"))
            {
                //foreach (var item in windows)
                //{
                //    if (ImGui.MenuItem(item.Name))
                //    {
                //        item.Switch();
                //    }
                //}

                ImGui.EndMenu();
            }
            if (ImGui.BeginMenu("Debug"))
            {
                if (ImGui.MenuItem("PrintObject"))
                {
                    //foreach (var item in ObjectManager.LoadObjects)
                    //{
                    //    Debug.Log(item.Key + " : " + item.Value);
                    //}
                }
                ImGui.EndMenu();
            }

            ImGui.EndMainMenuBar();

            HandleGlobalShortcuts();
        }

        private static void HandleGlobalShortcuts()
        {
            if (ImGuiUtils.IsKeyPressedWithModifier(ImGuiKey.ModCtrl, ImGuiKey.S))
            {
                Debug.Log("TODO save"); //TODO
            }
        }
    }
}
