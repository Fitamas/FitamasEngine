using Fitamas.Animation;
using Fitamas.Entities;
using Fitamas.Extended.Entities;
using Fitamas.Math2D;
using Microsoft.Xna.Framework;

namespace Fitamas.Physics.Characters
{
    public class CharacterController : EntityFixedUpdateSystem
    {
        private const int bounces = 10;

        private PhysicsWorldSystem physicsWorld;

        private ComponentMapper<Character> characterMapper;
        private ComponentMapper<PhysicsCollider> colliderMapper;

        public CharacterController(PhysicsWorldSystem physicsWorld) : base(Aspect.All(typeof(PhysicsCollider), typeof(Character)))
        {
            this.physicsWorld = physicsWorld;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            characterMapper = mapperService.GetMapper<Character>();
            colliderMapper = mapperService.GetMapper<PhysicsCollider>();
        }

        public override void FixedUpdate(float deltaTime)
        {
            foreach (var id in ActiveEntities)
            {
                if (characterMapper.Has(id))
                {
                    PhysicsCollider collider = colliderMapper.Get(id);
                    Character character = characterMapper.Get(id);

                    if (!character.canMove)
                    {
                        return;
                    }

                    //Update(character, collider, deltaTime);
                }
            }
        }

        //private void Update(Character character, Collider collider, float deltaTime)
        //{
        //    //collider.Body.LinearVelocity = character.velocity;

        //    GroundCheck(character, collider, collider.Position);
        //    RoofCheck(character, collider, collider.Position);

        //    //Debug.Log(character.isGrounded);

        //    if (character.isGrounded && character.isRoofed)
        //    {
        //        character.isSlide = false;
        //    }

        //    if (!character.isGrounded)
        //    {
        //        character.velocity += character.gravity;
        //    }
        //    else if (character.isSlide)
        //    {
        //        character.velocity += character.gravity * character.slideMult;
        //    }
        //    else if (character.isRoofed && character.velocity.Y > 0)
        //    {
        //        character.velocity.Y = 0;
        //    }
        //    else if (character.isGrounded && character.velocity.Y < 0)
        //    {
        //        character.velocity.Y = 0;
        //    }


        //    Vector2 delta = SlideRigidBody(character, collider, collider.Position, character.velocity * deltaTime);

        //    if (delta.Length() < character.minMoveDistance)
        //    {
        //        delta = Vector2.Zero;
        //    }
        //    if (delta.Length() > character.maxMoveDistance)
        //    {
        //        delta = delta.NormalizeF() * character.maxMoveDistance;
        //    }
        //    character.currentVelocity = delta;

        //    collider.BodyPosition += delta;


        //    //RaycastHit2D[] hits = CapsuleCastAll(position, Vector2.zero, 0, capsuleCollider.size + new Vector2(ContactOffset, contactOffset) * 2, capsuleCollider.offset);

        //    //List<IColliding> collid = new List<IColliding>();

        //    //foreach (RaycastHit2D _hit in hits)
        //    //{
        //    //    if (_hit.collider != null)
        //    //    {
        //    //        IColliding colliding = _hit.collider.GetComponent<IColliding>();

        //    //        if (colliding != null)
        //    //        {
        //    //            if (!collidings.Contains(colliding))
        //    //            {
        //    //                colliding.ColliderEnter(_hit, this);
        //    //            }
        //    //            collid.Add(colliding);
        //    //        }
        //    //    }
        //    //}

        //    //foreach (var col in collidings)
        //    //{
        //    //    if (!collid.Contains(col))
        //    //    {
        //    //        col.ColliderExit(this);
        //    //    }
        //    //}
        //    //collidings = collid;

        //    //foreach (var col in collidings)
        //    //{
        //    //    col.ColliderStay(this);
        //    //}
        //}

        private Vector2 SlideRigidBody(Character character, PhysicsCollider collider, Vector2 origin, Vector2 initVelocity)
        {
            Vector2 position = origin;
            Vector2 velocity = initVelocity;

            for (int i = 0; i < bounces; i++)
            {
                float distance = velocity.Length() + character.skinSize;
                bool isHit = false;
                RayCastHit hit = new RayCastHit();
                RayCastHit[] hits = physicsWorld.CapsuleCast(position, velocity, distance, collider.Size, character.layerMask);

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
                    Vector2 snapToSurface = velocity.NormalizeF() * (hit.Distance - character.skinSize);
                    Vector2 leftTover = velocity - snapToSurface;
                    float angle = MathV.AngleDegrees(new Vector2(0, 1), hit.Normal);

                    //Debug.Log(hit.normal);

                    if (hit.Distance <= character.skinSize)
                    {
                        snapToSurface = Vector2.Zero;
                    }

                    //пол стены
                    if (angle <= 90)
                    {
                        //пол
                        if (angle <= character.maxSlopeAngle || !character.isGrounded || character.isSlide) //(!isRoofed)
                        {
                            leftTover = MathV.Project(leftTover, hit.Normal);
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

                            leftTover = MathV.Project(leftTover, hit.Normal);
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
                            leftTover = MathV.Project(leftTover, hit.Normal);
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



        private Vector2 DownStairs(Character character, PhysicsCollider collider, Vector2 position)
        {
            float distance = character.maxHightStair + character.skinSize;
            //RaycastHit2D hit = CapsuleCast(position, Vector2.down, distance);
            RayCastHit[] hits = physicsWorld.CapsuleCast(position, new Vector2(0, -1), distance, collider.Size, character.layerMask);

            if (hits.Length > 0)
            {
                RayCastHit hit = hits[0];

                float angle = MathV.AngleDegrees(new Vector2(0, 1), hit.Normal);

                if (angle <= 90 && hit.Distance >= character.skinSize)
                {
                    Vector2 snapToSurface = new Vector2(0, -1) * (hit.Distance - character.skinSize);
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

        private void GroundCheck(Character character, PhysicsCollider collider, Vector2 position)
        {
            bool grounded = false;
            bool slide = false;

            character.groundNormal = Vector2.Zero;

            //RaycastHit2D hit = CapsuleCast(position, Vector3.down, 2 * contactOffset);
            RayCastHit[] hits = physicsWorld.CapsuleCast(position, new Vector2(0, -1), character.skinSize * 2, collider.Size, character.layerMask);

            foreach (RayCastHit hit in hits)
            {
                //ChechCollider(hit);

                //if (hit.collider != null && !hit.collider.isTrigger && !ignoreColliders.Contains(hit.collider))
                {
                    float angle = MathV.AngleDegrees(new Vector2(0, 1), hit.Normal);

                    if (angle <= 90/* && hit.distance < character.skinSize * 2*/)
                    {
                        character.groundNormal = hit.Normal;
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

        private void RoofCheck(Character character, PhysicsCollider collider, Vector2 position)
        {
            bool roofed = false;

            character.roofNormal = Vector2.Zero;

            //RaycastHit2D hit = CapsuleCast(position, Vector3.up, 2 * contactOffset);
            RayCastHit[] hits = physicsWorld.CapsuleCast(position, new Vector2(0, 1), character.skinSize * 2, collider.Size, character.layerMask); //CapsuleCastAll(position, Vector2.down, 2 * contactOffset);

            foreach (var hit in hits)
            {
                //ChechCollider(hit);

                //if (hit.collider != null && !hit.collider.isTrigger && !ignoreColliders.Contains(hit.collider))
                {
                    float angle = MathV.AngleDegrees(new Vector2(0, 1), hit.Normal);

                    if (angle > 90)
                    {
                        character.roofNormal = hit.Normal;
                        roofed = true;
                        break;
                    }
                }
            }

            character.isRoofed = roofed;
        }
    }
}
