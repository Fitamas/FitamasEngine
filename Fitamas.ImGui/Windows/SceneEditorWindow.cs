using Fitamas.DebugTools;
using Fitamas.ECS;
using Fitamas.Graphics;
using Fitamas.Graphics.ViewportAdapters;
using Fitamas.Math;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Fitamas.ImGuiNet.Windows
{
    public class SceneEditorWindow : EditorWindow
    {
        private GraphicsDevice graphicsDevice;
        private ScalingWindowViewportAdapter adapter;
        private Camera camera;
        //private RenderTarget2D renderTarget;
        //private nint idTexture;

        private Vector2 contentSize;
        private Vector2 mouseOnsScreen;
        private Vector2 mousePosition;

        public float CameraSpeed = 10;

        public SceneEditorWindow()
        {
            Name = "Scene";
        }

        protected override void OnOpen()
        {
            graphicsDevice = manager.Game.GraphicsDevice;
            adapter = new ScalingWindowViewportAdapter(graphicsDevice);
            camera = new Camera();
            camera.Color = Color.DimGray;
            camera.ViewportAdapter = adapter;
        }

        protected override void OnFocus()
        {
            Camera.Current = camera; //TODO fix
        }

        public override void OnOpenEditor()
        {
            Camera.Current = camera;
            manager.Game.InputManager.IsActive = false;
        }

        public override void OnCloseEditor()
        {
            Camera.Current = Camera.Main;
            manager.Game.InputManager.IsActive = true;
        }

        protected override void OnGUI(GameTime gameTime)
        {
            ImGui.BeginChild("GameRender");

            Vector2 mousePos = ImGui.GetMousePos();
            mouseOnsScreen = adapter.PointToScreen(mousePos.ToPoint()).ToVector2();
            mousePosition = camera.ScreenToWorld(mouseOnsScreen);

            Vector2 size = ImGui.GetContentRegionAvail();
            Vector2 uv0 = Vector2.Zero;
            Vector2 uv1 = Vector2.One;

            if (contentSize != size)
            {
                contentSize = size;
                //ResizeWindow((int)contentSize.X, (int)contentSize.Y);
            }

            if (focusedWindow)
            {
                UpdateInput(gameTime);
            }

            Vector2 windowPos = ImGui.GetWindowPos();
            adapter.ScreenPosition = windowPos.ToPoint();
            adapter.ViewportSize = size.ToPoint();
            adapter.VirtualSize = contentSize.ToPoint();

            Gizmos.Begin();
            RenderScene();
            Gizmos.End();

            ImGuiUtils.Image(manager.RenderTargetId, size, uv0, uv1, Color.White, new Color());

            ImGui.EndChild();
        }

        private void RenderScene()
        {
            if (ImGuiManager.SelectObject is Entity entity)
            {
                if (entity.TryGet(out Transform transform))
                {
                    transform.Position = HandleUtils.PositionHandle(transform.Position, transform.Rotation, mousePosition);
                }
            }

            if (ImGui.IsMouseClicked(ImGuiMouseButton.Left) && HandleUtils.Handle == Handle.none)
            {
                if (!TrySelectEntityOnMouse(mousePosition, out Entity selectEntity))
                {
                    ImGuiManager.SelectObject = selectEntity;
                }
                else
                {
                    ImGuiManager.SelectObject = null;
                }
            }
        }

        private void UpdateInput(GameTime gameTime)
        {
            if (ImGui.IsMouseDragging(ImGuiMouseButton.Middle))
            {
                Vector2 dragDelta = ImGui.GetMouseDragDelta(ImGuiMouseButton.Middle);
                ImGui.ResetMouseDragDelta(ImGuiMouseButton.Middle);

                Vector2 delta = mousePosition - camera.ScreenToWorld(mouseOnsScreen + dragDelta);

                camera.Position += delta;
            }
            else if (!ImGui.IsKeyDown(ImGuiKey.ModCtrl))
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Vector2 direction = Vector2.Zero;

                if (ImGui.IsKeyDown(ImGuiKey.A)) direction -= new Vector2(1, 0);
                if (ImGui.IsKeyDown(ImGuiKey.D)) direction += new Vector2(1, 0);
                if (ImGui.IsKeyDown(ImGuiKey.W)) direction += new Vector2(0, 1);
                if (ImGui.IsKeyDown(ImGuiKey.S)) direction -= new Vector2(0, 1);

                Vector2 delta = direction.NormalizeF() * deltaTime * CameraSpeed;

                camera.Position += delta;
            }

            if (mouseOverWindow && ImGui.IsKeyDown(ImGuiKey.MouseWheelY))
            {
                float mouseDirection = ImGui.GetIO().MouseWheel;

                camera.AdjustZoom(mouseDirection);
            }
        }

        private bool TrySelectEntityOnMouse(Vector2 mousePosition, out Entity entity)
        {
            //TODO renderers from render manager
            entity = null;
            return false;
        }

        //private void ResizeWindow(int width, int hight)
        //{
        //    if (renderTarget != null)
        //    {
        //        manager.UnbindTexture(idTexture);
        //    }

        //    renderTarget = new RenderTarget2D(graphicsDevice, width, hight);

        //    idTexture = manager.BindTexture(renderTarget);
        //}
    }
}
