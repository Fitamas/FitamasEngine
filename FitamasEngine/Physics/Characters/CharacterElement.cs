using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Physics.Characters
{
    public class CharacterElement
    {
        [SerializableField] private bool isRoot;
        [SerializableField] private string name = "element";
        [SerializableField] private string parentElement = "element";
        [SerializableField] private Vector2 connectPosition;

        public bool IsRoot => isRoot;
        public string Name => name;
        public string ParentElement => parentElement;
        public Vector2 ConnetionPosition => connectPosition;

        public CharacterElement()
        {

        }

        public CharacterElement(string name, Vector2 localPosition)
        {
            this.name = name;
            connectPosition = localPosition;
            isRoot = true;
        }

        public CharacterElement(string name, string parentName, Vector2 localPosition)
        {
            this.name = name;
            parentElement = parentName;
            connectPosition = localPosition;
            isRoot = false;
        }
    }
}
