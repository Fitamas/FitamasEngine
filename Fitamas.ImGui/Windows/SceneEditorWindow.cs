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
        private nint idTexture;

        private Vector2 contentSize;
        private Vector2 mouseOnsScreen;

        public float CameraSpeed = 20;

        //public Vector2 RenderTargetScale
        //{
        //    get
        //    {
        //        return new Vector2(renderTarget.Width, renderTarget.Height);
        //    }
        //}

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

        protected override void OnGUI()
        {
            ImGui.BeginChild("GameRender");

            Vector2 mousePos = ImGui.GetMousePos();
            mouseOnsScreen = adapter.PointToScreen(mousePos.ToPoint()).ToVector2();

            Vector2 size = ImGui.GetContentRegionAvail();
            Vector2 uv0 = Vector2.Zero;
            Vector2 uv1 = Vector2.One;

            if (contentSize != size)
            {
                contentSize = size;
                //ResizeWindow((int)contentSize.X, (int)contentSize.Y);
            }

            if (ImGui.IsWindowHovered(ImGuiHoveredFlags.RootAndChildWindows |
                                      ImGuiHoveredFlags.AllowWhenBlockedByActiveItem))
            {
                UpdateInput();
            }

            Vector2 windowPos = ImGui.GetWindowPos();
            adapter.ScreenPosition = windowPos.ToPoint();
            adapter.ViewportSize = size.ToPoint();
            adapter.VirtualSize = contentSize.ToPoint();

            Camera.Current = camera;
            //graphicsDevice.SetRenderTarget(renderTarget);
            //if (EditorSystem.RuntimeMode)
            //{
            //    editor.GameWorld.Draw(gameTime);
            //}
            //else
            //{
            //    RenderScene(EditorSystem.OpenScene);
            //}
            //DebugGizmo.BeginBatch(graphicsDevice);
            //editor.DrawScene(gameTime);
            //DebugGizmo.EndBatch();
            //graphicsDevice.SetRenderTarget(null);

            ImGuiUtils.Image(idTexture, size, uv0, uv1, Color.White, new Color());

            ImGui.EndChild();
        }

        //private void RenderScene(SerializebleScene scene)
        //{
        //    RenderBatch.Begin(graphicsDevice, camera);
        //    foreach (var gameObject in scene.GameObjects)
        //    {
        //        if (gameObject.TryGetComponent(out Transform transform))
        //        {
        //            if (gameObject.TryGetComponent(out SpriteRender sprite))
        //            {
        //                RenderBatch.Render(transform, sprite);
        //            }
        //            if (gameObject.TryGetComponent(out MeshRender meshRender) &&
        //                gameObject.TryGetComponent(out Mesh mesh))
        //            {
        //                RenderBatch.Render(transform, meshRender, mesh);
        //            }
        //        }
        //    }
        //    RenderBatch.End();
        //    DebugGizmo.BeginBatch(graphicsDevice);
        //    foreach (var gameObject in EditorSystem.OpenScene.GameObjects)
        //    {
        //        if (gameObject.TryGetComponent(out Transform transform))
        //        {
        //            DebugGizmo.DrawAnchor(transform);

        //            if (gameObject.TryGetComponent(out SpriteRender sprite))
        //            {
        //                DebugGizmo.DrawSprite(transform, sprite);
        //            }
        //            if (gameObject.TryGetComponent(out Mesh mesh))
        //            {
        //                DebugGizmo.DrawRenderMesh(transform, mesh);
        //            }
        //            if (gameObject.TryGetComponent(out Collider collider))
        //            {
        //                DebugGizmo.DrawCollider(transform, collider);
        //            }
        //        }
        //    }
        //    DebugGizmo.EndBatch();
        //}

        private void UpdateInput()
        {
            Vector2 mousePosition = camera.ScreenToWorld(mouseOnsScreen);

            if (ImGui.IsMouseClicked(ImGuiMouseButton.Left))
            {
                if (!SelectEntityOnMouse(mousePosition))
                {
                    //EditorSystem.SelectObject = null;
                }
            }

            if (ImGui.IsMouseDragging(ImGuiMouseButton.Middle))
            {
                Vector2 dragDelta = ImGui.GetMouseDragDelta(ImGuiMouseButton.Middle);
                ImGui.ResetMouseDragDelta(ImGuiMouseButton.Middle);

                Vector2 delta = mousePosition - camera.ScreenToWorld(mouseOnsScreen + dragDelta);

                camera.Position += delta;
            }
            else
            {
                //float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                //Vector2 direction = Vector2.Zero;

                //if (ImGui.IsKeyPressed(ImGuiKey.A)) direction -= new Vector2(1, 0);
                //if (ImGui.IsKeyPressed(ImGuiKey.D)) direction += new Vector2(1, 0);
                //if (ImGui.IsKeyPressed(ImGuiKey.W)) direction += new Vector2(0, 1);
                //if (ImGui.IsKeyPressed(ImGuiKey.S)) direction -= new Vector2(0, 1);

                //Vector2 delta = direction.NormalizeF() * deltaTime * CameraSpeed;

                //camera.Position += delta;
            }

            if (ImGui.IsKeyDown(ImGuiKey.MouseWheelY))
            {
                float mouseDirection = ImGui.GetIO().MouseWheel;

                camera.AdjustZoom(mouseDirection);
            }
        }

        public void BeforeDraw()
        {
            //if (Camera.Main != null)
            //{
            //    DebugGizmo.DrawCamera(Camera.Main);
            //}

            //Vector2 mousePosition = camera.ScreenToWorld(mouseOnsScreen);

            //if (EditorSystem.SelectObject is Entity entity)
            //{
            //    if (entity.TryGet(out Transform transform))
            //    {
            //        transform.Position = HandleUtils.PositionHandle(transform.Position, transform.Rotation, mousePosition);
            //    }
            //}
            //else if (EditorSystem.SelectObject is GameObject gameObject)
            //{
            //    if (gameObject.TryGetComponent(out Transform transform))
            //    {
            //        transform.Position = HandleUtils.PositionHandle(transform.Position, transform.Rotation, mousePosition);
            //    }
            //}
        }

        private bool SelectEntityOnMouse(Vector2 mousePosition)
        {
            //foreach (var gameObject in EditorSystem.OpenScene.GameObjects)
            //{
            //    if (gameObject.TryGetComponent(out Transform transform))
            //    {
            //        if (EditorSystem.SelectObject == gameObject)
            //        {
            //            Handle handle = HandleUtils.GetHandle(transform.Position, transform.Rotation, mousePosition, out Vector2 localMousePosition);
            //            if (handle != Handle.none)
            //            {
            //                return true;
            //            }
            //        }

            //        if (gameObject.TryGetComponent(out SpriteRender sprite))
            //        {
            //            Vector2 local = transform.ToLocalPosition(mousePosition);
            //            if (sprite.GetSpriteRectangle().Contains(local))
            //            {
            //                EditorSystem.SelectObject = gameObject;
            //                return true;
            //            }
            //        }

            //        if (gameObject.TryGetComponent(out Mesh mesh))
            //        {
            //            Vector2 local = transform.ToLocalPosition(mousePosition);

            //            //TODO
            //        }
            //    }
            //}

            //foreach (var gameObject in EditorSystem.OpenScene.GameObjects)
            //{
            //    if (gameObject.TryGetComponent(out Transform transform))
            //    {
            //        if (EditorSystem.SelectObject is Entity res && res == entity)
            //        {
            //            Handle handle = HandleUtils.GetHandle(transform.Position, transform.Rotation, mousePosition, out Vector2 localMousePosition);
            //            if (handle != Handle.none)
            //            {
            //                return true;
            //            }
            //        }

            //        if (entity.TryGet(out SpriteRender sprite))
            //        {
            //            Vector2 local = transform.ToLocalPosition(mousePosition);
            //            if (sprite.GetSpriteRectangle().Contains(local))
            //            {
            //                EditorSystem.SelectObject = entity;
            //                return true;
            //            }
            //        }

            //        if (entity.TryGet(out Mesh mesh))
            //        {
            //            Vector2 local = transform.ToLocalPosition(mousePosition);

            //            //TODO
            //        }
            //    }
            //}

            return false;
        }

        //private void ResizeWindow(int width, int hight)
        //{
        //    //if (renderTarget != null)
        //    //{
        //    //    editor.Renderer.UnbindTexture(idTexture);
        //    //}

        //    //if (adapter == null)
        //    //{
        //    //    adapter = new ScalingWindowViewportAdapter(graphicsDevice);
        //    //}

        //    //PresentationParameters parameters = graphicsDevice.PresentationParameters;

        //    //renderTarget = new RenderTarget2D(graphicsDevice, width, hight, false,
        //    //                                  parameters.BackBufferFormat, parameters.DepthStencilFormat);

        //    //idTexture = editor.Renderer.BindTexture(renderTarget);
        //}
    }
}
