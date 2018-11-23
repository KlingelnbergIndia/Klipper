using GalaSoft.MvvmLight.Threading;
using Klipper.Desktop.Service.Login;
using Klipper.Desktop.WPF.Connectors;
using Microsoft.Shell;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Klipper.Desktop.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ISingleInstanceApp
    {
        private const string uniqueIdentifier
            = "Many men go fishing all of their lives without knowing that it is not the fish they are after. - Henry David Thoreau";

        static App()
        {
            DispatcherHelper.Initialize();
        }

        [STAThread]
        public static void Main()
        {
            if (SingleInstance<App>.InitializeAsFirstInstance(uniqueIdentifier))
            {
                var application = new App();
                application.InitializeComponent();
                application.Run();

                // Allow single instance code to perform cleanup operations
                SingleInstance<App>.Cleanup();
            }
        }

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            // Bring window to foreground
            if (this.MainWindow.WindowState == WindowState.Minimized)
            {
                this.MainWindow.WindowState = WindowState.Normal;
            }

            this.MainWindow.Activate();

            return true;
        }

        private void StartApp(object sender, StartupEventArgs e)
        {
            //Extract login data from vault
            var loginData = ExtractLoginData();
            if (loginData == null)
            {
                new LoginLauncher().Launch();
            }
            else
            {
                var success = LoginManager.Instance.LoginWithHashedPassword(loginData.Item1, loginData.Item2);
                if (success)
                {
                    ApplicationLauncher.Instance.Launch();
                }
            }
        }

        private Tuple<string, string> ExtractLoginData()
        {
            //Extract login Data from vault
            return null;
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var msg = "An unrecoverable error has occurred, Krypton needs to close."
                + Environment.NewLine + Environment.NewLine;

            var exception = e.Exception;
            var exceptionMsg = "";
            var count = 1;
            while (exception != null)
            {
                var src = count.ToString() + ") " + "Source: " + exception.Source + " - " + exception.Message;
                exceptionMsg += src + Environment.NewLine;
                exception = exception.InnerException;
                count++;
            }

            var logMsg = msg + exceptionMsg;
            MessageBox.Show(logMsg);
        }

    }
}
