using Newtonsoft.Json;
using System;
using System.Net.Http;
using Models.Core.Employment;
using Klipper.Desktop.Service.Login;

namespace Klipper.Desktop.Service.EmployeeProfile
{
    public class EmployeeProfileService
    {
        #region Instance

        static EmployeeProfileService _instance = null;

        public static EmployeeProfileService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EmployeeProfileService();
                }
                return _instance;
            }
        }

        public static void DeleteInstance()
        {
            _instance = null;
        }

        private EmployeeProfileService()
        {
        }

        #endregion

        #region Fields

        HttpClient _httpClient = new HttpClient() { BaseAddress = new Uri("http://localhost:7000/") };

        #endregion

        #region Public methods

        public Employee GetEmployeeById(int employeeId)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer", Auth.SessionToken);

            var str = "api/employees/" + employeeId.ToString();

            HttpResponseMessage response = _httpClient.GetAsync(str).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                var employee = JsonConvert.DeserializeObject<Employee>(jsonString);
                return employee;
            }
            else
            {
                return null;
            }
        }

        public Employee GetEmployeeByUserName(string userName)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer", Auth.SessionToken);

            //KK: Please add this action on Employees controller in KlipperAPI
            var str = "api/employees/ByUserName?UserName=" + userName;

            HttpResponseMessage response = _httpClient.GetAsync(str).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                var employee = JsonConvert.DeserializeObject<Employee>(jsonString);
                return employee;
            }
            else
            {
                return null;
            }
        }

        #endregion

    }
}
