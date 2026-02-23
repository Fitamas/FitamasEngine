using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitamas.ImGuiNet
{
    public static class ImGuiToolbar
    {
        public const float toolbarSize = 60;
        public const float toolbarButtonSize = 35;

        public static void Draw()
        {
            BeginToolbar();

            //if (ImGuiUtils.ToolbarButton(ImGuiStyle.PlayTexture))
            //{
            //    RuntimeMode = true;
            //}
            //if (RuntimeMode)
            //{
            //    if (ImGuiUtils.ToolbarButton(ImGuiStyle.PauseTexture))
            //    {

            //    }

            //    if (ImGuiUtils.ToolbarButton(ImGuiStyle.StopTexture))
            //    {
            //        RuntimeMode = false;
            //    }
            //}



            EndToolbar();
        }

        public static void BeginToolbar()
        {
            ImGuiViewportPtr viewport = ImGui.GetMainViewport();

            float menuBarHeight = viewport.Size.Y - viewport.WorkSize.Y;

            ImGui.SetNextWindowPos(new System.Numerics.Vector2(viewport.Pos.X, viewport.Pos.Y + menuBarHeight));
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(viewport.Size.X, toolbarSize));
            ImGui.SetNextWindowViewport(viewport.ID);
            ImGuiWindowFlags window_flags = 0
                | ImGuiWindowFlags.NoDocking
                | ImGuiWindowFlags.NoTitleBar
                | ImGuiWindowFlags.NoResize
                | ImGuiWindowFlags.NoMove
                | ImGuiWindowFlags.NoScrollbar
                | ImGuiWindowFlags.NoSavedSettings
                ;
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0);
            ImGui.Begin("TOOLBAR", window_flags);
            ImGui.PopStyleVar();
        }

        public static void EndToolbar()
        {
            ImGui.End();
        }

        public static bool ToolbarButton(nint textureId, Vector2? size = default)
        {
            ImGui.SameLine();
            ImGui.PushID("ToolbarButton");
            System.Numerics.Vector2 imageSize = size.HasValue ? size.Value.ToNumerics()
                                                              : new System.Numerics.Vector2(toolbarButtonSize);
            return ImGui.ImageButton("ToolbarButton", textureId, imageSize);
        }
    }
}
