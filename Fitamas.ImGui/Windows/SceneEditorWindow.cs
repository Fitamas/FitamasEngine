using Fitamas.Core;
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
        //private GraphicsDevice graphicsDevice;
        private ScalingWindowViewportAdapter adapter;

        private Vector2 contentSize;
        private Vector2 mouseOnScreen;
        private Vector2 mousePosition;

        public float CameraSpeed = 10;

        public SceneEditorWindow() : base("Scene")
        {
            adapter = new ScalingWindowViewportAdapter(GameEngine.Instance.GraphicsDevice);
        }

        protected override void OnFocus()
        {
            ImGuiManager.Instance.Camera.ViewportAdapter = adapter;
            Camera.Current = ImGuiManager.Instance.Camera;
        }

        public override void OnOpenEditor()
        {
            GameEngine.Instance.InputManager.IsActive = false;
        }

        public override void OnCloseEditor()
        {
            GameEngine.Instance.InputManager.IsActive = true;
        }

        protected override void OnGUI(GameTime gameTime)
        {
            ImGui.BeginChild("GameRender");

            Vector2 mousePos = ImGui.GetMousePos();
            mouseOnScreen = adapter.PointToScreen(mousePos.ToPoint()).ToVector2();
            mousePosition = ImGuiManager.Instance.Camera.ScreenToWorld(mouseOnScreen);

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

            ImGuiUtils.Image(ImGuiManager.Instance.RenderTargetId, size, uv0, uv1, Color.White, new Color());

            ImGui.EndChild();
        }

        protected override void OnScene(GameTime gameTime)
        {
            if (ImGuiManager.SelectObject is Entity entity)
            {
                if (entity.TryGet(out Transform transform))
                {
                    transform.Position = HandleUtils.PositionHandle(transform.Position, transform.Rotation, mousePosition);
                }
            }

            if (!focusedWindow)
            {
                return;
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

                Vector2 delta = mousePosition - ImGuiManager.Instance.Camera.ScreenToWorld(mouseOnScreen + dragDelta);

                ImGuiManager.Instance.Camera.Position += delta;
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

                ImGuiManager.Instance.Camera.Position += delta;
            }

            if (mouseOverWindow && ImGui.IsKeyDown(ImGuiKey.MouseWheelY))
            {
                float mouseDirection = ImGui.GetIO().MouseWheel;

                ImGuiManager.Instance.Camera.AdjustZoom(mouseDirection);
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
