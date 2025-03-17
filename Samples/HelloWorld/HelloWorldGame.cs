using Fitamas.Entities;
using Fitamas.Input;
using Fitamas.Core;
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

            Debug.Log("Hello World");
        }

        protected override void LoadContent()
        {
            GUIDebug.Active = true;

            FontManager.DefaultFont = Content.Load<SpriteFont>("Font\\Pixel_20");
            ResourceDictionary.DefaultResources[CommonResourceKeys.DefaultFont] = FontManager.DefaultFont;

            GUISystem system = Container.Resolve<GUISystem>("gui_system");
            system.LoadScreen("Layouts\\MainMenu.xml");

            GUIButton button = GUI.CreateButton(new Point(0, 100), "Button from C# script");
            button.SetAlignment(GUIAlignment.Center);
            button.OnClicked.AddListener((b, a) => { Debug.Log(b); });
            system.AddComponent(button);

            GUITextBlock textBlock = GUI.CreateTextBlock(new Point(50, 10), "Hello World!\nHello World!");
            textBlock.SetValue(GUITextBlock.ScaleProperty, 1.5f)
                     .SetValue(GUIComponent.PivotProperty, new Vector2(0, 0));
            textBlock.SetValue(GUITextBlock.AutoScaleProperty, false);
            textBlock.LocalSize = new Point(400, 120);
            textBlock.TextHorisontalAlignment = GUITextHorisontalAlignment.Middle;
            textBlock.TextVerticalAlignment = GUITextVerticalAlignment.Middle;
            system.AddComponent(textBlock);

            textBlock = GUI.CreateTextBlock(new Point(50, 150), "Enable button:");
            textBlock.SetValue(GUIComponent.PivotProperty, new Vector2(0, 0));
            system.AddComponent(textBlock);

            GUICheckBox checkBox = GUI.CreateCheckBox(Point.Zero);
            checkBox.SetValue(GUIComponent.HorizontalAlignmentProperty, GUIHorizontalAlignment.Right)
                    .SetValue(GUIComponent.VerticalAlignmentProperty, GUIVerticalAlignment.Center)
                    .SetValue(GUIComponent.PivotProperty, new Vector2(0, 0.5F))
                    .SetValue(GUICheckBox.ValueProperty, button.Interacteble);
            checkBox.OnValueChanged.AddListener((r, v) => { button.Interacteble = v; });
            textBlock.AddChild(checkBox);

            GUIComboBox comboBox = GUI.CreateComboBox(new Point(50, 200), size: new Point(250, 45));
            comboBox.Pivot = new Vector2(0, 0);
            comboBox.OnSelectItem.AddListener((r, v) => { Debug.Log(v.Item); });
            comboBox.SetItems<GUIAlignment>();
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
            lineRenderer.LocalSize = new Point(200, 200);
            lineRenderer.Pivot = new Vector2(0, 0);
            lineRenderer.SetAnchorPoints([new Point(0, 200), new Point(0, 0), new Point(100, 200), new Point(200, 0), new Point(200, 200)]);
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

            InputSystem.mouse.MouseUp += (s, e) =>
            {
                if (e.Button == MouseButton.Right)
                {
                    GUIContextMenu contextMenu = GUI.CreateContextMenu(e.Position);
                    contextMenu.AddItem("Test_111111");
                    contextMenu.AddItem("Test_222");
                    contextMenu.AddItem("Test_3");
                    system.Root.OpenPopup(contextMenu);
                }
            };

            base.LoadContent();
        }
    }
}
