using Microsoft.Xna.Framework;

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
        public CollisionFilter layerMask;

        public Vector2 velocity;
        public Vector2 groundNormal;
        public Vector2 roofNormal;
        public Vector2 currentVelocity;
        public bool isRoofed = false;
        public bool isGrounded = false;
        public bool isSlide = false;
        public bool canMove = true;

        public Character()
        {

        }
    }
}
