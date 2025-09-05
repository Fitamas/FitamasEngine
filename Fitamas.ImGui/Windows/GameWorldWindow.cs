using Fitamas.DebugTools;
using Fitamas.Graphics;
using Fitamas.Graphics.ViewportAdapters;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Fitamas.ImGuiNet.Windows
{
    public class GameWorldWindow : EditorWindow
    {
        //    private GraphicsDevice graphicsDevice;
        //    private ScalingWindowViewportAdapter adapter;
        //    private bool fixedScreen;
        //    private Vector2 contentSize;

        //    public GameWorldWindow()
        //    {
        //        Name = "GameWorld";
        //    }

        //    protected override void OnOpen()
        //    {
        //        graphicsDevice = manager.Game.GraphicsDevice;
        //        adapter = new ScalingWindowViewportAdapter(manager.Game.GraphicsDevice);
        //        manager.Game.ViewportAdapterProperty.Value = adapter;
        //    }

        //    protected override void OnFocus()
        //    {
        //        Camera.Current = Camera.Main;
        //    }

        //    protected override void OnGUI()
        //    {
        //        if (ImGui.Button("Fixed Screen"))
        //        {
        //            fixedScreen = !fixedScreen;

        //            if (fixedScreen)
        //            {
        //                contentSize = new Vector2(1920, 1080);
        //            }
        //        }

        //        ImGui.SameLine();

        //        ImGui.Text($"FPS {FramesPerSecondCounter.Instance.FramesPerSecond}");

        //        ImGui.BeginChild("GameRender");

        //        Vector2 size = ImGui.GetContentRegionAvail();
        //        Vector2 uv0 = Vector2.Zero;
        //        Vector2 uv1 = Vector2.One;

        //        if (!fixedScreen)
        //        {
        //            contentSize = size;
        //        }

        //        if (fixedScreen)
        //        {
        //            Vector2 scale = contentSize;

        //            if (size.X / size.Y > scale.X / scale.Y)
        //            {
        //                size.X = size.Y * scale.X / scale.Y;
        //            }
        //            else
        //            {
        //                size.Y = size.X * scale.Y / scale.X;
        //            }
        //        }

        //        Vector2 windowPos = ImGui.GetWindowPos();
        //        adapter.ScreenPosition = windowPos.ToPoint();
        //        adapter.ViewportSize = size.ToPoint();
        //        adapter.VirtualSize = contentSize.ToPoint();

        //        manager.Game.InputManager.IsActive = focusedWindow;

        //        if (focusedWindow)
        //        {
        //            ImGuiUtils.Image(manager.RenderTargetId, size, uv0, uv1, Color.White, new Color());
        //        }

        //        ImGui.EndChild();
        //    }
    }
}
