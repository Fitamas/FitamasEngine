using Fitamas.ECS;
using Fitamas.Math;
using Fitamas.Graphics.ViewportAdapters;
using Microsoft.Xna.Framework;
using System;
using Fitamas.Core;

namespace Fitamas.Graphics
{
    public class Camera : Component, IDisposable
    {
        public static Camera Main { get; set; }
        public static Camera Current { get; set; }

        public Vector2 Position { get; set; }
        public float Rotation { get; set; }

        public float Zoom;
        public Color Color;

        public ViewportAdapter ViewportAdapter { get; set; }

        public Vector2 Origin
        {
            get
            {
                return VirtualSize / 2f;
            }
        }

        public Vector2 ViewportSize
        {
            get
            {
                return new Vector2(ViewportAdapter.ViewportWidth, ViewportAdapter.ViewportHeight);
            }
        }

        public Vector2 VirtualSize
        {
            get
            {
                return new Vector2(ViewportAdapter.VirtualWidth, ViewportAdapter.VirtualHeight);
            }
        }

        public Matrix TranslationVirtualMatrix
        {
            get
            {
                return GetVirtualViewMatrix();
            }
        }

        public Matrix ProjectionMatrix
        {
            get
            {
                return GetProjectionMatrix(TranslationVirtualMatrix);
            }
        }

        public Matrix ViewportScaleMatrix
        {
            get
            {
                return ViewportAdapter.GetScaleMatrix();
            }
        }

        public Camera()
        {
            Color = Color.CornflowerBlue;
            Zoom = 100;
        }

        public void Dispose()
        {
            ViewportAdapter?.Dispose();
        }

        private Matrix GetVirtualViewMatrix()
        {
            return
                Matrix.CreateTranslation(new Vector3(-Position, 0.0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, -Zoom, 1) *
                Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }

        public Matrix GetProjectionMatrix()
        {
            Vector2 viewportSize = ViewportSize;
            return Matrix.CreateOrthographicOffCenter(0, viewportSize.X, viewportSize.Y, 0, -1, 0);
        }

        public Matrix GetProjectionMatrix(Matrix viewMatrix)
        {
            var projection = GetProjectionMatrix();
            Matrix.Multiply(ref viewMatrix, ref projection, out Matrix result);
            return result;
        }

        public Matrix GetViewMatrix()
        {
            return GetVirtualViewMatrix() * ViewportScaleMatrix;
        }

        public void AdjustZoom(float amount)
        {
            Zoom += amount;
            if (Zoom < 0.25f)
            {
                Zoom = 0.25f;
            }
        }

        public void MoveCamera(Vector2 delta)
        {
            Position += delta;
        }

        public void LookAt(Vector2 pos)
        {
            Position = pos;
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, GetViewMatrix());
        }

        public Vector2 ScreenToWorld(Point screenPosition)
        {
            return ScreenToWorld(new Vector2(screenPosition.X, screenPosition.Y));
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(GetVirtualViewMatrix()));
        }

        public RectangleF WorldBounds()
        {
            Vector2 size = VirtualSize / Zoom;
            return new RectangleF(Position - size / 2, size);
        }
    }
}
