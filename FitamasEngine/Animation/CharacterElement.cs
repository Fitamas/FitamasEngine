using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Animation
{
    public class CharacterElement
    {
        [SerializeField] private bool isRoot;
        [SerializeField] private string name = "element";
        [SerializeField] private string parentElement = "element";
        [SerializeField] private Vector2 connectPosition;

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
