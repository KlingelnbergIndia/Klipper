using Klipper.Desktop.Service.Session;
using Klipper.Desktop.WPF.Connectors.Main;
using Klipper.Desktop.WPF.Views.Main;
using Sparkle.Appearance;
using Sparkle.Controls.Buttons;
using Sparkle.Controls.Navigators;
using Sparkle.Controls.Panels;
using Sparkle.Controls.Toolbars;
using Sparkle.Controls.Workflows;
using Sparkle.DataStructures;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Klipper.Desktop.WPF.Connectors
{
    public class MainConnector : AbstractConnector
    {
        #region Instance

        static MainConnector _instance = null;

        public static MainConnector Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MainConnector();
                }
                return _instance;
            }
        }

        public static void DeleteInstance()
        {
            _instance = null;
        }

        private MainConnector()
            : base(null, null)
        {
        }

        #endregion

        #region Events

        //public event EventHandler<string> XYZEvent = null;

        #endregion

        #region Properties

        public HamburgerNavigator MainMenuNavigator { get; private set; } = null;
        public WindowHeaderPanel TopPanel { get; private set; } = null;
        public ContentControl BottomPanel { get; private set; } = null;
        public VerticalToolbar SideToolbar { get; private set; } = null;
        public MultiColorTextPanel StatusPanel { get; private set; } = null;
        public MultiColorTextPanel TopRightTextPanel { get; private set; } = null;
        public StackPanel TopLeftContainer { get; private set; } = null;
        public StackPanel TopRightContainer { get; private set; } = null;

        #endregion

        #region Public methods

        #endregion

        #region Loading methods

        protected override void LoadViews()
        {
            AssignLayoutElementProperties();
            LoadMainMenuItems();
            LoadTopPanel();
            LoadBottomPanel();
            LoadSideToolbar();
        }

        private void AssignLayoutElementProperties()
        {
            MainMenuNavigator = (Ui as StockApplicationWindow).Navigator;
            TopPanel = (Ui as StockApplicationWindow).TopPanel;
            BottomPanel = (Ui as StockApplicationWindow).BottomPanel;
            SideToolbar = (Ui as StockApplicationWindow).SideToolbar;
        }

        private void LoadMainMenuItems()
        {
            MainMenuNavigator.Loaded += (s, e) =>
            {
                MainMenuNavigator.Menu.HeaderText = "Main Menu";
                MainMenuNavigator.Menu.CollapsedWidth = 50.0;
                MainMenuNavigator.Menu.ExpandedWidth = 200.0;
                MainMenuNavigator.Menu.MenuSelectionChanged += OnMenuSelectionChanged;
                MainMenuNavigator.Menu.SelectedIndex = 0;
                MainMenuNavigator.Menu.Expand();

                AddChild("Home", new HomeConnector("Home", this, new HomeControl()));
                AddChild("Work Time", new WorkTimeConnector("Work Time", this, new WorkTimeControl()));
                AddChild("Accounts", new AccountsConnector("Accounts", this, new AccountsControl()));
                AddChild("Documents", new DocumentsConnector("Documents", this, new DocumentsControl()));
                AddChild("Admin", new AdminConnector("Admin", this, new AdminControl()));
                AddChild("Management", new ManagementConnector("Management", this, new ManagementControl()));
                AddChild("Settings", new SettingsConnector("Settings", this, new SettingsControl()));
                AddChild("Help", new HelpConnector("Help", this, new HelpControl()));

                foreach (var childTag in this.Children)
                {
                    var childConnector = this.Child(childTag);
                    var control = childConnector.Ui;
                    var tag = childConnector.Tag;
                    var imageStr = tag.Replace(" ", "");
                    var item = new SelectableItem(childTag, control, "./Images/MainMenu/" + imageStr + "_white.png") { IconHeight = 35, IconWidth = 35, ItemHeight = 50 };
                    MainMenuNavigator.Menu.AddMenuItem(item);
                }
            };
        }

        private void LoadSideToolbar()
        {
            SideToolbar.Loaded += (s, e) =>
            {
                SideToolbar.HorizontalAlignment = HorizontalAlignment.Stretch;
                var iconSize = 35.0;
                SideToolbar.AddTool(GetToolbarButton("DeveloperToolbox/Save_environment", iconSize, () => { MessageBox.Show("Save test environment clicked."); }, "Save test environment", true));
                SideToolbar.AddTool(GetToolbarButton("DeveloperToolbox/Load_environment", iconSize, () => { MessageBox.Show("Load test environment clicked."); }, "Load test environment", true));
                SideToolbar.AddToolSeparator();
                SideToolbar.AddTool(GetToolbarButton("DeveloperToolbox/Create_test", iconSize, () => { MessageBox.Show("Create test clicked."); }, "Create a automated regression test", true));
                SideToolbar.AddTool(GetToolbarButton("DeveloperToolbox/Test_manager", iconSize, () => { MessageBox.Show("Test manager clicked."); }, "Open test manager", true));
                SideToolbar.AddTool(GetToolbarButton("DeveloperToolbox/Report_bug", iconSize, () => { MessageBox.Show("Report bug clicked."); }, "Report a bug/issue", true));
            };
        }

        private void LoadBottomPanel()
        {
            BottomPanel.Loaded += (s, e) =>
            {
                StatusPanel = new MultiColorTextPanel()
                {
                    IsSelectable = true,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    SelectedBackground = AppearanceManager.GetCurrentSkinResource("BackgroundBase_05") as Brush,
                };
                BottomPanel.Content = StatusPanel;
                StatusPanel.IsSelectable = true;
                StatusPanel.Clicked += (sender, args) => { MessageBox.Show("Override action when clicked on text-panel, such as showing session log."); };
                StatusPanel.SetText("One can add application status here.", new SolidColorBrush(Colors.YellowGreen));
                StatusPanel.AddText(" Click to execute the registered action.", new SolidColorBrush(Colors.Orange));
            };
        }

        private void LoadTopPanel()
        {
            TopPanel.Loaded += (s, e) =>
            {
                //var iconSize = 35.0;
                //(Ui as StockApplicationWindow).TopToolContainer.Children.Add(GetToolbarButton("Krypton/Save", iconSize, () => { MessageBox.Show("Save drawing changes clicked."); }, "Save drawing changes"));

                TopLeftContainer = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };
                (Ui as StockApplicationWindow).TopToolContainer.Children.Add(TopLeftContainer);
                TopRightContainer = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center
                };
                (Ui as StockApplicationWindow).TopToolContainer.Children.Add(TopRightContainer);
                TopPanel.UpdateLayout();

                TopRightTextPanel = new MultiColorTextPanel()
                {
                    IsSelectable = true,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    HorizontalContentAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    SelectedBackground = AppearanceManager.GetCurrentSkinResource("BackgroundBase_05") as Brush,
                };
                TopRightTextPanel.IsSelectable = true;
                TopRightTextPanel.Clicked += (sender, args) => { MessageBox.Show("Popup current user info dialog!"); };
                TopRightContainer.Children.Add(TopRightTextPanel);
                TopRightContainer.Children.Add(new VerticalSeparator());
                if (SessionContext.CurrentSubject != null)
                {
                    TopRightTextPanel.SetText(SessionContext.CurrentSubject.FirstName + " " + SessionContext.CurrentSubject.LastName, new SolidColorBrush(Colors.YellowGreen));
                }
            };

        }

        #endregion


        #region Event handlers

        private void OnMenuSelectionChanged(object sender, SelectableItemSelectionChangedEventArgs e)
        {
            if (StatusPanel == null)
            {
                return;
            }
            var currItem = e.Current;
            var title = currItem.Header;
            StatusPanel.SetText("Current Session: ", new SolidColorBrush(Colors.YellowGreen));
            StatusPanel.AddText(title, new SolidColorBrush(Colors.Orange));
        }

        internal void HandleWindowClose()
        {
            //throw new NotImplementedException();
        }

        #endregion

        #region Private methods

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

        #endregion


    }
}
