using Fitamas.Entities;
using Fitamas.Input;
using Fitamas.Main;
using Fitamas.UserInterface;
using Fitamas.UserInterface.Components;
using Fitamas.UserInterface.Themes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.InteropServices;

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
            base.LoadContent();

            GUIDebug.Active = true;

            GUISystem system = Container.Resolve<GUISystem>("gui_system");

            FontManager.DefaultFont = Content.Load<SpriteFont>("Font\\Pixel_20");
            ResourceDictionary.DefaultResources[CommonResourceKeys.DefaultFont] = FontManager.DefaultFont;

            GUIButton button = GUI.CreateButton(new Rectangle(0, -100, 400, 80), "Button from C# script");
            button.SetAlignment(GUIAlignment.Center);
            //button.Image.Color = Color.Red;
            button.OnClicked.AddListener(r => { Debug.Log(r); });
            system.AddComponent(button);

            GUITextBlock textBlock = GUI.CreateTextBlock("Hello World!\nHello World!");
            textBlock.Pivot = Vector2.Zero;
            textBlock.LocalPosition = new Point(50, -50);
            textBlock.Scale = 1.5f;
            system.AddComponent(textBlock);

            textBlock = GUI.CreateTextBlock("Enable button:");
            textBlock.Pivot = Vector2.Zero;
            textBlock.LocalPosition = new Point(50, -150);
            system.AddComponent(textBlock);

            GUICheckBox checkBox = GUI.CreateCheckBox(new Rectangle(0, 0, 40, 40));
            checkBox.HorizontalAlignment = GUIHorizontalAlignment.Right;
            checkBox.VerticalAlignment = GUIVerticalAlignment.Center;
            checkBox.Pivot = new Vector2(0, 0.5F);
            checkBox.Value = button.Interecteble;
            checkBox.OnValueChanged.AddListener((r, v) => { button.Interecteble = v; });
            textBlock.AddChild(checkBox);

            GUIComboBox comboBox = GUI.CreateComboBox(new Rectangle(50, -200, 200, 40), ["item1", "item2", "item3"]);
            comboBox.Pivot = Vector2.Zero;
            system.AddComponent(comboBox);

            GUIHorizontalGroup group = GUI.CreateHorizontalGroup();
            group.LocalPosition = new Point(50, -250);
            group.CellSize = new Point(50, 50);
            group.Pivot = Vector2.Zero;
            system.AddComponent(group);

            group.AddChild(GUI.CreateButton(new Rectangle(0, 0, 50, 50), "A"));
            group.AddChild(GUI.CreateButton(new Rectangle(0, 0, 50, 50), "B"));
            group.AddChild(GUI.CreateButton(new Rectangle(0, 0, 50, 50), "C"));
            group.AddChild(GUI.CreateButton(new Rectangle(0, 0, 50, 50), "D"));

            InputSystem.mouse.MouseUp += (s, e) =>
            {
                if (e.Button == MonoGame.Extended.Input.MouseButton.Right)
                {
                    GUIContextMenu contextMenu = new GUIContextMenu();
                    contextMenu.LocalScale = new Point(100, 100);
                    contextMenu.LocalPosition = system.Root.ScreenToLocal(e.Position);
                    contextMenu.Pivot = Vector2.Zero;
                    //system.AddComponent(contextMenu);
                    system.Root.OpenPopup(contextMenu);
                }
            };

            //TODO open context on right click
        }
    }
}
