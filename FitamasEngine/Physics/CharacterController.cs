using Fitamas.Entities;
using Fitamas.Extended.Entities;
using Fitamas.Gameplay.Characters;
using Fitamas.Math2D;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;
using System;
using System.Collections.Generic;

namespace Fitamas.Physics
{
    public class CharacterController : EntityFixedUpdateSystem
    {
        private const int bounces = 10;

        private ComponentMapper<Character> characterMapper;
        private ComponentMapper<Collider> colliderMapper;
        private ComponentMapper<CharacterElement> elementMapper;
        private World world;

        public CharacterController() : base(Aspect.All(typeof(Collider)).One(typeof(Character), typeof(CharacterElement)))
        {

        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            world = Physics2D.world;
            characterMapper = mapperService.GetMapper<Character>();
            colliderMapper = mapperService.GetMapper<Collider>();
            elementMapper = mapperService.GetMapper<CharacterElement>();
        }

        protected override void OnEntityAdded(int entityId)
        {
            InitCharacter(entityId);
        }

        protected override void OnEntityChanged(int entityId)
        {
            InitCharacter(entityId);
        }

        private void InitCharacter(int entityId)
        {
            if (ActiveEntities.Contains(entityId))
            {
                Character character = characterMapper.Get(entityId);
                Entity entity = GetEntity(entityId);

                if (character != null)
                {
                    character.Init(GameWorld.EntityManager, entity);
                }
            }
        }

        protected override void OnEntityRemoved(int entityId)
        {
            if (ActiveEntities.Contains(entityId))
            {
                Character character = characterMapper.Get(entityId);

                character?.Destroy();
            }
        }

        public override void FixedUpdate(float deltaTime)
        {
            foreach (var id in ActiveEntities)
            {
                if (characterMapper.Has(id))
                {
                    Collider collider = colliderMapper.Get(id);
                    Character character = characterMapper.Get(id);

                    if (character.isRagDoll)
                    {
                        character.CreateRagDoll();
                    }
                    else
                    {
                        character.RemoveRagDoll();
                    }

                    if (!character.canMove)
                    {
                        return;
                    }

                    Update(character, collider, deltaTime);
                }
            }
        }

        private void Update(Character character, Collider collider, float deltaTime) 
        {
            //collider.Body.LinearVelocity = character.velocity;

            GroundCheck(character, collider, collider.Position);
            RoofCheck(character, collider, collider.Position);

            //Debug.Log(character.isGrounded);

            if (character.isGrounded && character.isRoofed)
            {
                character.isSlide = false;
            }

            if (!character.isGrounded)
            {
                character.velocity += character.gravity;
            }
            else if (character.isSlide)
            {
                character.velocity += character.gravity * character.slideMult;
            }
            else if (character.isRoofed && character.velocity.Y > 0)
            {
                character.velocity.Y = 0;
            }
            else if (character.isGrounded && character.velocity.Y < 0)
            {
                character.velocity.Y = 0;
            }


            Vector2 delta = SlideRigidBody(character, collider, collider.Position, character.velocity * deltaTime);

            if (delta.Length() < character.minMoveDistance)
            {
                delta = Vector2.Zero;
            }
            if (delta.Length() > character.maxMoveDistance)
            {
                delta = delta.NormalizeF() * character.maxMoveDistance;
            }
            character.currentVelocity = delta;

            collider.BodyPosition += delta;


            //RaycastHit2D[] hits = CapsuleCastAll(position, Vector2.zero, 0, capsuleCollider.size + new Vector2(ContactOffset, contactOffset) * 2, capsuleCollider.offset);

            //List<IColliding> collid = new List<IColliding>();

            //foreach (RaycastHit2D _hit in hits)
            //{
            //    if (_hit.collider != null)
            //    {
            //        IColliding colliding = _hit.collider.GetComponent<IColliding>();

            //        if (colliding != null)
            //        {
            //            if (!collidings.Contains(colliding))
            //            {
            //                colliding.ColliderEnter(_hit, this);
            //            }
            //            collid.Add(colliding);
            //        }
            //    }
            //}

            //foreach (var col in collidings)
            //{
            //    if (!collid.Contains(col))
            //    {
            //        col.ColliderExit(this);
            //    }
            //}
            //collidings = collid;

            //foreach (var col in collidings)
            //{
            //    col.ColliderStay(this);
            //}
        }

        private Vector2 SlideRigidBody(Character character, Collider collider, Vector2 origin, Vector2 initVelocity)
        {
            Vector2 position = origin;
            Vector2 velocity = initVelocity;

            for (int i = 0; i < bounces; i++)
            {
                float distance = velocity.Length() + character.skinSize;
                bool isHit = false;
                RayCastHit hit = new RayCastHit();
                RayCastHit[] hits = collider.CapsuleCast(position, velocity, distance, character.layerMask);

                if (hits.Length > 0)
                {
                    hit = hits[0];
                    isHit = true;
                }

                //foreach (var _hit in hits)
                //{
                //    //ChechCollider(_hit);

                //    //if (_hit.collider != null && !_hit.collider.isTrigger && !ignoreColliders.Contains(_hit.collider)/*&& _hit.distance > hit.distance*/)
                //    if (_hit.distance < hit.distance || !isHit)
                //    {
                //        hit = _hit;
                //        isHit = true;
                //    }
                //}

                if (isHit)
                {
                    Vector2 snapToSurface = velocity.NormalizeF() * (hit.distance - character.skinSize);
                    Vector2 leftTover = velocity - snapToSurface;
                    float angle = MathV.AngleDegrees(new Vector2(0, 1), hit.normal);

                    //Debug.Log(hit.normal);

                    if (hit.distance <= character.skinSize)
                    {
                        snapToSurface = Vector2.Zero;
                    }

                    //пол стены
                    if (angle <= 90)
                    {
                        //пол
                        if (angle <= character.maxSlopeAngle || !character.isGrounded || character.isSlide) //(!isRoofed)
                        {
                            leftTover = MathV.Project(leftTover, hit.normal);
                        }
                        //ступени и стены
                        else if (character.isGrounded && !character.isSlide && !character.isRoofed)
                        {
                            //Vector2 test = hit.collider.ClosestPoint(hit.point + new Vector2(0, maxHightStair));
                            //float stair = test.y - (UnderPoint.y + position.y);
                            //Vector2 offset = Vector2.Zero;
                            //if (stair <= maxHightStair)
                            //{
                            //    offset = UpStairs(position, velocity.x, stair);
                            //}

                            ////ступени
                            //if (offset != Vector2.Zero)
                            //{
                            //    position += offset;
                            //}
                            ////стены
                            //else
                            //{
                            //    leftTover = Project(leftTover, hit.normal);
                            //}

                            leftTover = MathV.Project(leftTover, hit.normal);
                        }
                        //потолок и стена
                        else
                        {
                            snapToSurface = Vector2.Zero;
                            leftTover = Vector2.Zero;
                        }
                    }
                    //потолок
                    else
                    {
                        //потолок и земля
                        if (character.isGrounded && character.isRoofed)
                        {
                            snapToSurface = Vector2.Zero;
                            leftTover = Vector2.Zero;
                        }
                        // потолок или стена или земля
                        else
                        {
                            leftTover = MathV.Project(leftTover, hit.normal);
                        }
                    }

                    velocity = leftTover;
                    position += snapToSurface;
                }
                else
                {
                    position += velocity;
                    break;
                }
            }

            if (initVelocity.Y <= 0 && character.isGrounded && !character.isRoofed)
            {
                position += DownStairs(character, collider, position);
            }

            return position - origin;
        }



        private Vector2 DownStairs(Character character, Collider collider, Vector2 position)
        {
            float distance = character.maxHightStair + character.skinSize;
            //RaycastHit2D hit = CapsuleCast(position, Vector2.down, distance);
            RayCastHit[] hits = collider.CapsuleCast(position, new Vector2(0, -1), distance, character.layerMask);

            if (hits.Length > 0) 
            { 
                RayCastHit hit = hits[0];

                float angle = MathV.AngleDegrees(new Vector2(0, 1), hit.normal);

                if (angle <= 90 && hit.distance >= character.skinSize)
                {
                    Vector2 snapToSurface = new Vector2(0, -1) * (hit.distance - character.skinSize);
                    return snapToSurface;
                }
            }

            //foreach (var hit in hits)
            //{
            //    //ChechCollider(hit);

            //    //if (hit.collider != null && !hit.collider.isTrigger && !ignoreColliders.Contains(hit.collider))
            //    {

            //    }
            //}

            return Vector2.Zero;
        }

        private void GroundCheck(Character character, Collider collider, Vector2 position)
        {
            bool grounded = false;
            bool slide = false;

            character.groundNormal = Vector2.Zero;

            //RaycastHit2D hit = CapsuleCast(position, Vector3.down, 2 * contactOffset);
            RayCastHit[] hits = collider.CapsuleCast(position, new Vector2(0, -1), character.skinSize * 2, character.layerMask);

            foreach (RayCastHit hit in hits)
            {
                //ChechCollider(hit);

                //if (hit.collider != null && !hit.collider.isTrigger && !ignoreColliders.Contains(hit.collider))
                {
                    float angle = MathV.AngleDegrees(new Vector2(0, 1), hit.normal);

                    if (angle <= 90/* && hit.distance < character.skinSize * 2*/)
                    {
                        character.groundNormal = hit.normal;
                        grounded = true;
                        if (angle > character.maxSlopeAngle)
                        {
                            slide = true;
                        }

                        break;
                    }
                }
            }



            bool isGroundExemption = character.isGrounded == true && grounded == false;
            bool isGroundLanding = character.isGrounded == false && grounded == true;

            character.isSlide = slide;
            character.isGrounded = grounded;

            if (isGroundExemption)
            {
                //onGroundExemption.Invoke();
            }
            else if (isGroundLanding)
            {
                //onGroundLanding.Invoke();

                character.velocity.Y = 0;
            }
        }

        private void RoofCheck(Character character, Collider collider, Vector2 position)
        {
            bool roofed = false;

            character.roofNormal = Vector2.Zero;

            //RaycastHit2D hit = CapsuleCast(position, Vector3.up, 2 * contactOffset);
            RayCastHit[] hits = collider.CapsuleCast(position, new Vector2(0, 1), character.skinSize * 2, character.layerMask); //CapsuleCastAll(position, Vector2.down, 2 * contactOffset);

            foreach (var hit in hits)
            {
                //ChechCollider(hit);

                //if (hit.collider != null && !hit.collider.isTrigger && !ignoreColliders.Contains(hit.collider))
                {
                    float angle = MathV.AngleDegrees(new Vector2(0, 1), hit.normal);

                    if (angle > 90)
                    {
                        character.roofNormal = hit.normal;
                        roofed = true;
                        break;
                    }
                }
            }

            character.isRoofed = roofed;
        }
    }
}
