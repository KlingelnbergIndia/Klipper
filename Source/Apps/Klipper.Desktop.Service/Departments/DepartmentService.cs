using Newtonsoft.Json;
using System;
using System.Net.Http;
using Models.Core.Employment;
using Klipper.Desktop.Service.Login;
using Common;
using Klipper.Desktop.Service.WorkTime;
using Klipper.Desktop.Service.WorkTime.Policies;
using Models.Core.Operationals;

namespace Klipper.Desktop.Service.Departments
{
    public class DepartmentService
    {
        #region Instance

        static DepartmentService _instance = null;

        public static DepartmentService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DepartmentService();
                }
                return _instance;
            }
        }

        public static void DeleteInstance()
        {
            _instance = null;
        }

        private DepartmentService()
        {
        }

        #endregion

        #region Public methods

        public Department GetDepartmentById(int departmentId)
        {
            var client = CommonHelper.GetClient(AddressResolver.GetAddress("KlipperApi", false), Auth.SessionToken);
            var str = "api/departments/" + departmentId.ToString();

            HttpResponseMessage response = client.GetAsync(str).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                var department = JsonConvert.DeserializeObject<Department>(jsonString);
                return department;
            }
            else
            {
                return null;
            }
        }

        public Department GetDepartmentByName(string departmentName)
        {
            var client = CommonHelper.GetClient(AddressResolver.GetAddress("KlipperApi", false), Auth.SessionToken);

            //KK: Please add this action on Employees controller in KlipperAPI
            var str = "api/departments/ByName?departmentName=" + departmentName;

            HttpResponseMessage response = client.GetAsync(str).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                var department = JsonConvert.DeserializeObject<Department>(jsonString);
                return department;
            }
            else
            {
                return null;
            }
        }

        #endregion

    }
}
