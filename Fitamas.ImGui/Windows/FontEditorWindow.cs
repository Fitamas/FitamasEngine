using Fitamas.Core;
using Fitamas.Fonts;
using Fitamas.ImGuiNet.Assets;
using Fitamas.ImGuiNet.Editors;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Fitamas.ImGuiNet.Windows
{
    [CustomEditor(typeof(FontAsset))]
    public class FontEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            FontAsset font = target as FontAsset;

            ImGui.Text($"Face count: {font.FaceCount}");
            ImGui.Text($"Face index: {font.FaceIndex}");
            ImGui.Text($"Family name: {font.FamilyName}");
            ImGui.Text($"Style name: {font.StyleName}");

            if (ImGui.Button("Open editor"))
            {
                EditorWindow.OpenWindow(new FontEditorWindow((FontAsset)target));
            }
        }
    }

    public class FontEditorWindow : EditorWindow
    {
        private FontAsset font;

        private string name = "New_font";
        private int size = 16;
        private int texDims = 256;
        private char baseChar = '?';

        public FontEditorWindow(FontAsset font)
        {
            this.font = font;

            Flags |= ImGuiWindowFlags.NoSavedSettings;
        }

        protected override void OnGUI(GameTime gameTime)
        {
            ImGui.InputText("Name", ref name, 1000);
            ImGui.DragInt("Size", ref size);
            ImGui.DragInt("TexDims", ref texDims);

            if (ImGui.Button("Create sprite font"))
            {
                GraphicsDevice graphicsDevice = GameEngine.Instance.GraphicsDevice;
                Texture2D texture = new Texture2D(graphicsDevice, texDims, texDims);
                FontAtlas atlas = font.CreateAtlas(texture, size, 0, 0, baseChar);
                string path = Path.Combine(Path.GetDirectoryName(font.Name), name);

                string texturePath = Path.Combine(GameEngine.Instance.Content.RootDirectory, path) + ".png";
                Stream stream = new FileStream(texturePath, FileMode.Create);
                using (stream)
                {
                    texture.SaveAsPng(stream, texture.Width, texture.Height);
                }

                AssetDatabase.CreateAsset(path, atlas);
            }
        }
    }
}
