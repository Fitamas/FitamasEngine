using Fitamas.Core;
using Fitamas.ECS;
using Fitamas.ImGuiNet.Editors;
using Fitamas.Serialization;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fitamas.ImGuiNet.Windows
{
    public class InspectorWindow : EditorWindow
    {
        private static readonly Type[] componentTypes = ReflectionUtils.GetTypesAssignableFrom<Component>();
        private static readonly Type[] editorType = ReflectionUtils.GetTypesWithAttribute<CustomEditorAttribute>();

        private object selectObject;
        private List<Editor> editors;

        public InspectorWindow() : base("Inspector")
        {

        }

        private void UpdateComponents()
        {
            editors = new List<Editor>();

            if (selectObject is MonoObject || selectObject is GraphicsResource)
            {
                Editor res = CreateEditorComponent(selectObject);
                res.Name = $"{selectObject.GetType().Name}(TODO asset naame)";
                res.Properties = SerializedProperty.GetChildProperty(selectObject);
                res.target = selectObject;

                res.OnEnable();
                editors.Add(res);
            }
            else if (selectObject is Entity entity)
            {
                if (GameEngine.Instance.GameWorld != null)
                {
                    ComponentManager prefabManager = GameEngine.Instance.GameWorld.ComponentManager;

                    foreach (var mapper in prefabManager.ComponentMappers)
                    {
                        if (mapper != null && mapper.Has(entity.Id))
                        {
                            object component = mapper.GetObject(entity.Id);
                            Editor res = CreateEditorComponent(component);
                            //res.Mapper = mapper;
                            res.Name = mapper.ComponentType.Name;
                            res.Properties = SerializedProperty.GetChildProperty(component);
                            res.target = component;

                            res.OnEnable();
                            editors.Add(res);
                        }
                    }
                }
            }
        }

        private Editor CreateEditorComponent(object component)
        {
            Editor editor = null;

            foreach (var type in editorType)
            {
                if (type.GetCustomAttribute<CustomEditorAttribute>().EditorType == component.GetType())
                {
                    editor = (Editor)Activator.CreateInstance(type);
                }
            }

            if (editor == null)
            {
                editor = new Editor();
            }

            return editor;
        }

        protected override void OnGUI(GameTime gameTime)
        {
            if (ImGuiManager.SelectObject != selectObject)
            {
                selectObject = ImGuiManager.SelectObject;
                UpdateComponents();
            }
        
            if (ImGuiManager.SelectObject is Entity entity)
            {
                string name = "Entity name: " + entity.Name;
                string id = "Entity id: " + entity.Id;

                ImGui.Text(name);
                ImGui.Text(id);

                foreach (var component in editors)
                {
                    bool visible = true;

                    if (ImGui.CollapsingHeader(component.Name, ref visible, ImGuiTreeNodeFlags.DefaultOpen))
                    {
                        ImGui.Indent();

                        component.OnInspectorGUI();

                        ImGui.Unindent();
                    }

                    if (!visible)
                    {
                        //component.Mapper.Delete(entity.Id);
                        Debug.LogError("TODO DELECTE COMPONENT FROM ENTITY");
                        editors.Remove(component);
                        break;
                    }
                }

                if (ImGui.Button("Add component"))
                {
                    ImGui.OpenPopup("select");
                }

                if (ImGui.BeginPopup("select"))
                {
                    Type selectType = null;
                    int idx = 0;
                    foreach (var componentType in componentTypes)
                    {
                        bool selected = selectType == componentType;
                        ImGui.PushID(idx);
                        {
                            if (ImGui.Selectable(componentType.Name, selected))
                            {
                                selectType = componentType;
                            }

                            if (selected)
                            {
                                ImGui.SetItemDefaultFocus();
                            }
                        }
                        ImGui.PopID();
                        idx++;
                    }

                    if (selectType != null)
                    {
                        object component = Activator.CreateInstance(selectType);
                        Type type = component.GetType();

                        entity.Attach(type, component);
                        UpdateComponents();
                    }
                    ImGui.EndPopup();
                }
            }
            else if (editors != null)
            {
                foreach (var component in editors)
                {
                    if (ImGui.CollapsingHeader(component.Name, ImGuiTreeNodeFlags.DefaultOpen))
                    {
                        ImGui.Indent();

                        component.OnInspectorGUI();

                        ImGui.Unindent();
                    }
                }
            }
        }
    }
}
