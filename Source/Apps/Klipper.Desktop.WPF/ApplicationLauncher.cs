﻿using Sparkle.Appearance;
using Sparkle.Controls.Panels;
using Sparkle.Controls.Workflows;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows;
using Sparkle.Controls.Buttons;
using Sparkle.DataStructures;
using Sparkle.Controls.Navigators;
using System.Windows.Controls;
using Klipper.Desktop.WPF.Views.Main;
using Klipper.Desktop.WPF.Connectors;
using Klipper.Desktop.Service.EmployeeProfile;
using Models.Core.Employment;

namespace Klipper.Desktop.WPF
{
    public class ApplicationLauncher
    {
        #region Instance

        static ApplicationLauncher _instance = null;

        public static ApplicationLauncher Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ApplicationLauncher();
                }
                return _instance;
            }
        }

        public static void DeleteInstance()
        {
            _instance = null;
        }

        private ApplicationLauncher()
        {
        }

        #endregion

        #region Fields

        private MultiColorTextPanel _statusBottomTextPanel;

        private Dictionary<string, UserControl>  _mainMenuControls 
            = new Dictionary<string, UserControl>()
            {
                { "Home", new HomeControl() },
                { "Work Time", new WorkTimeControl() },
                { "Salary", new SalaryControl() },
                { "Documents", new DocumentsControl() },
                { "Admin", new AdminControl() },
                { "Management", new ManagementControl() },
                { "Settings", new SettingsControl() },
                { "Help", new HelpControl() },
            };

        private MultiColorTextPanel _topRightTextPanel;
        private StockApplicationWindow _appWindow;

        #endregion

        #region Public methods

        public void Launch()
        {
            StockApplicationWindow w = new StockApplicationWindow()
            {
                WindowBackground = AppearanceManager.GetCurrentSkinResource("BackgroundBase_02") as Brush,
                TopPanelBackground = AppearanceManager.GetCurrentSkinResource("BackgroundBase_04") as Brush,
                BottomPanelBackground = AppearanceManager.GetCurrentSkinResource("BackgroundBase_02") as Brush,
                SideToolbarBackground = AppearanceManager.GetCurrentSkinResource("BackgroundBase_05") as Brush,
                TopPanelHeight = 55,
                BottomPanelHeight = 65,
                TopStripHeight = 10,
                BottomStripHeight = 5,
                WindowHeaderIcon = (ImageSource)new ImageSourceConverter().ConvertFromString("./Images/Klingelnberg/Klingelnberg_Logo.png"),
                WindowHeader = "Klipper",
                ShowSideToolbar = true,
                WindowState = WindowState.Normal
            };
            LoadNavigator(w);
            LoadTopPanel(w);
            LoadBottomPanel(w);
            LoadSideToolbar(w);

            AppearanceManager.CurrentSkin = SkinType.OrangeOnBlack;

            w.WindowState = System.Windows.WindowState.Normal;
            w.Topmost = false;

            //Register and unregister events on MainConnector
            w.Closed += (s, e) =>
            {
                UnregisterMainConnectorEvents();
            };
            RegisterMainConnectorEvents();

            _appWindow = w;

            w.Show();
        }

        #endregion

        #region Other Private Methods

        private void RegisterMainConnectorEvents()
        {
            MainConnector.Instance.LoginSuccessful += OnSuccessfulLogin;
        }

        private void UnregisterMainConnectorEvents()
        {
            MainConnector.Instance.LoginSuccessful -= OnSuccessfulLogin;
        }

        #endregion

        #region Loading methods

        private void LoadSideToolbar(StockApplicationWindow w)
        {
            var s = w.SideToolbar;
            s.HorizontalAlignment = HorizontalAlignment.Stretch;
            var iconSize = 35.0;
            s.AddTool(GetToolbarButton("DeveloperToolbox/Save_environment", iconSize, () => { MessageBox.Show("Save test environment clicked."); }, "Save test environment", true));
            s.AddTool(GetToolbarButton("DeveloperToolbox/Load_environment", iconSize, () => { MessageBox.Show("Load test environment clicked."); }, "Load test environment", true));
            s.AddToolSeparator();
            s.AddTool(GetToolbarButton("DeveloperToolbox/Create_test", iconSize, () => { MessageBox.Show("Create test clicked."); }, "Create a automated regression test", true));
            s.AddTool(GetToolbarButton("DeveloperToolbox/Test_manager", iconSize, () => { MessageBox.Show("Test manager clicked."); }, "Open test manager", true));
            s.AddTool(GetToolbarButton("DeveloperToolbox/Report_bug", iconSize, () => { MessageBox.Show("Report bug clicked."); }, "Report a bug/issue", true));
        }

        private void LoadBottomPanel(StockApplicationWindow w)
        {
            var b = w.BottomPanel;
            _statusBottomTextPanel = new MultiColorTextPanel()
            {
                IsSelectable = true,
                HorizontalAlignment = HorizontalAlignment.Left,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                SelectedBackground = AppearanceManager.GetCurrentSkinResource("BackgroundBase_05") as Brush,
            };
            b.Content = _statusBottomTextPanel;
            _statusBottomTextPanel.IsSelectable = true;
            _statusBottomTextPanel.Clicked += (s, e) => { MessageBox.Show("Override action when clicked on text-panel, such as showing session log."); };
            _statusBottomTextPanel.SetText("One can add application status here.", new SolidColorBrush(Colors.YellowGreen));
            _statusBottomTextPanel.AddText(" Click to execute the registered action.", new SolidColorBrush(Colors.Orange));

        }

        private void LoadTopPanel(StockApplicationWindow w)
        {
            var t = w.TopPanel;
            t.Loaded += (s, e) =>
            {
                //var iconSize = 35.0;
                //w.TopToolContainer.Children.Add(GetToolbarButton("Krypton/Save", iconSize, () => { MessageBox.Show("Save drawing changes clicked."); }, "Save drawing changes"));
            };
        }

        private void LoadNavigator(StockApplicationWindow w)
        {
            var n = w.Navigator;
            n.Menu.HeaderText = "Main Menu";
            n.Menu.CollapsedWidth = 50.0;
            n.Menu.ExpandedWidth = 200.0;
            n.Menu.MenuSelectionChanged += OnMenuSelectionChanged;
            n.Menu.SelectedIndex = 0;
            n.Menu.Expand();

            LoadMenuItems(n);
        }

        private ToolButton GetToolbarButton(string iconId, double iconSize, Action action, string toolTip, bool showGlow = false)
        {
            var s = "./Images/" + iconId;
            var btn = new ToolButton(s + "_enabled.png", s + "_disabled.png", s + "_mouse_over.png");
            btn.Width = iconSize;
            btn.Height = iconSize;
            btn.VerticalAlignment = VerticalAlignment.Center;
            btn.Action = action;
            btn.TooltipText = toolTip;
            btn.GlowOnMouseOver = showGlow;
            btn.MouseOverGlowColor = Colors.SkyBlue;
            return btn;
        }

        private void LoadMenuItems(HamburgerNavigator navigator)
        {
            foreach (var k in _mainMenuControls.Keys)
            {
                var control = _mainMenuControls[k];
                var imageStr = k.Replace(" ", "");
                var item = new SelectableItem(k, control, "./Images/MainMenu/" + imageStr + "_white.png") { IconHeight = 35, IconWidth = 35, ItemHeight = 50 };
                navigator.Menu.AddMenuItem(item);
            }
        }

        private ContentControl GetControl(string imageName)
        {
            var imageSource = (ImageSource)new ImageSourceConverter().ConvertFromString("./Images/Klingelnberg/" + imageName);
            var image = new Image()
            {
                Source = imageSource,
            };

            var b = new Border()
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                Child = image,
            };
            return new ContentControl()
            {
                Content = b,
                VerticalAlignment = VerticalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Stretch
            };
        }

        #endregion

        #region Event handlers

        private void OnSuccessfulLogin(object sender, string userName)
        {
            Employee e = EmployeeProfileService.Instance.GetEmployeeByUserName(userName);
            if (e != null)
            {
                _topRightTextPanel.SetText(e.FirstName + " " + e.LastName, new SolidColorBrush(Colors.Azure));
            }
        }

        private void OnMenuSelectionChanged(object sender, SelectableItemSelectionChangedEventArgs e)
        {
            if (_statusBottomTextPanel == null)
            {
                return;
            }
            var currItem = e.Current;
            var title = currItem.Header;
            _statusBottomTextPanel.SetText("Current Session: ", new SolidColorBrush(Colors.YellowGreen));
            _statusBottomTextPanel.AddText(title, new SolidColorBrush(Colors.Orange));
        }

        #endregion
    }
}
