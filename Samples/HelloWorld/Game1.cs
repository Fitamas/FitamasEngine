using Fitamas.Entities;
using Fitamas.Input;
using Fitamas.Core;
using Fitamas.UserInterface;
using Fitamas.UserInterface.Components;
using Fitamas.UserInterface.Themes;
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

            GUISystem system = Container.Resolve<GUISystem>("gui_system");

            FontManager.DefaultFont = Content.Load<SpriteFont>("Font\\Pixel_20");
            ResourceDictionary.DefaultResources[CommonResourceKeys.DefaultFont] = FontManager.DefaultFont;

            GUIButton button = GUI.CreateButton(new Point(0, -100), "Button from C# script");
            button.SetAlignment(GUIAlignment.Center);
            button.OnClicked.AddListener(r => { Debug.Log(r); });
            system.AddComponent(button);

            GUITextBlock textBlock = GUI.CreateTextBlock(new Point(50, -50), "Hello World!\nHello World!");
            textBlock.SetValue(GUITextBlock.ScaleProperty, 1.5f)
                     .SetValue(GUIComponent.PivotProperty, new Vector2(0, 1));
            textBlock.SetAlignment(GUIAlignment.LeftTop);
            system.AddComponent(textBlock);

            textBlock = GUI.CreateTextBlock(new Point(50, -150), "Enable button:");
            textBlock.SetValue(GUIComponent.PivotProperty, new Vector2(0, 1));
            textBlock.SetAlignment(GUIAlignment.LeftTop);
            system.AddComponent(textBlock);

            GUICheckBox checkBox = GUI.CreateCheckBox(Point.Zero);
            checkBox.SetValue(GUIComponent.HorizontalAlignmentProperty, GUIHorizontalAlignment.Right)
                    .SetValue(GUIComponent.VerticalAlignmentProperty, GUIVerticalAlignment.Center)
                    .SetValue(GUIComponent.PivotProperty, new Vector2(0, 0.5F))
                    .SetValue(GUICheckBox.ValueProperty, button.Interacteble);
            checkBox.OnValueChanged.AddListener((r, v) => { button.Interacteble = v; });
            textBlock.AddChild(checkBox);

            GUIComboBox comboBox = GUI.CreateComboBox(new Point(50, -200), ["item1", "item2", "item3"], new Point(250, 45));
            comboBox.Pivot = new Vector2(0, 1);
            comboBox.SetAlignment(GUIAlignment.LeftTop);
            comboBox.OnSelectItem.AddListener((r, v) => { Debug.Log(v); });
            system.AddComponent(comboBox);

            GUIStack group = new GUIStack();
            group.Orientation = GUIGroupOrientation.Horizontal;
            group.LocalPosition = new Point(50, -250);
            group.LocalSize = new Point(400, 100);
            group.ControlChildSizeWidth = true;
            group.ControlChildSizeHeight = true;
            group.ControlSizeWidth = true;
            group.ControlSizeHeight = true;
            group.Spacing = 10;
            group.Pivot = new Vector2(0, 1);
            group.SetAlignment(GUIAlignment.LeftTop);
            system.AddComponent(group);

            group.AddChild(GUI.CreateButton(Point.Zero, "A"));
            group.AddChild(GUI.CreateButton(Point.Zero, "B"));
            group.AddChild(GUI.CreateButton(Point.Zero, "C"));
            group.AddChild(GUI.CreateButton(Point.Zero, "D"));
            group.AddChild(GUI.CreateButton(Point.Zero, "E"));

            InputSystem.mouse.MouseUp += (s, e) =>
            {
                if (e.Button == MouseButton.Right)
                {
                    GUIContextMenu contextMenu = GUI.CreateContextMenu(e.Position);
                    contextMenu.AddItem("Test111111");
                    contextMenu.AddItem("Test222");
                    contextMenu.AddItem("Test3");
                    system.Root.OpenPopup(contextMenu);
                }
            };

            //TODO max and min size

            base.LoadContent();
        }
    }
}
