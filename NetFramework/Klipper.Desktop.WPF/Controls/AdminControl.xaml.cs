using Klipper.Desktop.WPF.CustomControls;
using Sparkle.Appearance;
using Sparkle.Controls.Buttons;
using Sparkle.Controls.Dialogs;
using Sparkle.Controls.Panels;
using Sparkle.DataStructures;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Klipper.Desktop.WPF.Controls
{
    /// <summary>
    /// Interaction logic for AdminControl.xaml
    /// </summary>
    public partial class AdminControl : UserControl
    {
        public AdminControl()
        {
            InitializeComponent();
            AdminMenu.CollapsedWidth = 50.0;
            AdminMenu.ExpandedWidth = 220.0;
            AdminMenu.SelectedIndex = 0;
            AdminMenu.MenuSelectionChanged += OnMenuSelectionChanged;
            AdminMenu.Expand();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadMenuItems();
        }

        private void LoadMenuItems()
        {
            var strs = new Dictionary<string, string>() {
                { "User Management", "Continue_disabled.png" },
                { "Settings", "Continue_disabled.png" },
            };

            foreach (var k in strs.Keys)
            {
                var imageName = strs[k];
                var tab = new SelectableItem(k, GetControl(imageName), "./Images/Generic/" + imageName)
                {
                    IconHeight = 35,
                    IconWidth = 35,
                    ItemHeight = 50
                };
                AdminMenu.AddMenuItem(tab);
            }
        }

        private ContentControl GetControl(string imageName)
        {
            var imageSource = (ImageSource)new ImageSourceConverter().ConvertFromString("./Images/Generic/" + imageName);
            var image = new Image()
            {
                Source = imageSource,
                Width = 290
            };

            var b = new Border()
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                Child = image,
                CornerRadius = new CornerRadius(0),
                Margin = new Thickness(0),
                Padding = new Thickness(0),
                Background = new SolidColorBrush(Color.FromRgb(45, 55, 65)),
            };
            return new ContentControl()
            {
                Content = b,
                VerticalAlignment = VerticalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Stretch
            };

        }

        private void OnMenuSelectionChanged(object sender, SelectableItemSelectionChangedEventArgs e)
        {
            if (e.Current.Header.Equals("User Management", StringComparison.OrdinalIgnoreCase))
            {
                InteractionArea.Content = new EmployeeListPanelControl(); //load dynamic panel
            }
            else if(e.Current.Header.Equals("Settings", StringComparison.OrdinalIgnoreCase))
            {
                InteractionArea.Content = null;
            }
            AdminMenu.ForceResize();
        }

        private void AddEmployee_click(object sender, RoutedEventArgs e)
        {
            GenericDialog dlg = new GenericDialog
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Header = "Add New Employee",
                ShowCloseButton = true,
            };
            dlg.CollapseBottomRegion();
            AppearanceManager.SetAppearance(dlg);

            BasicDialogPanel cp = new BasicDialogPanel();
            cp.Container.Content = new AddEmployeeControl();

            dlg.SetDialogRegion(cp);
            dlg.DialogClosed += (s, args) => { dlg.Close(); };

            var btn = new PanelButton() { ButtonWidth = 150, ButtonText = "close!" };
            btn.Clicked += (s, args) => { dlg.Close(); };
            dlg.AddButton(btn);
            dlg.ShowDialog();
        }

        private void RefreshEmployee_click(object sender, RoutedEventArgs e)
        {
            InteractionArea.Content = new EmployeeListPanelControl();
        }
    }
}
