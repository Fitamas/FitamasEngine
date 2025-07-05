using Fitamas.Input;
using Fitamas.Core;
using Fitamas.ECS;
using Fitamas.UserInterface;
using Fitamas.UserInterface.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fitamas.Samples.HelloWorld
{
    public class HelloWorldGame : GameEngine
    {
        protected override void Initialize()
        {
            base.Initialize();

            GameWorld.CreateMainCamera();

            GUISystem system = MainContainer.Resolve<GUISystem>(ApplicationKey.GUISystem);

            GUIButton button = GUI.CreateButton(new Point(0, 100), "Button from C# script");
            button.SetAlignment(GUIAlignment.Center);
            button.OnClicked.AddListener(b =>
            {
                Debug.Log(b);
            });
            system.AddComponent(button);

            GUITextBlock textBlock = GUI.CreateTextBlock(new Point(50, 10), "Hello World!\nHello World!");
            textBlock.SetValue(GUITextBlock.ScaleProperty, 1.5f)
                     .SetValue(GUIComponent.PivotProperty, new Vector2(0, 0));
            textBlock.SetValue(GUITextBlock.AutoScaleProperty, false);
            textBlock.LocalSize = new Point(400, 120);
            textBlock.TextHorisontalAlignment = GUITextHorisontalAlignment.Middle;
            textBlock.TextVerticalAlignment = GUITextVerticalAlignment.Middle;
            system.AddComponent(textBlock);

            GUIStack stack = GUIHelpers.Sameline();
            stack.LocalPosition = new Point(50, 150);
            stack.Pivot = new Vector2(0, 0);
            system.AddComponent(stack);

            GUITextBlock textBlock1 = GUI.CreateTextBlock(Point.Zero, "Enable button:");
            stack.AddChild(textBlock1);

            GUICheckBox checkBox = GUI.CreateCheckBox(Point.Zero);
            checkBox.SetValue(GUICheckBox.ValueProperty, button.Interacteble);
            checkBox.OnValueChanged.AddListener((r, v) => { button.Interacteble = v; });
            stack.AddChild(checkBox);

            GUIComboBox comboBox = GUI.CreateComboBox(new Point(50, 200));
            comboBox.LocalSize += new Point(300, 0);
            comboBox.Pivot = new Vector2(0, 0);
            comboBox.OnSelectItem.AddListener((r, v) => { Debug.Log(v.Item); });
            comboBox.AddItemsFromEnum<GUIAlignment>();
            system.AddComponent(comboBox);

            GUIStack group = new GUIStack();
            group.Orientation = GUIGroupOrientation.Horizontal;
            group.LocalPosition = new Point(50, 250);
            group.LocalSize = new Point(400, 20);
            group.ControlChildSizeWidth = true;
            group.ControlChildSizeHeight = true;
            group.ControlSizeWidth = true;
            group.ControlSizeHeight = true;
            group.Spacing = 10;
            group.Pivot = new Vector2(0, 0);
            system.AddComponent(group);

            group.AddChild(GUI.CreateButton(Point.Zero, "A"));
            group.AddChild(GUI.CreateButton(Point.Zero, "B"));
            group.AddChild(GUI.CreateButton(Point.Zero, "C"));
            group.AddChild(GUI.CreateButton(Point.Zero, "D"));
            group.AddChild(GUI.CreateButton(Point.Zero, "E"));

            GUISlider slider = GUI.CreateSlider(new Point(50, 300), GUISliderDirection.BottomToTop, lenght: 200);
            slider.Pivot = new Vector2(0, 0);
            slider.Track.OnValueChanged.AddListener((s, v) => { Debug.Log(v); });
            slider.Track.MaxValue = 10;
            slider.WholeNumbers = true;
            system.AddComponent(slider);

            GUIImage image = new GUIImage();
            image.LocalSize = new Point(600, 600);

            GUIScrollRect rect = GUI.CreateScrollRect(new Point(50, 550), new Point(400, 400));
            rect.Pivot = new Vector2(0, 0);
            rect.Viewport.AddChild(image);
            rect.Content = image;
            system.AddComponent(rect);

            GUITextInput input = GUI.CreateTextInput(new Point(500, 50));
            input.Pivot = new Vector2(0, 0);
            input.OneLine = false;
            input.LocalSize = new Point(200, 200);
            system.AddComponent(input);

            GUILineRenderer lineRenderer = new GUILineRenderer();
            lineRenderer.LocalPosition = new Point(500, 300);
            lineRenderer.Pivot = new Vector2(0, 0);
            lineRenderer.Anchors.AddRange([new Point(0, 200), new Point(0, 0), new Point(100, 200), new Point(200, 0), new Point(200, 200)]);
            lineRenderer.Thickness = 10;
            lineRenderer.ShadowSize = 20;
            lineRenderer.ShadowEnable = true;
            system.AddComponent(lineRenderer);

            GUITreeView treeView = GUI.CreateTreeView(new Point(750, 50), new Point(300, 300));
            treeView.Pivot = new Vector2(0, 0);
            treeView.OnSelectTreeNode.AddListener(a => Debug.Log(a.Id));
            system.AddComponent(treeView);

            treeView.CreateTreeNode("TEST1");
            GUITreeNode node = treeView.CreateTreeNode("TEST2");
            node.CreateTreeNode("TEST3").CreateTreeNode("TEST4");
            node.CreateTreeNode("TEST5");
            node.CreateTreeNode("TEST6");
            treeView.CreateTreeNode("TEST7");
            treeView.CreateTreeNode("TEST8");

            CreateWindow();

            GUIPopup popup1 = new GUIPopup();
            popup1.PlacementMode = GUIPlacementMode.Mouse;
            system.AddComponent(popup1);

            GUIWindow window1 = new GUIWindow();
            //window1.LocalSize = new Point(200, 200);
            popup1.Window = window1;
            popup1.AddChild(window1);

            GUIImage image1 = new GUIImage();
            image1.LocalSize = new Point(200, 200);
            window1.AddChild(image1);


            GUIPopup popup = new GUIPopup();
            system.AddComponent(popup);

            GUIContextMenu contextMenu = GUI.CreateContextMenu();
            contextMenu.AddItem("Create window");
            contextMenu.AddItem("Message box");
            contextMenu.AddItem("popup");
            contextMenu.AddItem("HI");
            contextMenu.OnSelectItem.AddListener((m, a) =>
            {
                if (a.Index == 0)
                {
                    CreateWindow().LocalPosition = m.System.Mouse.Position;
                }
                else if (a.Index == 1)
                {
                    GUIMessageBox.Show("Hi world!", string.Empty, GUIMessageBoxType.OK, null);
                }
                else if (a.Index == 2)
                {
                    popup1.IsOpen = true;
                }
                else if (a.Index == 3)
                {
                    Debug.Log("HI!!!");
                }
            });
            popup.Window = contextMenu;
            popup.AddChild(contextMenu);
            GUIContextMenuManager.SetContextMenu(system.Root.Screen, contextMenu);

            GUIWindow CreateWindow()
            {
                GUIWindow window = GUI.CreateWindow<GUIWindow>();
                window.LocalPosition = new Point(1200, 50);
                window.LocalSize = new Point(300, 300);
                window.Pivot = new Vector2(0, 0);
                system.AddComponent(window);

                return window;
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            GUIDebug.Active = true;

            FontManager.DefaultFont = Content.Load<SpriteFont>("Font\\Pixel_20");
            ResourceDictionary.DefaultResources[CommonResourceKeys.DefaultFont] = FontManager.DefaultFont;
        }
    }
}
