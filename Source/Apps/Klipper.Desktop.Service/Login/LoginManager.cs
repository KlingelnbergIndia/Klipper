using Common;
using Klipper.Desktop.Service.Employees;
using Klipper.Desktop.Service.Session;
using Models.Core.Authentication;
using Models.Core.Employment;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Klipper.Desktop.Service.Login
{
    public class LoginManager
    {
        #region Instance

        static LoginManager _instance = null;

        public static LoginManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LoginManager();
                }
                return _instance;
            }
        }

        public static void DeleteInstance()
        {
            _instance = null;
        }

        private LoginManager()
        {
        }

        #endregion

        #region Properties

        public Employee CurrentEmployee { get; private set; } 

        #endregion

        public void HandleForgottenPassword(string username)
        {
            if (String.IsNullOrWhiteSpace(username))
            {
                throw new Exception("Please enter the correct username. An email with password will be sent to you.");
            }
            //Do something to send an email to user resetting the password.
        }

        public bool Login(string username, string password)
        {
            var hash = CommonHelper.ToSha256(password);
            var success = LoginWithHashedPassword(username, hash);
            return success;
        }

        public bool LoginWithHashedPassword(string username, string hash)
        {
            var user = new User()
            {
                UserName = username,
                PasswordHash = hash
            };
            var client = CommonHelper.GetClient(AddressResolver.GetAddress("KlipperApi", false));
            var json = JsonConvert.SerializeObject(user, Formatting.Indented);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = client.PostAsync("api/auth/login", httpContent).Result;
            if (response.IsSuccessStatusCode)
            {
                Auth.SessionToken = ExtractToken(response);
                StoreToVault(username, hash);
                Employee employee = EmployeeService.Instance.GetEmployeeByUserName(username);
                if (employee != null)
                {
                    SessionContext.CurrentSubject = employee;
                }
                return true;
            }
            return false;
        }

        //This will be a separate module (a C++ library) which will manage the security vault
        private void StoreToVault(string username, string hash)
        {
            //Stores current username or password to vault
        }

        private string ExtractToken(HttpResponseMessage response)
        {
            //Extract jwt from response
            var jsonString = response.Content.ReadAsStringAsync().Result;
            var t = JsonConvert.DeserializeObject<TokenResponse>(jsonString);
            return t.Token;
        }
    }
}



