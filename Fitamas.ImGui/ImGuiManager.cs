using Fitamas.Core;
using Fitamas.Graphics;
using Fitamas.ImGuiNet.Windows;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Fitamas.ImGuiNet
{
    public class ImGuiManager : IFinalRender, IInitializable
    {
        private GraphicsDevice graphicsDevice;
        private ImGuiRenderer renderer;
        private List<EditorWindow> editorWindows;
        private int windowCount;

        private RenderTarget2D lastRenderTarget;
        private IntPtr renderTargetId = IntPtr.Zero;

        public bool ShowSeperateGameWindow;

        public static object SelectObject { get; set; }
        public static object DragDropObject { get; set; }

        public GameEngine Game { get; }

        public RenderTarget2D LastRenderTarget => lastRenderTarget;
        public IntPtr RenderTargetId => renderTargetId;

        public ImGuiManager(GameEngine game)
        {
            Game = game;
            graphicsDevice = game.GraphicsDevice;

            editorWindows = new List<EditorWindow>();
            windowCount = 0;

            renderer = new ImGuiRenderer(game);
            renderer.RebuildFontAtlas();

            ImGuiIOPtr io = ImGui.GetIO();
            io.ConfigFlags |= ImGuiConfigFlags.DockingEnable | ImGuiConfigFlags.NavEnableKeyboard;
            io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;
            io.WantCaptureKeyboard = true;
        }

        public void Initialize()
        {
            foreach (var window in editorWindows)
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

                ImGuiMainMenuBar.MainMenuBar(this);

                ImGuiToolbar.Toolbar();

                ImGuiUtils.DockSpace();

                for (int i = editorWindows.Count - 1; i >= 0; i--)
                {
                    if (!editorWindows[i].Enable)
                    {
                        EditorWindow window = editorWindows[i];
                        editorWindows.RemoveAt(i);
                        window.Close();
                    }
                    else
                    {
                        editorWindows[i].DrawGUI(context.GameTime);
                    }
                }
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
                    foreach (var window in editorWindows)
                    {
                        window.OnOpenEditor();
                    }
                }
                else
                {
                    foreach (var window in editorWindows)
                    {
                        window.OnCloseEditor();
                    }
                }
            }


            graphicsDevice.SetRenderTarget(renderingData.Destination);
            renderer.EndLayout();
        }

        public void OpenWindow(EditorWindow window)
        {
            editorWindows.Add(window);
            window.Initialize(this, windowCount);
            windowCount++;
        }

        public void CloseWindow(EditorWindow window)
        {
            editorWindows.Remove(window);
            window.Close();
        }

        public void UnbindTexture(IntPtr textureId)
        {
            renderer.UnbindTexture(textureId);
        }

        public IntPtr BindTexture(Texture2D texture)
        {
            return renderer.BindTexture(texture);
        }
    }
}
