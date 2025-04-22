using Fitamas.Entities;
using Fitamas.Graphics;
using Fitamas.Input;
using Fitamas.Math2D;
using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using System;
using Fitamas.Physics.Characters;

namespace Fitamas.Animation
{
    public abstract class AnimationController
    {
        protected Animator animator;
        protected Entity root;

        public void Init(Entity root, Animator animator)
        {
            this.root = root;
            this.animator = animator;
            OnInit();
        }

        public virtual void SetAvatar(Avatar avatar) 
        { 
        
        }

        public void Update(GameTime gameTime)
        {
            OnUpdate(gameTime);
        }

        protected abstract void OnInit();

        protected abstract void OnUpdate(GameTime gameTime);
    }

    public class HumanAnimationController : AnimationController
    {
        private HumanAvatar humenAvatar;

        private AimRig head;
        private MoveRig body;
        private TwoBoneRig rightLeg;
        private TwoBoneRig leftLeg;
        private TwoBoneRig rightArm;
        private TwoBoneRig leftArm;

        //head
        [SerializableField] private Vector2 headVectorUp = new Vector2(0, 1);
        [SerializableField] private Vector2 headVectorRight = new Vector2(1, 0);

        //body
        [SerializableField] private Vector2 bodyPosition = new Vector2(0, 0.5f);
        [SerializableField] private float bodyIdleAmplitude = 0.04f;
        [SerializableField] private float bodyIdleSpeed = 1f;

        //arm
        [SerializableField] private Vector2 armPosition = new Vector2(0, -0.5f);

        //walp
        [SerializableField] private float bodyWalkAmplitude = 0.1f;
        [SerializableField] private float bodyWalkSpeed = 2f;
        [SerializableField] private float bodyWalkAngle = 0.2f;

        [SerializableField] private float armWalkAmlitude = 0.35f;
        [SerializableField] private float armWalkSpeed = 2f;

        [SerializableField] private float lenghtStep = 0.6f;
        [SerializableField] private float height = 0.4f;
        [SerializableField] private float speed = 2.8f;
        [SerializableField] private Vector2 legPosition = new Vector2(-0.2f, -1.7f);

        private bool lookAtRight;
        private bool isFlip;

        protected override void OnInit()
        {
            RigController rigController = animator.RigController;

            head = new AimRig(rigController, HumanAvatar.Head, headVectorUp, headVectorRight);

            body = new MoveRig(rigController, HumanAvatar.Body);

            leftLeg = new TwoBoneRig(rigController, HumanAvatar.LeftLegUp, HumanAvatar.LeftLegDown, HumanAvatar.LeftFoot);
            rightLeg = new TwoBoneRig(rigController, HumanAvatar.RightLegUp, HumanAvatar.RightLegDown, HumanAvatar.RightFoot);

            leftArm = new TwoBoneRig(rigController, HumanAvatar.LeftArmUp, HumanAvatar.LeftArmDown, HumanAvatar.LeftHand);
            rightArm = new TwoBoneRig(rigController, HumanAvatar.RightArmUp, HumanAvatar.RightArmDown, HumanAvatar.RightHand);
        }

        public override void SetAvatar(Avatar _avatar)
        {
            if (animator.Avatar is HumanAvatar avatar)
            {
                this.humenAvatar = avatar;
            }
            else
            {
                return;
            }

            humenAvatar.SetWeights(1);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            Character character = root.Get<Character>();    
            InputRequest inputRequest = root.Get<InputRequest>();
            Transform transform = root.Get<Transform>();


            //Flip sprite
            float lookDirection = MathV.Sign(inputRequest.Target.X - transform.Position.X);
            bool lookAtRight = inputRequest.Target.X > transform.Position.X;

            if (this.lookAtRight != lookAtRight)
            {
                this.lookAtRight = lookAtRight;
                isFlip = true;
            }

            if (isFlip)
            {
                isFlip = false;
                Entity[] entities = humenAvatar.GetElements();

                foreach (Entity entity in entities)
                {
                    if (entity.TryGet(out SpriteRender sprite))
                    {
                        sprite.flipHorizontally = !this.lookAtRight;
                    }
                }
            }

            //Head rig
            head.Target = inputRequest.Target;

            //Leg rig
            float time = (float)gameTime.TotalGameTime.TotalSeconds * speed;
            float direction = MathV.Sign(inputRequest.MoveDirection.X);

            Vector2 leftArmPosition = armPosition;
            Vector2 rightArmPosition = armPosition;

            Vector2 leftLegPosition = legPosition;
            Vector2 rightLegPosition = legPosition;
            switch(character.characterState)
            {
                case CharacterState.idle:
                    leftLegPosition = new Vector2(0.2f, -1.7f);
                    rightLegPosition = new Vector2(-0.4f, -1.7f);

                    body.localPosition = bodyPosition + Move(new Vector2(0, 1), time * bodyIdleSpeed, bodyIdleAmplitude);
                    body.angle = 0;
                    break;
                case CharacterState.walk:
                    leftArmPosition += Move(new Vector2(1, 0), -time * armWalkSpeed, armWalkAmlitude);
                    rightArmPosition += Move(new Vector2(1, 0), time * armWalkSpeed, armWalkAmlitude);

                    leftLegPosition += MoveLeg(time * speed, lenghtStep, height);
                    rightLegPosition += MoveLeg((time + MathF.PI / 2) * speed, lenghtStep, height);

                    leftLegPosition.X *= direction;
                    rightLegPosition.X *= direction;

                    body.angle = -direction * bodyWalkAngle;
                    body.localPosition = bodyPosition + Move(new Vector2(0, 1), time * bodyWalkSpeed, bodyWalkAmplitude);
                    break;
                case CharacterState.run:
                    break;
            }

            leftArm.direction.X = -lookDirection;
            rightArm.direction.X = -lookDirection;

            leftLeg.direction.X = lookDirection;
            rightLeg.direction.X = lookDirection;

            leftArm.Target = transform.ToAbsolutePosition(leftArmPosition);
            rightArm.Target = transform.ToAbsolutePosition(rightArmPosition);

            leftLeg.Target = transform.ToAbsolutePosition(leftLegPosition);
            rightLeg.Target = transform.ToAbsolutePosition(rightLegPosition);
        }

        private Vector2 MoveLeg(float time, float lenghtStep, float height)
        {
            float x = MathF.Sin(time);

            Vector2 position;
            if (Math.Cos(time) >= 0)
            {
                position = new Vector2(x * lenghtStep, MathF.Cos(x * MathF.PI / 2) * height);
            }
            else
            {
                position = new Vector2(x * lenghtStep, 0);
            }

            return position;
        }

        private Vector2 Move(Vector2 direction, float time, float amlitude)
        {
            float magnitude = MathF.Sin(time) * amlitude;

            return direction.NormalizeF() * magnitude;
        }
    }
}
