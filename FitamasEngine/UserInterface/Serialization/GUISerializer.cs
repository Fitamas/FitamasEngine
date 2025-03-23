using Fitamas.Graphics;
using Fitamas.Serialization;
using Fitamas.UserInterface.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Fitamas.UserInterface.Components.NodeEditor;
using Fitamas.Events;
using Fitamas.Serialization.Xml;
using Fitamas.UserInterface.ViewModel;

namespace Fitamas.UserInterface.Serialization
{
    public class GUISerializer
    {
        private Dictionary<string, Func<XElement, GUIComponent>> converters;
        private Dictionary<string, GUIComponent> dictionary;
        private XDocument document;

        public GUISerializer()
        {
            dictionary = new Dictionary<string, GUIComponent>();
            converters = new Dictionary<string, Func<XElement, GUIComponent>>
            {
                { "text", LoadGUITextBlock },
                { "image", LoadGUIImage },
                { "button", LoadGUIButton },
                { "frame", LoadGUIFrame },
                { "treenode", LoadGUITeeNode },
                { "nodeeditor",  LoadGUINodeEditor },
            };
        }

        public GUIWindowBinder Load(string path)
        {
            string fileName = Path.Combine(Resources.RootDirectory, path);

            document = XDocument.Load(fileName);

            string lua = fileName + ".lua";
            GUIWindow window = new GUIWindow();
            //scripting = new GUIScripting(window, lua);

            //List<GUIComponent> components = new List<GUIComponent>();

            //foreach (XElement el in document.Root.Elements())
            //{
            //    GUIComponent component = FromXML(el);
            //    //frame.AddChild(component);
            //}

            GUIWindowBinder binder = new GUIWindowBinder();
            //binder.Scripting = scripting;
            binder.Window = window;
            return binder;
        }

        public GUIComponent FromXML(XElement element)
        {
            string id = element.GetAttributeString("id");

            if (!string.IsNullOrEmpty(id) && dictionary.ContainsKey(id))
            {
                return dictionary[id];
            }

            GUIComponent component;

            foreach (var subElement in element.Elements())
            {
                if (subElement.Name.ToString().Equals("conditional", StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }
            }

            string type = element.Name.ToString().ToLowerInvariant();

            component = converters[type]?.Invoke(element);

            if (component == null)
            {
                throw new NotImplementedException("Loading GUI component \"" + element.Name + "\" from XML is not implemented.");
            }

            LoadGUIComponent(element, component);

            if (!string.IsNullOrEmpty(id))
            {
                dictionary[id] = component;
            }

            if (component != null)
            {
                foreach (var subElement in element.Elements())
                {
                    if (subElement.Name.ToString().Equals("conditional", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    GUIComponent child = FromXML(subElement);
                    component.AddChild(child);
                }
            }
            return component;
        }

        public GUIComponent FromId(string id)
        {
            var filter = document.Descendants()
                             .Where(e => (e.Attribute("id") != null) && e.Attribute("id").Value == id);

            if (filter.Count() == 0)
            {
                return null;
            }

            XElement element1 = filter.First();

            return FromXML(element1);
        }

        //public MonoAction CreateAction(string name)
        //{
        //    if (!string.IsNullOrEmpty(name) && lua != null)
        //    {
        //        LuaFunction function = lua[name] as LuaFunction;

        //        return () => { function?.Call(); };
        //    }

        //    return null;
        //}

        //public MonoAction<T0> CreateAction<T0>(string name)
        //{
        //    if (!string.IsNullOrEmpty(name) && lua != null)
        //    {
        //        LuaFunction function = lua[name] as LuaFunction;

        //        return (arg1) => { function?.Call(arg1); };
        //    }

        //    return null;
        //}

        //public MonoAction<T0, T1> CreateAction<T0, T1>(string name)
        //{
        //    if (!string.IsNullOrEmpty(name) && lua != null)
        //    {
        //        LuaFunction function = lua[name] as LuaFunction;

        //        return (arg0, arg1) => { function?.Call(arg0, arg1); };
        //    }

        //    return null;
        //}

        /*
             _____                           _                
            /  __ \                         | |               
            | /  \/ ___  _ ____   _____ _ __| |_ ___ _ __ ___ 
            | |    / _ \| '_ \ \ / / _ \ '__| __/ _ \ '__/ __|
            | \__/\ (_) | | | \ V /  __/ |  | ||  __/ |  \__ \
             \____/\___/|_| |_|\_/ \___|_|   \__\___|_|  |___/
         */

        private void LoadGUIComponent(XElement element, GUIComponent component)
        {
            if (element.Attribute("alignment") != null)
            {
                //component.Alignment = element.GetAttributeEnum<GUIAlignment>("alignment");
            }
            else
            {
                //component.Anchor = element.GetAttributeVector2("anchor");
            }

            //if (element.Attribute("layer") != null)
            //{
            //    component.AutoSortingLayer = false;
            //    component.Layer = element.GetAttributeInt("layer", component.Layer);
            //}

            //component.Stretch = element.GetAttributeEnum<GUIStretch>("stretch", component.Stretch);
            component.LocalPosition = element.GetAttributePoint("pos", component.LocalPosition);
            component.LocalSize = element.GetAttributePoint("size", component.LocalSize);
            component.Enable = element.GetAttributeBool("enable", component.Enable);
            component.Interacteble = element.GetAttributeBool("enterecteble", component.Interacteble);
            component.IsMask = element.GetAttributeBool("isMask", component.IsMask);
            component.Name = element.GetAttributeString("id", component.Name);
            component.Pivot = element.GetAttributeVector2("pivot", component.Pivot);
        }

        private GUITextBlock LoadGUITextBlock(XElement element)
        {
            string text = element.GetAttributeString("text");
            ////text = text.Replace(@"\n", "\n");
            GUITextBlock textBlock = GUI.CreateTextBlock(Point.Zero, text);

            textBlock.TextHorisontalAlignment = element.GetAttributeEnum<GUITextHorisontalAlignment>("textHorisontalAlignment");
            textBlock.Scale = element.GetAttributeFloat("scale", 1.0f);
            textBlock.AutoScale = element.GetAttributeBool("autoScale");

            return textBlock;
        }

        private GUIButton LoadGUIButton(XElement element)
        {
            GUIButton button = new GUIButton();

            string textBlockId = element.GetAttributeString("text");
            string imageId = element.GetAttributeString("image");

            //button.DefoultColor = element.GetAttributeColor("color", button.DefoultColor);
            //button.DefoultTextColor = element.GetAttributeColor("textColor", button.DefoultTextColor);
            //button.SelectColor = element.GetAttributeColor("selectColor", button.SelectColor);
            //button.SelectTextColor = element.GetAttributeColor("selectTextColor", button.SelectTextColor);

            string onClicked = element.GetAttributeString("onClicked");
            //button.OnClicked.AddListener(scripting.CreateAction<GUIButton>(onClicked));

            return button;
        }

        private GUIImage LoadGUIImage(XElement element)
        {
            GUIImage image = new GUIImage();

            image.Color = element.GetAttributeColor("color", image.Color);
            image.Sprite = element.GetAttributeMonoObject<Sprite>("sprite");

            return image;
        }

        private GUIFrame LoadGUIFrame(XElement element)
        {
            return new GUIFrame();
        }

        private GUITreeNode LoadGUITeeNode(XElement element)
        {
            GUITreeNode node = new GUITreeNode();

            string textBlockId = element.GetAttributeString("name");
            string imageId = element.GetAttributeString("header");
            string iconOpenId = element.GetAttributeString("iconOpened");
            string iconClosedId = element.GetAttributeString("iconClosed");
            string iconId = element.GetAttributeString("icon");

            //node.DefoultColor = element.GetAttributeColor("color", node.DefoultColor);
            //node.DefoultTextColor = element.GetAttributeColor("textColor", node.DefoultTextColor);
            //node.SelectColor = element.GetAttributeColor("selectColor", node.SelectColor);
            //node.SelectTextColor = element.GetAttributeColor("selectTextColor", node.SelectTextColor);
            //node.FolderIconOpen = (GUIImage)FromId(iconOpenId);
            //node.FolderIconClose = (GUIImage)FromId(iconClosedId);
            //node.Icon = (GUIImage)FromId(iconId);

            string onClicked = element.GetAttributeString("onClicked");
            //node.OnClicked.AddListener(scripting.CreateAction<GUIButton>(onClicked));

            //node.Name = (GUITextBlock)FromId(textBlockId);
            //node.Header = (GUIImage)FromId(imageId);

            //string onClicked = element.GetAttributeString("onClicked");
            //button.OnClicked.AddListener(scripting.CreateAction<GUIButton>(onClicked));

            return node;
        }

        private GUINodeEditor LoadGUINodeEditor(XElement element)
        {
            GUINodeEditor editor = new GUINodeEditor();

            //editor.Settings.BackGroundColor = element.GetAttributeColor("backGroundColor", editor.Settings.BackGroundColor);
            //editor.Settings.NodeSize = element.GetAttributePoint("nodeSize", editor.Settings.NodeSize);
            //editor.Settings.HeaderSize = element.GetAttributeFloat("headerSize", editor.Settings.HeaderSize);
            //editor.Settings.PinSize = element.GetAttributePoint("pinSize", editor.Settings.PinSize);
            //editor.Settings.PinSpacing = element.GetAttributeInt("pinSpacing", editor.Settings.PinSpacing);
            //editor.Settings.PinOn = element.GetAttributeMonoObject<Sprite>("pinOn");
            //editor.Settings.PinOff = element.GetAttributeMonoObject<Sprite>("pinOff");

            return editor;
        }
    }
}
