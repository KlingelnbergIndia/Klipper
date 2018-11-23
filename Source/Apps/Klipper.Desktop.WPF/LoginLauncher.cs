using Sparkle.Appearance;
using Sparkle.Controls.Dialogs;
using System.Windows;
using System.Windows.Controls;

namespace Klipper.Desktop.WPF
{
    internal class LoginLauncher
    {
        LoginControl _loginControl = null;

        public LoginLauncher()
        {
        }

        internal void Launch()
        {
            _loginControl = new LoginControl();

            var control = new ContentControl()
            {
                Content = _loginControl
            };

            AnimatedDialog dialog = new AnimatedDialog(
                new Point(0, 0),
                BalloonPopupPosition.ScreenCenter,
                control,
                50, true, true, null, null)
            {
                ShowHeaderPanel = true,
                ShowTickButton = false
            };

            _loginControl.Closed += (s, e) => { dialog.Close(); };

            dialog.ShowCloseButton = true;
            dialog.ShowMaximizeRestore = false;
            dialog.Topmost = false;
            AppearanceManager.SetAppearance(dialog);
            dialog.Show();
        }

    }
}