using Fitamas.Entities;
using Fitamas.Physics;
using Fitamas.Serializeble;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fitamas.Physics.Characters
{
    public abstract class Avatar
    {
        [SerializableField] protected Dictionary<string, float> weights = new Dictionary<string, float>();

        public void SetWeights(float weight)
        {
            foreach (var key in weights.Keys)
            {
                weights[key] = weight;
            }
        }

        public float GetWeight(string name)
        {
            return weights[name];
        }

        public void SetWeight(string name, float weight)
        {
            weights[name] = weight;
        }

        public abstract void CreateElements(EntityManager entityManager, Entity root);
        public abstract Entity[] GetElements();
        public abstract Entity GetElement(string name);

        public abstract Transform GetTransform(string name);
    }

    public class DefoultAvatar : Avatar
    {
        [SerializableField] private Entity entity;

        public DefoultAvatar(Entity entity)
        {
            this.entity = entity;
            weights["root"] = 0;
        }

        public override void CreateElements(EntityManager entityManager, Entity root)
        {

        }

        public override Entity GetElement(string name)
        {
            return entity;
        }

        public override Entity[] GetElements()
        {
            return new Entity[] { entity };
        }

        public override Transform GetTransform(string name)
        {
            throw new NotImplementedException();
        }
    }

    public class HumanAvatar : Avatar
    {
        public const string Head = "head";
        public const string Body = "body";

        public const string RightArmUp = "rightArmUp";
        public const string RightArmDown = "rightArmDown";
        public const string RightHand = "rightHand";

        public const string LeftArmUp = "leftArmUp";
        public const string LeftArmDown = "leftArmDown";
        public const string LeftHand = "leftHand";

        public const string RightLegUp = "rightLegUp";
        public const string RightLegDown = "rightLegDown";
        public const string RightFoot = "rightFoot";

        public const string LeftLegUp = "leftLegUp";
        public const string LeftLegDown = "leftLegDown";
        public const string LeftFoot = "leftFoot";

        private Dictionary<string, Entity> elements;

        public string head;
        public string body;

        public string rightArmUp;
        public string rightArmDown;
        public string rightHand;

        public string leftArmUp;
        public string leftArmDown;
        public string leftHand;

        public string rightLegUp;
        public string rightLegDown;
        public string rightFoot;

        public string leftLegUp;
        public string leftLegDown;
        public string leftFoot;

        public override void CreateElements(EntityManager entityManager, Entity root)
        {
            elements = new Dictionary<string, Entity>();

            List<Entity> entities = new List<Entity>
            {
                //entityManager.Create(head),
                //entityManager.Create(body),
                //entityManager.Create(rightArmUp),
                //entityManager.Create(rightArmDown),
                //entityManager.Create(rightHand),
                //entityManager.Create(leftArmUp),
                //entityManager.Create(leftArmDown),
                //entityManager.Create(leftHand),
                //entityManager.Create(rightLegUp),
                //entityManager.Create(rightLegDown),
                //entityManager.Create(rightFoot),
                //entityManager.Create(leftLegUp),
                //entityManager.Create(leftLegDown),
                //entityManager.Create(leftFoot)
            };

            List<Transform> transforms = new List<Transform>();
            List<CharacterElement> characterElements = new List<CharacterElement>();

            foreach (var entity in entities)
            {
                if (entity.TryGet(out CharacterElement element))
                {
                    elements[element.Name] = entity;
                    weights[element.Name] = 0;
                    characterElements.Add(element);
                }

                if (entity.TryGet(out Transform transform))
                {
                    transforms.Add(transform);
                }
            }

            for (int i = 0; i < transforms.Count; i++)
            {
                if (characterElements[i].IsRoot)
                {
                    transforms[i].Parent = root.Get<Transform>();

                    transforms[i].LocalPosition = characterElements[i].ConnetionPosition;
                }

                for (int j = 0; j < transforms.Count; j++)
                {
                    if (characterElements[i].ParentElement == characterElements[j].Name)
                    {
                        transforms[i].Parent = transforms[j];

                        transforms[i].LocalPosition = characterElements[i].ConnetionPosition;
                    }
                }
            }
        }

        public override Entity[] GetElements()
        {
            return elements.Values.ToArray();
        }

        public override Entity GetElement(string name)
        {
            return elements[name];
        }

        public override Transform GetTransform(string name)
        {
            Entity entity = GetElement(name);

            if (entity.TryGet(out Transform transform))
            {
                return transform;
            }

            return null;
        }
    }
}
