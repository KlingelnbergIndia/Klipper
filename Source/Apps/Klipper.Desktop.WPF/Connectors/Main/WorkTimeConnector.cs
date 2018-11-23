using System;
using System.Windows;
using Klipper.Desktop.WPF.Connectors.WorkTime;
using Klipper.Desktop.WPF.Views.Main;
using Sparkle.Controls.Navigators;
using Sparkle.DataStructures;
using Klipper.Desktop.WPF.Views.WorkTime;

namespace Klipper.Desktop.WPF.Connectors.Main
{
    public class WorkTimeConnector : AbstractConnector
    {
        #region Constructor

        public WorkTimeConnector(string tag, MainConnector parent, WorkTimeControl control)
            : base(parent, control)
        {
            this.Tag = tag;
            Ui.Loaded += OnLoaded;
        }

        #endregion

        #region Properties

        public HamburgerNavigator Navigator { get { return (Ui as WorkTimeControl).Navigator; } }

        #endregion

        #region Public methods

        #endregion

        #region Loading methods

        protected override void LoadViews()
        {
            LoadMenuItems();
        }

        private void LoadMenuItems()
        {
            Navigator.Loaded += (s, e) =>
            {
                Navigator.Menu.HeaderText = "Work Time";
                Navigator.Menu.CollapsedWidth = 50.0;
                Navigator.Menu.ExpandedWidth = 200.0;
                Navigator.Menu.MenuSelectionChanged += OnMenuSelectionChanged;
                Navigator.Menu.SelectedIndex = 0;
                Navigator.Menu.Expand();

                AddChild("Attendance", new AttendanceConnector("Attendance", this, new AttendanceControl()));
                AddChild("Regularization", new RegularizationConnector("Regularization", this, new RegularizationControl()));
                AddChild("Leaves", new LeavesConnector("Leaves", this, new LeavesControl()));

                foreach (var childTag in Children)
                {
                    var childConnector = Child(childTag);
                    var control = childConnector.Ui;
                    var tag = childConnector.Tag;
                    var imageStr = tag.Replace(" ", "");
                    var item = new SelectableItem(childTag, control, "./Images/WorkTime/" + imageStr + "_white.png") { IconHeight = 35, IconWidth = 35, ItemHeight = 50 };
                    Navigator.Menu.AddMenuItem(item);
                }
            };
        }

        #endregion

        #region Event handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Initialize();
        }

        private void OnMenuSelectionChanged(object sender, SelectableItemSelectionChangedEventArgs e)
        {
            //if (StatusPanel == null)
            //{
            //    return;
            //}
            //var currItem = e.Current;
            //var title = currItem.Header;
            //StatusPanel.SetText("Current Session: ", new SolidColorBrush(Colors.YellowGreen));
            //StatusPanel.AddText(title, new SolidColorBrush(Colors.Orange));
        }

        #endregion
    }
}
