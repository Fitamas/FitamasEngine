using Fitamas.Collections;
using Fitamas.Core;
using Fitamas.ECS;
using Fitamas.Graphics;
using Fitamas.Graphics.ViewportAdapters;
using Fitamas.ImGuiNet.Assets;
using Fitamas.ImGuiNet.Windows;
using Fitamas.Scenes;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Fitamas.ImGuiNet
{
    public class ImGuiManager : IFinalRender, IInitializable, Core.IUpdateable, IDisposable
    {
        private GraphicsDevice graphicsDevice;
        private ImGuiRenderer renderer;

        private Camera camera;
        private RenderTarget2D lastRenderTarget;
        private IntPtr renderTargetId = IntPtr.Zero;

        public bool ShowSeperateGameWindow;

        public static ImGuiManager Instance { get; private set; }
        public static object SelectObject { get; set; }
        public static object DragDropObject { get; set; }

        public Camera Camera => camera;
        public RenderTarget2D LastRenderTarget => lastRenderTarget;
        public IntPtr RenderTargetId => renderTargetId;

        public ImGuiManager(GameEngine game)
        {
            Instance = this;

            graphicsDevice = game.GraphicsDevice;

            camera = new Camera();
            camera.Color = Color.DimGray;
            Camera.Current = camera;

            renderer = new ImGuiRenderer(game);
            renderer.RebuildFontAtlas();

            ImGuiIOPtr io = ImGui.GetIO();
            io.ConfigFlags |= ImGuiConfigFlags.DockingEnable | ImGuiConfigFlags.NavEnableKeyboard;
            io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;
            io.WantCaptureKeyboard = true;

            EditorWindow.LoadData();
        }

        public void Initialize()
        {
            SceneEditor.Initialize();

            foreach (var window in EditorWindow.Windows)
            {
                window.OnOpenEditor();
            }
        }

        public void Draw(RenderContext context, RenderingData renderingData)
        {
            renderer.BeginLayout(context.GameTime);

            bool showSeperateGameWindowCache = ShowSeperateGameWindow;

            if (ImGuiUtils.IsKeyPressedWithModifier(ImGuiKey.ModCtrl, ImGuiKey.P))
            {
                ShowSeperateGameWindow = !ShowSeperateGameWindow;
            }

            if (ShowSeperateGameWindow)
            {
                if (lastRenderTarget != renderingData.Source)
                {
                    if (lastRenderTarget != null)
                    {
                        renderer.UnbindTexture(renderTargetId);
                    }

                    lastRenderTarget = renderingData.Source;
                    renderTargetId = renderer.BindTexture(renderingData.Source);
                }

                EditorWindow.DrawScene(context.GameTime);

                ImGuiMainMenuBar.Draw();

                ImGuiToolbar.Draw();

                ImGuiUtils.DockSpace();

                EditorWindow.Draw(context.GameTime);
            }
            else
            {
                graphicsDevice.SetRenderTarget(renderingData.Destination);
                context.DrawTexture(renderingData.Source);
            }

            if (showSeperateGameWindowCache != ShowSeperateGameWindow)
            {
                if (ShowSeperateGameWindow)
                {
                    foreach (var window in EditorWindow.Windows)
                    {
                        window.OnOpenEditor();
                    }
                }
                else
                {
                    foreach (var window in EditorWindow.Windows)
                    {
                        window.OnCloseEditor();
                    }
                }
            }

            graphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            graphicsDevice.SetRenderTarget(renderingData.Destination);
            renderer.EndLayout();
        }

        public void UnbindTexture(IntPtr textureId)
        {
            renderer.UnbindTexture(textureId);
        }

        public IntPtr BindTexture(Texture2D texture)
        {
            return renderer.BindTexture(texture);
        }

        public void Update(GameTime gameTime)
        {
            AssetSystem.Update();
        }

        public void Dispose()
        {

        }
    }
}
