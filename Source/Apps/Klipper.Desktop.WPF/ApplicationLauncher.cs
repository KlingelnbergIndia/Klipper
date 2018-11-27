using Sparkle.Appearance;
using Sparkle.Controls.Panels;
using Sparkle.Controls.Workflows;
using System;
using System.Windows.Media;
using System.Windows;
using Sparkle.Controls.Buttons;
using Sparkle.DataStructures;
using Sparkle.Controls.Navigators;
using Klipper.Desktop.WPF.Views.Main;
using Klipper.Desktop.WPF.Connectors;
using Klipper.Desktop.Service.Employees;
using Models.Core.Employment;
using Klipper.Desktop.WPF.Connectors.Main;

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

            AppearanceManager.CurrentSkin = SkinType.OrangeOnBlack;
            w.WindowState = System.Windows.WindowState.Normal;
            w.Topmost = false;

            //Register and unregister events on MainConnector
            w.Closed += (s, e) =>
            {
                MainConnector.Instance.HandleWindowClose();
            };

            _appWindow = w;

            MainConnector.Instance.Ui = w;
            MainConnector.Instance.Initialize();

            w.Show();
        }

        #endregion
    }
}
