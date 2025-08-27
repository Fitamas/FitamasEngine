using ImGuiNET;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.ImGuiNet
{
    public static class ImGuiThemes
    {
        public static void SetColor(this ImGuiStylePtr style, ImGuiCol idx, Color color)
        {
            style.Colors[(int)idx] = color.ToVector4().ToNumerics();
        }

        public static void DefaultDarkTheme()
        {
            ImGui.StyleColorsDark();
        }

        public static void DefaultLightTheme()
        {
            ImGui.StyleColorsLight();
        }

        public static void DefaultClassic()
        {
            ImGui.StyleColorsClassic();
        }

        public static void DarkHighContrastTheme()
        {
            var style = ImGui.GetStyle();

            style.SetColor(ImGuiCol.Text, new Color(0.80f, 0.80f, 0.83f, 1.00f));
            style.SetColor(ImGuiCol.TextDisabled, new Color(0.24f, 0.23f, 0.29f, 1.00f));
            style.SetColor(ImGuiCol.WindowBg, new Color(0.06f, 0.05f, 0.07f, 1.00f));
            style.SetColor(ImGuiCol.ChildBg, new Color(0.07f, 0.07f, 0.09f, 1.00f));
            style.SetColor(ImGuiCol.PopupBg, new Color(0.07f, 0.07f, 0.09f, 1.00f));
            style.SetColor(ImGuiCol.Border, new Color(0.80f, 0.80f, 0.83f, 0.88f));
            style.SetColor(ImGuiCol.BorderShadow, new Color(0.92f, 0.91f, 0.88f, 0.00f));
            style.SetColor(ImGuiCol.FrameBg, new Color(0.10f, 0.09f, 0.12f, 1.00f));
            style.SetColor(ImGuiCol.FrameBgHovered, new Color(0.24f, 0.23f, 0.29f, 1.00f));
            style.SetColor(ImGuiCol.FrameBgActive, new Color(0.56f, 0.56f, 0.58f, 1.00f));
            style.SetColor(ImGuiCol.TitleBg, new Color(0.10f, 0.09f, 0.12f, 1.00f));
            style.SetColor(ImGuiCol.TitleBgCollapsed, new Color(1.00f, 0.98f, 0.95f, 0.75f));
            style.SetColor(ImGuiCol.TitleBgActive, new Color(0.07f, 0.07f, 0.09f, 1.00f));
            style.SetColor(ImGuiCol.MenuBarBg, new Color(0.10f, 0.09f, 0.12f, 1.00f));
            style.SetColor(ImGuiCol.ScrollbarBg, new Color(0.10f, 0.09f, 0.12f, 1.00f));
            style.SetColor(ImGuiCol.ScrollbarGrab, new Color(0.80f, 0.80f, 0.83f, 0.31f));
            style.SetColor(ImGuiCol.ScrollbarGrabHovered, new Color(0.56f, 0.56f, 0.58f, 1.00f));
            style.SetColor(ImGuiCol.ScrollbarGrabActive, new Color(0.06f, 0.05f, 0.07f, 1.00f));
            style.SetColor(ImGuiCol.CheckMark, new Color(0.80f, 0.80f, 0.83f, 0.31f));
            style.SetColor(ImGuiCol.SliderGrab, new Color(0.80f, 0.80f, 0.83f, 0.31f));
            style.SetColor(ImGuiCol.SliderGrabActive, new Color(0.06f, 0.05f, 0.07f, 1.00f));
            style.SetColor(ImGuiCol.Button, new Color(0.10f, 0.09f, 0.12f, 1.00f));
            style.SetColor(ImGuiCol.ButtonHovered, new Color(0.24f, 0.23f, 0.29f, 1.00f));
            style.SetColor(ImGuiCol.ButtonActive, new Color(0.56f, 0.56f, 0.58f, 1.00f));
            style.SetColor(ImGuiCol.Header, new Color(0.10f, 0.09f, 0.12f, 1.00f));
            style.SetColor(ImGuiCol.HeaderHovered, new Color(0.56f, 0.56f, 0.58f, 1.00f));
            style.SetColor(ImGuiCol.HeaderActive, new Color(0.06f, 0.05f, 0.07f, 1.00f));
            style.SetColor(ImGuiCol.ResizeGrip, new Color(0.00f, 0.00f, 0.00f, 0.00f));
            style.SetColor(ImGuiCol.ResizeGripHovered, new Color(0.56f, 0.56f, 0.58f, 1.00f));
            style.SetColor(ImGuiCol.ResizeGripActive, new Color(0.06f, 0.05f, 0.07f, 1.00f));
            style.SetColor(ImGuiCol.PlotLines, new Color(0.40f, 0.39f, 0.38f, 0.63f));
            style.SetColor(ImGuiCol.PlotLinesHovered, new Color(0.25f, 1.00f, 0.00f, 1.00f));
            style.SetColor(ImGuiCol.PlotHistogram, new Color(0.40f, 0.39f, 0.38f, 0.63f));
            style.SetColor(ImGuiCol.PlotHistogramHovered, new Color(0.25f, 1.00f, 0.00f, 1.00f));
            style.SetColor(ImGuiCol.TextSelectedBg, new Color(0.25f, 1.00f, 0.00f, 0.43f));
            style.SetColor(ImGuiCol.ModalWindowDimBg, new Color(1.00f, 0.98f, 0.95f, 0.73f));
        }

        public static void DarkTheme()
        {
            var style = ImGui.GetStyle();

            style.SetColor(ImGuiCol.Text, new Color(1.00f, 1.00f, 1.00f, 0.95f));
            style.SetColor(ImGuiCol.TextDisabled, new Color(0.50f, 0.50f, 0.50f, 1.00f));
            style.SetColor(ImGuiCol.WindowBg, new Color(0.13f, 0.12f, 0.12f, 1.00f));
            style.SetColor(ImGuiCol.ChildBg, new Color(1.00f, 1.00f, 1.00f, 0.00f));
            style.SetColor(ImGuiCol.PopupBg, new Color(0.05f, 0.05f, 0.05f, 0.94f));
            style.SetColor(ImGuiCol.Border, new Color(0.53f, 0.53f, 0.53f, 0.46f));
            style.SetColor(ImGuiCol.BorderShadow, new Color(0.00f, 0.00f, 0.00f, 0.00f));
            style.SetColor(ImGuiCol.FrameBg, new Color(0.00f, 0.00f, 0.00f, 0.85f));
            style.SetColor(ImGuiCol.FrameBgHovered, new Color(0.22f, 0.22f, 0.22f, 0.40f));
            style.SetColor(ImGuiCol.FrameBgActive, new Color(0.16f, 0.16f, 0.16f, 0.53f));
            style.SetColor(ImGuiCol.TitleBg, new Color(0.00f, 0.00f, 0.00f, 1.00f));
            style.SetColor(ImGuiCol.TitleBgActive, new Color(0.00f, 0.00f, 0.00f, 1.00f));
            style.SetColor(ImGuiCol.TitleBgCollapsed, new Color(0.00f, 0.00f, 0.00f, 0.51f));
            style.SetColor(ImGuiCol.MenuBarBg, new Color(0.12f, 0.12f, 0.12f, 1.00f));
            style.SetColor(ImGuiCol.ScrollbarBg, new Color(0.02f, 0.02f, 0.02f, 0.53f));
            style.SetColor(ImGuiCol.ScrollbarGrab, new Color(0.31f, 0.31f, 0.31f, 1.00f));
            style.SetColor(ImGuiCol.ScrollbarGrabHovered, new Color(0.41f, 0.41f, 0.41f, 1.00f));
            style.SetColor(ImGuiCol.ScrollbarGrabActive, new Color(0.48f, 0.48f, 0.48f, 1.00f));
            style.SetColor(ImGuiCol.CheckMark, new Color(0.79f, 0.79f, 0.79f, 1.00f));
            style.SetColor(ImGuiCol.SliderGrab, new Color(0.48f, 0.47f, 0.47f, 0.91f));
            style.SetColor(ImGuiCol.SliderGrabActive, new Color(0.56f, 0.55f, 0.55f, 0.62f));
            style.SetColor(ImGuiCol.Button, new Color(0.50f, 0.50f, 0.50f, 0.63f));
            style.SetColor(ImGuiCol.ButtonHovered, new Color(0.67f, 0.67f, 0.68f, 0.63f));
            style.SetColor(ImGuiCol.ButtonActive, new Color(0.26f, 0.26f, 0.26f, 0.63f));
            style.SetColor(ImGuiCol.Header, new Color(0.54f, 0.54f, 0.54f, 0.58f));
            style.SetColor(ImGuiCol.HeaderHovered, new Color(0.64f, 0.65f, 0.65f, 0.80f));
            style.SetColor(ImGuiCol.HeaderActive, new Color(0.25f, 0.25f, 0.25f, 0.80f));
            style.SetColor(ImGuiCol.Separator, new Color(0.58f, 0.58f, 0.58f, 0.50f));
            style.SetColor(ImGuiCol.SeparatorHovered, new Color(0.81f, 0.81f, 0.81f, 0.64f));
            style.SetColor(ImGuiCol.SeparatorActive, new Color(0.81f, 0.81f, 0.81f, 0.64f));
            style.SetColor(ImGuiCol.ResizeGrip, new Color(0.87f, 0.87f, 0.87f, 0.53f));
            style.SetColor(ImGuiCol.ResizeGripHovered, new Color(0.87f, 0.87f, 0.87f, 0.74f));
            style.SetColor(ImGuiCol.ResizeGripActive, new Color(0.87f, 0.87f, 0.87f, 0.74f));
            style.SetColor(ImGuiCol.Tab, new Color(0.01f, 0.01f, 0.01f, 0.86f));
            style.SetColor(ImGuiCol.TabHovered, new Color(0.29f, 0.29f, 0.29f, 1.00f));
            style.SetColor(ImGuiCol.PlotLines, new Color(0.61f, 0.61f, 0.61f, 1.00f));
            style.SetColor(ImGuiCol.PlotLinesHovered, new Color(0.68f, 0.68f, 0.68f, 1.00f));
            style.SetColor(ImGuiCol.PlotHistogram, new Color(0.90f, 0.77f, 0.33f, 1.00f));
            style.SetColor(ImGuiCol.PlotHistogramHovered, new Color(0.87f, 0.55f, 0.08f, 1.00f));
            style.SetColor(ImGuiCol.TextSelectedBg, new Color(0.47f, 0.60f, 0.76f, 0.47f));
            style.SetColor(ImGuiCol.DragDropTarget, new Color(0.58f, 0.58f, 0.58f, 0.90f));
            style.SetColor(ImGuiCol.NavWindowingHighlight, new Color(1.00f, 1.00f, 1.00f, 0.70f));
            style.SetColor(ImGuiCol.NavWindowingDimBg, new Color(0.80f, 0.80f, 0.80f, 0.20f));
            style.SetColor(ImGuiCol.ModalWindowDimBg, new Color(0.80f, 0.80f, 0.80f, 0.35f));
        }
    }
}
