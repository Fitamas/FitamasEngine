using Fitamas.Core;
using Fitamas.ECS;
using Fitamas.Serialization;
using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fitamas.ImGuiNet.Windows
{
    public class InspectorWindow : EditorWindow
    {
        private Type[] componentTypes;
        private Type[] editorType;

        private object selectObject;
        private List<EditorComponent> components;

        public InspectorWindow()
        {
            Name = "Inspector";
        }

        protected override void OnOpen()
        {
            componentTypes = ReflectionUtils.GetTypesAssignableFrom<Component>();
            editorType = ReflectionUtils.GetTypes<CustomEditorAttribute>();
        }

        private void UpdateComponents()
        {
            components = new List<EditorComponent>();

            if (selectObject is MonoObject serializeble)
            {
                EditorComponent res = CreateEditorComponent(selectObject);
                res.Name = serializeble.GetType().Name;
                res.Properties = SerializedProperty.GetChildProperty(serializeble);
                res.target = serializeble;

                res.OnEnable();
                components.Add(res);
            }
            else if (selectObject is Entity entity)
            {
                if (manager.Game.GameWorld != null)
                {
                    ComponentManager prefabManager = manager.Game.GameWorld.ComponentManager;

                    foreach (var mapper in prefabManager.ComponentMappers)
                    {
                        if (mapper != null && mapper.Has(entity.Id))
                        {
                            object component = mapper.GetObject(entity.Id);
                            EditorComponent res = CreateEditorComponent(component);
                            res.Mapper = mapper;
                            res.Name = mapper.ComponentType.Name;
                            res.Properties = SerializedProperty.GetChildProperty(component);
                            res.target = component;

                            res.OnEnable();
                            components.Add(res);
                        }
                    }
                }
            }
        }

        private EditorComponent CreateEditorComponent(object component)
        {
            EditorComponent editor = null;

            foreach (var type in editorType)
            {
                if (type.GetCustomAttribute<CustomEditorAttribute>().EditorType == component.GetType())
                {
                    editor = (EditorComponent)Activator.CreateInstance(type);
                }
            }

            if (editor == null)
            {
                editor = new EditorComponent();
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

                foreach (var component in components)
                {
                    bool visible = true;

                    if (ImGui.CollapsingHeader(component.Name, ref visible))
                    {
                        ImGui.Indent();

                        component.OnInspectorGUI();

                        ImGui.Unindent();
                    }

                    if (!visible)
                    {
                        component.Mapper.Delete(entity.Id);
                        components.Remove(component);
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
            else if (components != null)
            {
                foreach (var component in components)
                {
                    if (ImGui.CollapsingHeader(component.Name))
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
