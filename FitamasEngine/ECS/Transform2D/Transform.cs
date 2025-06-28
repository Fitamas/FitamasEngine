using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Fitamas.Serialization;

namespace Fitamas.ECS
{
    public class Transform : Component
    {
        [SerializeField] private Vector2 localPosition = Vector2.Zero;
        [SerializeField] private Vector2 localScale = Vector2.One;
        [SerializeField] private float localRotation = 0;
        [SerializeField] private Transform parent;
        [SerializeField] private List<Transform> children = new List<Transform>();

        private bool isAbsoluteDirty = true;
        private bool isLocalDirty = true;
        private float absoluteRotation;
        private Vector2 absoluteScale, absolutePosition;
        private Matrix absoluteMatrix, invertAbsoluteMatrix, localMatrix;

        public Transform(Vector2 position, Vector2 scale, float rotation) : this()
        {
            Position = position;
            Scale = scale;
            Rotation = rotation;
        }

        public Transform()
        {
            isAbsoluteDirty = true;
            isLocalDirty = true;
        }

        public Transform Parent
        {
            get => parent;
            set
            {
                if (parent != value && value != this)
                {
                    if (parent != null)
                    {
                        parent.children.Remove(this);
                    }

                    LocalPosition = value.ToLocalPosition(Position);

                    parent = value;

                    if (parent != null)
                    {
                        parent.children.Add(this);
                    }

                    SetLocalDirty();
                }
            }
        }

        public List<Transform> Children => children;

        public float EulerRotation
        {
            get
            {
                return Rotation * 180 / MathF.PI;
            }
            set
            {
                Rotation = value / 180 * MathF.PI;
            }
        }

        public float Rotation
        {
            get
            {
                UpdateLocal();
                UpdateAbsolute();
                return absoluteRotation;
            }
            set
            {
                if (parent != null)
                {
                    localRotation = value - parent.Rotation;
                }
                else
                {
                    localRotation = value;
                }

                SetLocalDirty();
            }
        }

        public Vector2 Scale
        {
            get
            {
                UpdateLocal();
                UpdateAbsolute();
                return absoluteScale;
            }
            set
            {
                if (parent != null)
                {
                    localScale = value / parent.Scale;
                }
                else
                {
                    localScale = value;
                }

                SetLocalDirty();
            }
        }

        public Vector2 Position
        {
            get
            {
                UpdateLocal();
                UpdateAbsolute();
                return absolutePosition;
            }
            set
            {
                if (parent != null)
                {
                    localPosition = parent.ToLocalPosition(value);
                }
                else
                {
                    localPosition = value;
                }
                
                SetLocalDirty();
            }
        }

        public float LocalEulerRotation
        {
            get
            {
                return LocalRotation * 180 / MathF.PI;
            }
            set
            {
                float angle = value * MathF.PI / 180;
                if (LocalRotation != angle)
                {
                    LocalRotation = angle;

                    SetLocalDirty();
                }
                
            }
        }

        public float LocalRotation
        {
            get
            {
                UpdateLocal();
                UpdateAbsolute();
                return localRotation;
            }
            set
            {
                if (localRotation != value)
                {
                    localRotation = value;

                    SetLocalDirty();
                }
            }
        }

        public Vector2 LocalPosition
        {
            get
            {
                UpdateLocal();
                UpdateAbsolute();
                return localPosition;
            }
            set
            {
                if (localPosition != value)
                {
                    localPosition = value;

                    SetLocalDirty();
                }
            }
        }

        public Vector2 LocalScale
        {
            get
            {
                UpdateLocal();
                UpdateAbsolute();
                return localScale;
            }
            set
            {
                if (localScale != value)
                {
                    localScale = value;

                    SetLocalDirty();
                }
            }
        }

        public Matrix LocalMatrix
        {
            get
            {
                UpdateLocal();
                UpdateLocal();
                return localMatrix;
            }
        }

        public Matrix AbsoluteMatrix
        {
            get
            {
                UpdateLocal();
                UpdateAbsolute();
                return absoluteMatrix;
            }
        }

        public Matrix InvertAbsoluteMatrix
        {
            get
            {
                UpdateLocal();
                UpdateAbsolute();
                return invertAbsoluteMatrix;
            }
        }

        public Vector2 Up
        {
            get
            {
                return ToAbsolutePosition(new Vector2(0, 1));
            }
        }

        public Vector2 Down
        {
            get
            {
                return ToAbsolutePosition(new Vector2(0, -1));
            }
        }

        public Vector2 Left
        {
            get
            {
                return ToAbsolutePosition(new Vector2(-1, 0));
            }
        }

        public Vector2 Right
        {
            get
            {
                return ToAbsolutePosition(new Vector2(1, 0));
            }
        }

        public void ToLocalPosition(ref Vector2 absolute, out Vector2 local)
        {
            var matrix = InvertAbsoluteMatrix;
            Vector2.Transform(ref absolute, ref matrix, out local);
        }

        public void ToAbsolutePosition(ref Vector2 local, out Vector2 absolute)
        {
            var matrix = AbsoluteMatrix;
            Vector2.Transform(ref local, ref matrix, out absolute);
        }

        public Vector2 ToLocalPosition(Vector2 absolute)
        {
            Vector2 result;
            ToLocalPosition(ref absolute, out result);
            return result;
        }

        public Vector2 ToAbsolutePosition(Vector2 local)
        {
            Vector2 result;
            ToAbsolutePosition(ref local, out result);
            return result;
        }

        private void SetLocalDirty()
        {
            isLocalDirty = true;
            SetAbsoluteDirty();
        }

        private void SetAbsoluteDirty()
        {
            isAbsoluteDirty = true;

            foreach (var child in children)
            {
                child.SetAbsoluteDirty();
            }
        }

        private void UpdateLocal()
        {
            if (!isLocalDirty)
            {
                return;
            }
            else
            {
                isLocalDirty = false;
            }

            var result = Matrix.CreateScale(localScale.X, localScale.Y, 1);
            result *= Matrix.CreateRotationZ(localRotation);
            result *= Matrix.CreateTranslation(localPosition.X, localPosition.Y, 0);
            localMatrix = result;
        }

        private void UpdateAbsolute()
        {
            if (!isAbsoluteDirty)
            {
                return;
            }
            else
            {
                isAbsoluteDirty = false;
            }

            if (Parent == null)
            {
                absoluteMatrix = localMatrix;
                absoluteScale = localScale;
                absoluteRotation = localRotation;
                absolutePosition = localPosition;
            }
            else
            {
                var parentAbsolute = Parent.AbsoluteMatrix;
                Matrix.Multiply(ref localMatrix, ref parentAbsolute, out absoluteMatrix);
                absoluteScale = Parent.Scale * LocalScale;
                absoluteRotation = Parent.Rotation + LocalRotation;
                absolutePosition = Vector2.Zero;
                ToAbsolutePosition(ref absolutePosition, out absolutePosition);
            }

            Matrix.Invert(ref absoluteMatrix, out invertAbsoluteMatrix);
        }
    }
}