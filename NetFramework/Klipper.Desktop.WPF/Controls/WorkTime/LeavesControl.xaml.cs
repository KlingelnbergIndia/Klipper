using Klipper.Desktop.WPF.Controls.CustomControls;
using Sparkle.Appearance;
using Sparkle.Controls.Dialogs;
using Sparkle.Controls.Tabs;
using Sparkle.DataStructures;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Klipper.Desktop.WPF.Controls.WorkTime
{
    /// <summary>
    /// Interaction logic for LeavesControl.xaml
    /// </summary>
    public partial class LeavesControl : UserControl
    {
        private Tabber _tabber = null;

        public LeavesControl()
        {
            InitializeComponent();
            _tabber = this.TheTabber;
            _tabber.MenuSelectionChanged += MenuSelectionChanged;
            LoadTabs();
        }

        private void LoadTabs()
        {
            var strs = new Dictionary<string, string>()
            {
                {"Home","Klingelnberg_building.jpg"},
                {"Admin","klingelnberg_office.jpg" }
            };

            foreach (var k in strs.Keys)
            {
                var tabName = strs[k];
                _tabber.AddTab(new SelectableItem(k, GetControl(tabName), "./Images/Generic/" + k + "_white.png") { ItemMinWidth = 120 });
            }


        }

        private ContentControl GetControl(string imageName)
        {
            var imageSource = (ImageSource)new ImageSourceConverter().ConvertFromString("./Images/Klingelnberg/" + imageName);
            //var image = new Image()
            //{
            //    Source = imageSource,
            //    Width = 650,
            //    Height = 450,
            //    Margin = new Thickness(15)
            //};
            object control = null;

            if (imageName == "Klingelnberg_building.jpg")
            {
                control = new SelfControll();
            }
            else
            {
                control = new LeaveEmployeesList();
            }
                       
            return new ContentControl()
            {
                Content = control,
                Background = new SolidColorBrush(Colors.Transparent),
                VerticalAlignment = VerticalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Stretch,
                Margin = new Thickness(0),
                Padding = new Thickness(10)
            };

        }

        private void MenuSelectionChanged(object sender, SelectableItemSelectionChangedEventArgs selectableItemSelectionChangedEventArgs)
        {
        }
    }
}
