using Fitamas.ECS;
using Fitamas.Graphics;
using Fitamas.Math;
using ImGuiNET;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.ImGuiNet.Editors
{
    public class Editor
    {
        public string Name { get; set; }
        public SerializedProperty[] Properties { get; set; }
        public object target { get; set; }

        public virtual void OnEnable()
        {

        }

        public virtual void OnInspectorGUI()
        {
            foreach (var property in Properties)
            {
                ImGuiUtils.PropertyField(property);
            }
        }

        public virtual void OnDisable()
        {

        }
    }

    [CustomEditor(typeof(Transform))]
    public class TransformEditor : Editor
    {
        private Transform transform;

        public override void OnEnable()
        {
            transform = (Transform)target;
        }

        public override void OnInspectorGUI()
        {
            Vector2 position = transform.LocalPosition;
            if (ImGuiUtils.InputVector2("Position", ref position))
            {
                transform.LocalPosition = position;
            }
            float rotation = transform.LocalEulerRotation;
            if (ImGui.DragFloat("Rotation", ref rotation))
            {
                transform.LocalEulerRotation = rotation;
            }
            Vector2 scale = transform.LocalScale;
            if (ImGuiUtils.InputVector2("Scale", ref scale))
            {
                transform.LocalScale = scale;
            }
            Transform parent = transform.Parent;
            if (ImGuiUtils.PropertyReference("Parent", ref parent))
            {
                transform.Parent = parent;
            }
        }
    }

    [CustomEditor(typeof(MeshComponent))]
    public class MeshEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            ImGui.Text("TODO MESH EDITOR");







            //TODO


            base.OnInspectorGUI();
        }
    }

    [CustomEditor(typeof(Sprite))]
    public class TextureAtlas : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (ImGui.Button("EditRegion"))
            {
                //TODO EDITOR ATLAS



                //Create new atlas if texture change



                Debug.LogWarning("TODO EDITOR ATLAS");
            }
        }
    }

    public class CustomEditorAttribute : Attribute
    {
        private Type type;

        public Type EditorType => type;

        public CustomEditorAttribute(Type type)
        {
            this.type = type;
        }
    }
}
