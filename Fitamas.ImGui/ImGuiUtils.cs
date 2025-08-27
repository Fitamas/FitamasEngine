using Fitamas.Core;
using Fitamas.Math;
using Fitamas.Serialization;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Fitamas.ImGuiNet
{
    public static class ImGuiUtils
    {
        public static bool PropertyField(SerializedProperty property)
        {
            object value = property.Value;
            string name = property.Name;

            if (property.IsInt)
            {
                int intValue = (int)value;
                if (ImGui.DragInt(name, ref intValue))
                {
                    property.SetValue(intValue);
                    return true;
                }
            }
            else if (property.IsFloat)
            {
                float floatValue = (float)value;
                if (ImGui.DragFloat(name, ref floatValue))
                {
                    property.SetValue(floatValue);
                    return true;
                }
            }
            else if (property.IsBool)
            {
                bool boolValue = (bool)value;
                if (ImGui.Checkbox(name, ref boolValue))
                {
                    property.SetValue(boolValue);
                    return true;
                }
            }
            else if (property.IsVector2)
            {
                Vector2 vectorValue = (Vector2)value;
                if (InputVector2(name, ref vectorValue))
                {
                    property.SetValue(vectorValue);
                    return true;
                }
            }
            else if (property.IsVector3)
            {
                Vector3 vectorValue = (Vector3)value;
                if (InputVector3(name, ref vectorValue))
                {
                    property.SetValue(vectorValue);
                    return true;
                }
            }
            else if (property.IsColor)
            {
                Color color = (Color)value;
                if (InputColor(name, ref color))
                {
                    property.SetValue(color);
                    return true;
                }
            }
            else if (property.IsRectangleF)
            {
                RectangleF rectangle = (RectangleF)value;
                if (InputRectangle(property.Name, ref rectangle))
                {
                    property.SetValue(rectangle);
                    return true;
                }
            }
            else if (property.IsString)
            {
                string stringValue = (string)value;
                if (ImGui.InputText(name, ref stringValue, 1000))
                {
                    property.SetValue(stringValue);
                    return true;
                }
            }
            else if (property.IsEnum)
            {
                int intValue = Convert.ToInt32(value);
                if (PropertyEnum(name, property.Type, 0, ref intValue))
                {
                    property.SetValue(intValue);
                    return true;
                }
            }
            else if (property.IsArray)
            {
                if (PropertyArray(property))
                {
                    return true;
                }
            }
            else if (property.IsMonoObject)
            {
                MonoObject serializeble = (MonoObject)value;
                if (PropertyMonoObject(name, property.Type, ref serializeble))
                {
                    property.SetValue(serializeble);
                    return true;
                }
            }
            else if (property.IsTexture2D)
            {
                Texture2D texture = (Texture2D)value;
                string path = texture != null && !string.IsNullOrEmpty(texture.Name) ? texture.Name : "";

                if (ImGui.InputText(name, ref path, 1000, ImGuiInputTextFlags.EnterReturnsTrue))
                {
                    if (string.IsNullOrEmpty(path))
                    {
                        property.SetValue(null);
                        return true;
                    }
                    else
                    {
                        try
                        {
                            texture = GameEngine.Instance.Content.Load<Texture2D>(path);
                            property.SetValue(texture);
                            return true;
                        }
                        catch (Exception e)
                        {
                            Debug.Log(e);
                        }
                    }
                }
                if (ImGui.BeginDragDropTarget())
                {
                    if (ImGui.IsMouseReleased(ImGuiMouseButton.Left))
                    {
                        if (ImGuiManager.DragDropObject is string result)
                        {
                            try
                            {
                                path = Path.GetRelativePath(GameEngine.Instance.Content.RootDirectory, result);
                                texture = GameEngine.Instance.Content.Load<Texture2D>(path);
                                property.SetValue(texture);
                                return true;
                            }
                            catch (Exception e)
                            {
                                Debug.Log(e);
                            }
                        }
                    }

                    ImGui.EndDragDropTarget();
                }
            }
            else
            {
                if (ImGui.TreeNode(name))
                {
                    if (property != null && property.ChildProperty != null)
                    {
                        foreach (var prop in property.ChildProperty)
                        {
                            PropertyField(prop);
                        }
                    }
                    ImGui.TreePop();
                }
            }

            return false;
        }

        public static bool InputVector2(string name, ref Vector2 value)
        {
            System.Numerics.Vector2 vectorValue = value.ToNumerics();
            if (ImGui.DragFloat2(name, ref vectorValue))
            {
                value = new Vector2(vectorValue.X, vectorValue.Y);
                return true;
            }
            return false;
        }

        public static bool InputVector3(string name, ref Vector3 value)
        {
            System.Numerics.Vector3 vectorValue = value.ToNumerics();
            if (ImGui.DragFloat3(name, ref vectorValue))
            {
                value = new Vector3(vectorValue.X, vectorValue.Y, vectorValue.Z);
                return true;
            }
            return false;
        }

        public static bool InputVector4(string name, ref Vector4 value)
        {
            System.Numerics.Vector4 vectorValue = value.ToNumerics();
            if (ImGui.DragFloat4(name, ref vectorValue))
            {
                value = new Vector4(vectorValue.X, vectorValue.Y, vectorValue.Z, vectorValue.W);
                return true;
            }
            return false;
        }

        public static bool InputColor(string name, ref Color value)
        {
            System.Numerics.Vector4 vectorValue = value.ToVector4().ToNumerics();
            if (ImGui.ColorEdit4(name, ref vectorValue))
            {
                value = new Color(vectorValue);
                return true;
            }
            return false;
        }

        public static bool InputRectangle(string name, ref RectangleF rectangle)
        {
            bool result = false;
            if (ImGui.TreeNode(name))
            {
                Vector2 position = rectangle.Position;
                if (InputVector2("RectanglePosition", ref position))
                {
                    rectangle.Position = position;
                    result = true;
                }
                Vector2 size = new Vector2(rectangle.Width, rectangle.Height);
                if (InputVector2("RectangleSize", ref size))
                {
                    rectangle.Width = size.X;
                    rectangle.Height = size.Y;
                    result = true;
                }
                ImGui.TreePop();
            }

            return result;
        }

        public static bool PropertyArray(SerializedProperty property)
        {
            bool isEdit = false;
            if (ImGui.TreeNode(property.Name))
            {
                for (int i = 0; i < property.ArraySize; i++)
                {
                    SerializedProperty serializedProperty = property.GetArrayElementAtIndex(i);

                    ImGui.PushID(i);

                    if (PropertyField(serializedProperty))
                    {
                        property.SetArrayElementIndex(i, serializedProperty.Value);
                        isEdit = true;
                    }

                    ImGui.PopID();
                }


                if (ImGui.Button("+"))
                {
                    property.AddNewElement();
                    isEdit = true;
                }
                ImGui.SameLine();
                if (ImGui.Button("-"))
                {
                    property.DeleteArrayElementAtIndex(property.ArraySize);
                    isEdit = true;
                }

                ImGui.TreePop();
            }

            return isEdit;
        }

        public static bool PropertyReference<T>(string name, ref T reference) where T : class
        {
            object result = reference;
            bool isEdit = PropertyReference(name, ref result, typeof(T));
            if (isEdit)
            {
                reference = (T)result;
            }
            return isEdit;
        }

        public static bool PropertyReference(string name, ref object reference, Type type)
        {
            PropertyReference(type.Name, reference == null);

            bool isEdit = false;
            if (ImGui.BeginDragDropTarget())
            {
                if (ImGui.IsMouseReleased(ImGuiMouseButton.Left))
                {
                    //if (ImGuiManager.DragDropObject is GameObject prefab)
                    //{
                    //    //if (prefab.TryGet(out object result, type))
                    //    //{
                    //    //    reference = result;
                    //    //    isEdit = true;
                    //    //}
                    //}
                }

                ImGui.EndDragDropTarget();
            }
            ImGui.SameLine();
            ImGui.Text(name);

            return isEdit;
        }

        public static void PropertyReference(string text, bool isEmpty = false)
        {
            if (ImGui.BeginChild("region" + text, new System.Numerics.Vector2(250, 40),
                ImGuiChildFlags.Borders, ImGuiWindowFlags.NoScrollbar))
            {
                if (!isEmpty)
                {
                    ImGui.Text(text);
                }
                else
                {
                    ImGui.TextDisabled("empty field");
                }
            }
            ImGui.EndChild();
        }

        public static bool PropertyMonoObject(string name, Type type, ref MonoObject monoObject)
        {
            if (monoObject != null)
            {
                PropertyReference(monoObject.GetType().Name, false);
            }
            else
            {
                PropertyReference(" ", true);
            }

            bool isEdit = false;
            if (ImGui.BeginDragDropTarget())
            {
                if (ImGui.IsMouseReleased(ImGuiMouseButton.Left))
                {
                    if (ImGuiManager.DragDropObject is MonoObject obj)
                    {
                        if (type == obj.GetType())
                        {
                            isEdit = true;
                            monoObject = obj;
                        }
                    }
                }

                ImGui.EndDragDropTarget();
            }
            ImGui.SameLine();
            ImGui.Text(name);

            return isEdit;
        }

        //public static bool PropertyPath(string name, ref string path, ref ImFileDialogInfo info, ref bool openFileDialog)
        //{
        //    ImGui.PushID(name);

        //    if (ImGui.Button("Browse"))
        //    {
        //        string seleceFile = EditorSystem.ObjectManager.RootDirectory;

        //        //if (!string.IsNullOrEmpty(path))
        //        //{
        //        //    seleceFile = Directory.GetParent(path).FullName;
        //        //}
        //        //else
        //        //{
        //        //    seleceFile = SceneEditor.content.RootDirectory;
        //        //}

        //        info = new ImFileDialogInfo()
        //        {
        //            directoryPath = new DirectoryInfo(seleceFile),
        //            fileName = "",
        //            refreshInfo = true
        //        };
        //        openFileDialog = true;
        //    }

        //    bool isEdit = false;

        //    ImGui.SameLine();
        //    if (ImGui.InputText(name, ref path, 1000, ImGuiInputTextFlags.EnterReturnsTrue))
        //    {
        //        isEdit = true;
        //    }

        //    if (openFileDialog)
        //    {
        //        if (ImGui.Begin(name, ref openFileDialog))
        //        {
        //            if (ImGuiFileDialog.FileDialog(ref openFileDialog, info))
        //            {
        //                path = info.resultPath;
        //                isEdit = true;
        //            }
        //        }
        //    }

        //    return isEdit;
        //}

        //private static ImFileDialogInfo info;
        //private static bool openFileDialog;

        //private static bool FileDialog(string name, ref string path)
        //{
        //    return PropertyPath(name, ref path, ref info, ref openFileDialog);
        //}

        public static bool PropertyEnum(string name, Type enumType, int idOffset, ref int selection)
        {
            bool change = false;
            int idx = idOffset;
            if (ImGui.BeginCombo(name, Enum.GetName(enumType, selection)))
            {
                var enumValues = Enum.GetValues(enumType);
                foreach (var i in enumValues)
                {
                    bool selected = i.Equals(selection);
                    ImGui.PushID(idx);
                    {
                        if (ImGui.Selectable(Enum.GetName(enumType, i), selected))
                        {
                            selection = (int)i;
                            change = true;
                        }

                        if (selected)
                        {
                            ImGui.SetItemDefaultFocus();
                        }
                    }
                    ImGui.PopID();
                    idx++;
                }
                ImGui.EndCombo();
            }
            return change;
        }

        public static void Image(nint textureId, Vector2 imageSize, Vector2 uv0, Vector2 uv1, Color color, Color borderColor)
        {
            ImGui.Image(textureId, imageSize.ToNumerics(),
                        uv0.ToNumerics(), uv1.ToNumerics(),
                        color.ToVector4().ToNumerics(), borderColor.ToVector4().ToNumerics());
        }

        public const float toolbarSize = 60;
        public const float toolbarButtonSize = 35;

        public static void DockSpace()
        {
            ImGuiViewportPtr viewport = ImGui.GetMainViewport();
            ImGui.SetNextWindowPos(viewport.Pos + new System.Numerics.Vector2(0, toolbarSize));
            ImGui.SetNextWindowSize(viewport.Size - new System.Numerics.Vector2(0, toolbarSize));
            ImGui.SetNextWindowViewport(viewport.ID);
            ImGuiWindowFlags window_flags = 0
                | ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.NoDocking
                | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse
                | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove
                | ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus;

            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new System.Numerics.Vector2(0.0f, 0.0f));
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0.0f);
            ImGui.Begin("MyDockspace", window_flags);
            uint dockMain = ImGui.GetID("MyDockspace");

            ImGui.DockSpace(dockMain);
            ImGui.End();
            ImGui.PopStyleVar(3);
        }

        public static void BeginToolbar()
        {
            ImGuiViewportPtr viewport = ImGui.GetMainViewport();

            float menuBarHeight = viewport.Size.Y - viewport.WorkSize.Y;

            ImGui.SetNextWindowPos(new System.Numerics.Vector2(viewport.Pos.X, viewport.Pos.Y + menuBarHeight));
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(viewport.Size.X, toolbarSize));
            ImGui.SetNextWindowViewport(viewport.ID);
            ImGuiWindowFlags window_flags = 0
                | ImGuiWindowFlags.NoDocking
                | ImGuiWindowFlags.NoTitleBar
                | ImGuiWindowFlags.NoResize
                | ImGuiWindowFlags.NoMove
                | ImGuiWindowFlags.NoScrollbar
                | ImGuiWindowFlags.NoSavedSettings
                ;
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0);
            ImGui.Begin("TOOLBAR", window_flags);
            ImGui.PopStyleVar();
        }

        public static void EndToolbar()
        {
            ImGui.End();
        }

        public static bool ToolbarButton(nint textureId, Vector2? size = default)
        {
            ImGui.SameLine();
            ImGui.PushID("ToolbarButton");
            System.Numerics.Vector2 imageSize = size.HasValue ? size.Value.ToNumerics()
                                                              : new System.Numerics.Vector2(toolbarButtonSize);
            return ImGui.ImageButton("ToolbarButton", textureId, imageSize);
        }

        public static void AlignForWidth(float width, float alignment = 0.5f)
        {
            float windowWidth = ImGui.GetWindowSize().X;
            ImGui.SameLine((windowWidth - width) * alignment);
            //ImGui.SetCursorPosX((windowWidth - width) * alignment);


            //float avail = ImGui.GetContentRegionAvail().X;
            //float off = (avail - width) * alignment;
            //if (off > 0.0f)
            //    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + off);
        }
    }
}
