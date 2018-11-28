using Newtonsoft.Json;
using System.Net.Http;
using Models.Core.Employment;
using Klipper.Desktop.Service.Login;
using Common;
using Klipper.Desktop.Service.WorkTime.Policies;
using Klipper.Desktop.Service.Departments;
using Klipper.Desktop.Service.WorkTime.Policies.SoftwareGroup;
using Klipper.Desktop.Service.WorkTime.Policies.DesignGroup;

namespace Klipper.Desktop.Service.Employees
{
    public class EmployeeService
    {
        #region Instance

        static EmployeeService _instance = null;

        public static EmployeeService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EmployeeService();
                }
                return _instance;
            }
        }

        public static void DeleteInstance()
        {
            _instance = null;
        }

        private EmployeeService()
        {
        }

        #endregion

        #region Public methods

        public Employee GetEmployeeById(int employeeId)
        {
            var client = CommonHelper.GetClient(AddressResolver.GetAddress("KlipperApi", false), Auth.SessionToken);
            var str = "api/employees/" + employeeId.ToString();

            HttpResponseMessage response = client.GetAsync(str).Result;
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
            var client = CommonHelper.GetClient(AddressResolver.GetAddress("KlipperApi", false), Auth.SessionToken);

            //KK: Please add this action on Employees controller in KlipperAPI
            var str = "api/employees/ByUserName?userName=" + userName;

            HttpResponseMessage response = client.GetAsync(str).Result;
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

        public IWorkTimePolicy GetWorkTimePolicy(int employeeId)
        {
            var employee = GetEmployeeById(employeeId);
            var departmentId = employee.DepartmentId;
            var department = DepartmentService.Instance.GetDepartmentById(departmentId);

            if (department.Name == "Software-Metrology" ||
                department.Name == "Software-Digital Unit")
            {
                return new SoftwareGroupWorkTimePolicy();
            }
            if (department.Name == "Design")
            {
                return new DesignGroupWorkTimePolicy();
            }
            return new BaseWorkTimePolicy();
        }

        #endregion

    }
}
