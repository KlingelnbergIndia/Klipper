using Models.Core.Authentication;
using Newtonsoft.Json;
using Sparkle.Appearance;
using Sparkle.Controls.Dialogs;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
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

        public static void OnPasswordForgotten(string username)
        {
            if (String.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please enter the correct username. An email with password will be sent to you.");
                return;
            }
            //Do something to send an email to user resetting the password.
        }

        public bool Login(string username, string password)
        {
            var hash = ToSha256(password);
            return LoginWithHashedPassword(username, hash);
        }

        public bool LoginWithHashedPassword(string username, string hash)
        {
            var user = new User()
            {
                UserName = username,
                PasswordHash = hash
            };
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:7000/");
            var json = JsonConvert.SerializeObject(user, Formatting.Indented);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync("api/auth/login", httpContent).Result;
            if (response.IsSuccessStatusCode)
            {
                Auth.SessionToken = ExtractToken(response);
                StoreToVault(username, hash);
                return true;
            }
            return false;
        }

        //This will be a separate module (a C++ library) which will manage the security vault
        private void StoreToVault(string username, string hash)
        {
            //Stores current username or password to vault
        }

        internal class TokenResponse
        {
            public string Token { get; set; } = "";
            public DateTime Expiration { get; set; }
            public string Username { get; set; } = "";
        }

        private static string ExtractToken(HttpResponseMessage response)
        {
            //Extract jwt from response
            var jsonString = response.Content.ReadAsStringAsync().Result;
            var t = JsonConvert.DeserializeObject<TokenResponse>(jsonString);
            return t.Token;
        }

        internal static string ToSha256(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.ASCII.GetBytes(input);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }

        }

    }
}