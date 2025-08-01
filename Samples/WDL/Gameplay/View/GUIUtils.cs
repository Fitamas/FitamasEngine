﻿using Fitamas.UserInterface.Components.NodeEditor;
using Fitamas.UserInterface.Components;
using Fitamas.UserInterface.Themes;
using Fitamas.UserInterface;
using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Fitamas.Graphics;
using Fitamas.Audio;

namespace WDL.Gameplay.View
{
    public static class GUIUtils
    {
        public static GUINodeItem CreateNodeItemWithCheckBox(Point buttonSize, GUINodeItemAlignment alignment, GUIPinType type)
        {
            return CreateNodeItemWithCheckBox(ResourceDictionary.DefaultResources, buttonSize, alignment, type);
        }

        public static GUINodeItem CreateNodeItemWithCheckBox(ResourceDictionary dictionary, Point buttonSize, GUINodeItemAlignment alignment, GUIPinType type)
        {
            return CreateNodeItemWithCheckBox(dictionary[CommonResourceKeys.NodeEditorPinStyle] as GUIStyle, dictionary[GUIStyleHelpers.CheckBoxStyle1] as GUIStyle, buttonSize, alignment, type);
        }

        public static GUINodeItem CreateNodeItemWithCheckBox(GUIStyle pinStyle, GUIStyle checkBoxStyle, Point buttonSize, GUINodeItemAlignment alignment, GUIPinType type)
        {
            GUINodeItem nodeItem = new GUINodeItem();
            nodeItem.Alignment = alignment;

            GUIPin pin = GUINodeUtils.CreatePin(pinStyle, type);
            nodeItem.AddChild(pin);
            nodeItem.Pin = pin;

            GUICheckBox checkBox = GUI.CreateCheckBox(checkBoxStyle);
            checkBox.LocalSize = buttonSize;
            nodeItem.Content = checkBox;
            nodeItem.AddChild(checkBox);

            return nodeItem;
        }

        public static void CreateStyles(ResourceDictionary dictionary)
        {
            dictionary[GUIStyleHelpers.PumpkinTexture] = Sprite.Create("Pumpkin");
            dictionary[GUIStyleHelpers.PinOffTexture] = Sprite.Create("PinOff");
            dictionary[GUIStyleHelpers.PinOnTexture] = Sprite.Create("PinOn");
            dictionary[GUIStyleHelpers.OpenTreeNodeTexture] = Sprite.Create("OpenTreeNode");
            dictionary[GUIStyleHelpers.CloseTreeNodeTexture] = Sprite.Create("CloseTreeNode");
            dictionary[GUIStyleHelpers.EnableButtonTexture] = Sprite.Create("EnableButton");
            dictionary[GUIStyleHelpers.DisableButtonTexture] = Sprite.Create("DisableButton");
            dictionary[GUIStyleHelpers.FolderTexture] = Sprite.Create("Folder");
            dictionary[GUIStyleHelpers.CircuitTexture] = Sprite.Create("Circuit");

            dictionary[GUIStyleHelpers.ButtonHoverAudio] = AudioClip.LoadWav("Sound\\Fantasy_UI (1).wav");
            dictionary[GUIStyleHelpers.ButtonPressedAudio] = AudioClip.LoadWav("Sound\\Fantasy_UI (6).wav");

            dictionary[CommonResourceKeys.ButtonStyle] = GUIStyleHelpers.CreateButton(dictionary);
            dictionary[GUIStyleHelpers.CheckBoxStyle1] = GUIStyleHelpers.CreateCheckBox(dictionary);
            dictionary[CommonResourceKeys.NodeEditorPinStyle] = GUIStyleHelpers.CreatePin(dictionary);
            dictionary[CommonResourceKeys.TreeNodeStyle] = GUIStyleHelpers.CreateTreeNode(dictionary);

            CreateWireStyle(dictionary, 0);
            CreateWireStyle(dictionary, 1);
            CreateWireStyle(dictionary, 2);

            CreateNodeStyle(dictionary, 0);
            CreateNodeStyle(dictionary, 1);
            CreateNodeStyle(dictionary, 2);
        }

        public static GUIStyle GetWireStyle(ResourceDictionary dictionary, int index)
        {
            return dictionary[CommonResourceKeys.NodeEditorWireStyle + index] as GUIStyle;
        }

        public static void CreateWireStyle(ResourceDictionary dictionary, int index)
        {
            dictionary[CommonResourceKeys.NodeEditorWireStyle + index] = GUIStyleHelpers.CreateWire(dictionary, index);
        }

        public static GUIStyle GetNodeStyle(ResourceDictionary dictionary, int index)
        {
            return dictionary[CommonResourceKeys.NodeEditorNodeStyle + index] as GUIStyle;
        }

        public static void CreateNodeStyle(ResourceDictionary dictionary, int index)
        {
            dictionary[CommonResourceKeys.NodeEditorNodeStyle + index] = GUIStyleHelpers.CreateNode(dictionary, index);
        }
    }
}
