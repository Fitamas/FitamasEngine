using Fitamas.Entities;
using Fitamas.Physics;
using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Physics.Characters
{
    public enum CharacterState
    {
        idle,
        walk,
        run,
        crouch,
        fly,
    }

    public class Character
    {
        [SerializableField] private Avatar avatar;

        private bool isInit = false;
        private bool isRagDollCreate = false;

        public Avatar Avatar => avatar;

        public CharacterState characterState = CharacterState.idle;

        public Vector2 gravity = new Vector2(0, -9.8f);
        public float moveSpeed = 4;
        public float maxSlopeAngle = 45;
        public float skinSize = 0.05f;
        public float maxHightStair = 1;
        public float minDepthStair = 0.5f;
        public float minMoveDistance = 0.001f;
        public float maxMoveDistance = 2000;
        public float slideMult = 0.5f;
        public Layer layer = Layer.Players;
        public LayerMask layerMask;

        public Vector2 velocity;
        public Vector2 groundNormal;
        public Vector2 roofNormal;
        public Vector2 currentVelocity;
        public bool isRoofed = false;
        public bool isGrounded = false;
        public bool isSlide = false;

        public bool canMove = true;
        public bool isRagDoll = false;

        public Character()
        {
            layerMask = LayerManager.GetMask(layer);
            isInit = false;
        }

        public Character(Avatar avatar) : this()
        {
            this.avatar = avatar;
        }

        public void Init(EntityManager entityManager, Entity root)
        {
            if (!isInit)
            {
                avatar?.CreateElements(entityManager, root);
                isInit = true;
            }
        }

        public void Destroy()
        {
            if (isInit)
            {
                isInit = false;
            }
        }

        public void CreateRagDoll()
        {
            if (!isRagDollCreate && avatar != null)
            {
                avatar.SetWeights(0);
                Joint2DHelper.CreateRagDoll(avatar.GetElements());
                isRagDoll = true;
                canMove = false;
                isRagDollCreate = true;
            }
        }

        public void RemoveRagDoll()
        {
            if (isRagDollCreate && avatar != null)
            {
                avatar.SetWeights(1);
                Joint2DHelper.RemoveRagDoll(avatar.GetElements());
                isRagDoll = false;
                canMove = true;
                isRagDollCreate = false;
            }
        }
    }
}
