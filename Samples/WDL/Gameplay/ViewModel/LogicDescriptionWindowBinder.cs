using Fitamas;
using Fitamas.UserInterface;
using Fitamas.UserInterface.Components;
using Fitamas.UserInterface.ViewModel;
using Microsoft.Xna.Framework;
using System;

namespace WDL.Gameplay.ViewModel
{
    public class LogicDescriptionWindowBinder : GUIWindowBinder<LogicDescriptionWindowViewModel>
    {
        public const string DefaultName = "NewComponent";

        protected override IDisposable OnBind(LogicDescriptionWindowViewModel viewModel)
        {
            SetAlignment(GUIAlignment.Center);
            SizeToContent = GUISizeToContent.WidthAndHeight;
            IsOnTop = true;

            Point padding = ResourceDictionary.DefaultResources.FramePadding;

            GUIStack stack0 = new GUIStack();
            stack0.Orientation = GUIGroupOrientation.Vertical;
            stack0.ControlSizeWidth = true;
            stack0.ControlSizeHeight = true;
            stack0.Spacing = padding.Y;
            stack0.IsFocusScope = true;
            Content = stack0;
            AddChild(stack0);

            GUITextBlock textBlock0 = GUI.CreateTextBlock(Point.Zero, "Do you want to save this component?");
            stack0.AddChild(textBlock0);

            GUIStack stack1 = GUIHelpers.Sameline();
            stack1.Spacing = padding.X;
            stack0.AddChild(stack1);

            GUITextBlock textBlock1 = GUI.CreateTextBlock(Point.Zero, "Name:");
            stack1.AddChild(textBlock1);

            GUITextInput input = GUI.CreateTextInput(Point.Zero, 500);
            input.OneLine = true;
            input.Text = DefaultName;
            bool flag = viewModel.IsSavedCurrentComponent();
            if (flag)
            {
                input.Text = viewModel.Name;
            }
            else if (viewModel.Contain(DefaultName))
            {
                int id = 1;
                while (viewModel.Contain(DefaultName + id))
                {
                    id++;
                }
                input.Text += id;
            }
            viewModel.Name = input.Text;
            input.OnEndEdit.AddListener((i, s) =>
            {
                if (!viewModel.Contain(s))
                {
                    viewModel.Name = s;
                }
            });
            stack1.AddChild(input);

            GUIStack stack2 = GUIHelpers.Sameline();
            stack2.Spacing = padding.X;
            stack0.AddChild(stack2);

            GUIComboBox comboBox = GUI.CreateComboBox(Point.Zero);
            comboBox.LocalSize += new Point(200, 0);
            comboBox.Items.AddRange(["Red", "Green", "Blue"]);
            comboBox.SelectedItem = viewModel.ThemeId;
            comboBox.OnSelectItem.AddListener((b, a) =>
            {
                viewModel.ThemeId = a.Index;
            });
            stack2.AddChild(comboBox);

            GUITextBlock textBlock2 = GUI.CreateTextBlock(Point.Zero, "A component with this name already exists");
            textBlock2.Color = Color.Red;
            textBlock2.Enable = false;
            stack0.AddChild(textBlock2);

            GUIStack stack3 = GUIHelpers.Sameline();
            stack3.Spacing = padding.X;
            stack0.AddChild(stack3);

            GUIButton button0 = GUI.CreateButton(Point.Zero, "Save");
            button0.OnClicked.AddListener(b =>
            {
                if (!viewModel.Contain(viewModel.Name))
                {
                    viewModel.SaveComponent();
                    Close();
                }
                else
                {
                    textBlock2.Enable = true;
                }
            });
            stack3.AddChild(button0);

            GUIButton button1 = GUI.CreateButton(Point.Zero, "Cancel");
            button1.OnClicked.AddListener(b =>
            {
                Close();
            });
            stack3.AddChild(button1);

            return null;
        }
    }
}
