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
    public class ImGuiManager : IFinalRender
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
            io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;
            io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;
        }

        public void Draw(RenderContext context, RenderingData renderingData)
        {
            renderer.BeginLayout(context.GameTime);

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

                ImGuiUtils.DockSpace();
            }
            else
            {
                context.DrawTexture(renderingData.Source);
            }

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
                    editorWindows[i].DrawGUI();
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
