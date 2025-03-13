using Fitamas.Entities;
using Fitamas.Math2D;
using Fitamas.Graphics.ViewportAdapters;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Graphics
{
    public class Camera : IDisposable
    {
        public static Camera Main => CameraSystem.MainCamera;
        public static Camera Current { get; set; }

        private ViewportAdapter viewportAdapter;
        private Transform transform;
        private float zoom;

        public Transform Transform => transform;
        public CameraType CameraType { get; set; }
        public Color Color { get; set; }

        public ViewportAdapter ViewportAdapter
        {
            get
            {
                return viewportAdapter;
            }
            set
            {
                if (value != null && viewportAdapter != value)
                {
                    viewportAdapter?.Dispose();
                    viewportAdapter = value;
                }
            }
        }

        public Vector2 Position
        {
            get
            {
                return transform.Position;
            }
            set
            {
                transform.Position = value;
            }
        }

        public float Zoom
        {
            get
            {
                return zoom;
            }
        }

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
                return new Vector2(ViewportAdapter.ViewportWidth, viewportAdapter.ViewportHeight);
            }
        }

        public Vector2 VirtualSize
        {
            get
            {
                return new Vector2(viewportAdapter.VirtualWidth, viewportAdapter.VirtualHeight);
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
                return viewportAdapter.GetScaleMatrix();
            }
        }

        public Camera(Game game, Vector2 position, float angle, float zoom)
        {
            viewportAdapter = new WindowViewportAdapter(game.Window, game.GraphicsDevice);

            Color = Color.CornflowerBlue;

            this.zoom = zoom;

            transform = new Transform();

            transform.LocalPosition = position;
            transform.LocalRotation = angle;
        }

        public Camera(Game game) : this(game, Vector2.Zero, 0, 100)
        {

        }

        public void Dispose()
        {
            viewportAdapter?.Dispose();
        }

        private Matrix GetVirtualViewMatrix()
        {
            return
                Matrix.CreateTranslation(new Vector3(-transform.Position, 0.0f)) *
                Matrix.CreateRotationZ(transform.Rotation) *
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
            zoom += amount;
            if (zoom < 0.25f)
            {
                zoom = 0.25f;
            }
        }

        public void MoveCamera(Vector2 delta)
        {
            transform.LocalPosition += delta;
        }

        public void LookAt(Vector2 pos)
        {
            transform.LocalPosition = pos;
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
            Vector2 size = VirtualSize / zoom;
            return new RectangleF(Position - size / 2, size);
        }
    }
}
