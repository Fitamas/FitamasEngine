using Fitamas.UserInterface.Components;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface
{
    public enum GUIMessageBoxType
    {
        OK,
        OKCancel,
        YesNo,
        YesNoCancel
    }

    public enum GUIMessageBoxResult
    {
        None,
        OK,
        Yes,
        No,
        Cancel
    }

    public class GUIMessageBox
    {
        private static GUIMessageBox instance = new GUIMessageBox();
        public static GUIMessageBox Instance => instance;

        private GUIWindow window;
        private GUIButton okButton;
        private GUIButton cancelButton;
        private GUIButton yesButton;
        private GUIButton noButton;
        private GUITextBlock textBox;

        public GUIMessageBoxResult DialogResult { get; private set; }
        public Action<GUIMessageBoxResult> ResultAction { get; private set; }

        public GUIMessageBox()
        {

        }

        public GUIWindow Init(GUIComponent component)
        {
            window = GUI.CreateWindow<GUIWindow>();
            window.Parent = component;
            window.Enable = false;
            window.DestroyOnClose = false;
            window.SetAlignment(GUIAlignment.Center);
            window.OnClose.AddListener(w =>
            {
                DialogResult = GUIMessageBoxResult.None;
                ResultAction?.Invoke(DialogResult);
                window.Enable = false;
            });
            window.SizeToContent = GUISizeToContent.WidthAndHeight;

            Point padding = ResourceDictionary.DefaultResources.WindowPadding;

            GUIStack stack0 = new GUIStack();
            stack0.Orientation = GUIGroupOrientation.Vertical;
            stack0.ControlSizeWidth = true;
            stack0.ControlSizeHeight = true;
            stack0.Spacing = padding.Y;
            stack0.IsFocusScope = true;
            stack0.ChildAlignment = GUIAlignment.CenterTop;
            window.Content = stack0;
            window.AddChild(stack0);

            textBox = GUI.CreateTextBlock(Point.Zero, string.Empty);
            stack0.AddChild(textBox);

            GUIStack stack1 = GUIHelpers.Sameline();
            stack0.AddChild(stack1);

            okButton = GUI.CreateButton(Point.Zero, "Ok");
            okButton.OnClicked.AddListener(b =>
            {
                window.Enable = false;
                DialogResult = GUIMessageBoxResult.OK;
                ResultAction?.Invoke(DialogResult);
            });
            stack1.AddChild(okButton);

            yesButton = GUI.CreateButton(Point.Zero, "Yes");
            yesButton.OnClicked.AddListener(b =>
            {
                window.Enable = false;
                DialogResult = GUIMessageBoxResult.Yes;
                ResultAction?.Invoke(DialogResult);
            });
            stack1.AddChild(yesButton);

            noButton = GUI.CreateButton(Point.Zero, "No");
            noButton.OnClicked.AddListener(b =>
            {
                window.Enable = false;
                DialogResult = GUIMessageBoxResult.No;
                ResultAction?.Invoke(DialogResult);
            });
            stack1.AddChild(noButton);

            cancelButton = GUI.CreateButton(Point.Zero, "Cancel");
            cancelButton.OnClicked.AddListener(b =>
            {
                window.Enable = false;
                DialogResult = GUIMessageBoxResult.Cancel;
                ResultAction?.Invoke(DialogResult);
            });
            stack1.AddChild(cancelButton);

            return window;
        }

        private void ShowCore(string message, string caption, GUIMessageBoxType buttons, Action<GUIMessageBoxResult> resultAction, bool onMousePosition = false)
        {
            ResultAction = resultAction;

            window.Enable = true;

            textBox.Text = message;
            okButton.Enable = false;
            yesButton.Enable = false;
            noButton.Enable = false;
            cancelButton.Enable = false;

            switch(buttons)
            {
                case GUIMessageBoxType.OK:
                    okButton.Enable = true;
                    break;
                case GUIMessageBoxType.OKCancel:
                    okButton.Enable = true;
                    cancelButton.Enable = true;
                    break;
                case GUIMessageBoxType.YesNo:
                    yesButton.Enable = true;
                    noButton.Enable = true;
                    break;
                case GUIMessageBoxType.YesNoCancel:
                    yesButton.Enable = true;
                    noButton.Enable = true;
                    cancelButton.Enable = true;
                    break;
            }

            if (onMousePosition)
            {
                window.LocalPosition = window.System.Mouse.Position;
            }
            else
            {
                window.LocalPosition = Point.Zero;
            }
        }

        public static void Show(string message, Action<GUIMessageBoxResult> resultAction, bool onMousePosition = false)
        {
            Instance.ShowCore(message, string.Empty, GUIMessageBoxType.OK, resultAction, onMousePosition);
        }

        public static void Show(string message, string caption, Action<GUIMessageBoxResult> resultAction, bool onMousePosition = false)
        {
            Instance.ShowCore(message, caption, GUIMessageBoxType.OK, resultAction, onMousePosition);
        }

        public static void Show(string message, string caption, GUIMessageBoxType buttons, Action<GUIMessageBoxResult> resultAction, bool onMousePosition = false)
        {
            Instance.ShowCore(message, caption, buttons, resultAction, onMousePosition);
        }
    }
}
