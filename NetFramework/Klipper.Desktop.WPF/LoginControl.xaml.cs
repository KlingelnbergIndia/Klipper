using Klipper.Desktop.Service.Login;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Klipper.Desktop.WPF
{
    /// <summary>
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl, INotifyPropertyChanged
    {
        public event EventHandler Closed;

        public LoginControl()
        {
            InitializeComponent();
        }

        #region INotifyPropertyChanged 

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Properties
        public bool ShouldEnableLoginButton
        {
            get
            {
                return !String.IsNullOrWhiteSpace(UsernameTextbox.Text) && !String.IsNullOrWhiteSpace(UsernameTextbox.Text);
            }
        }
        #endregion

        private void Login_Clicked(object sender, EventArgs e)
        {
            var username = UsernameTextbox.Text;
            var password = PasswordTextbox.Text;

            var success = LoginManager.Instance.Login(username, password);
            if(success)
            {
                ApplicationLauncher.Instance.Launch();
                Closed?.Invoke(this, null);
            }
            else
            {
                MessageBox.Show("Username or password is incorrect!");
            }
        }

        private void ForgotPasswordLabel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                LoginManager.Instance.HandleForgottenPassword(UsernameTextbox.Text);
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void PasswordTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RaisePropertyChanged("ShouldEnableLoginButton");
        }

        private void UsernameTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RaisePropertyChanged("ShouldEnableLoginButton");
        }
    }
}
