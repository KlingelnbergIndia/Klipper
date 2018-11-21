using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Common;
using Models.Core.Employment;
using Models.Core.HR.Attendance;
using Newtonsoft.Json;

namespace KlipperApi.Controllers.Employees
{
    public class EmployeesAccessor : IEmployeesAccessor
    {

        public async Task<Employee> GetEmployeeByIdAsync(int employeeId)
        {
            var client = CommonHelper.GetClient(AddressResolver.GetAddress("EmployeeApi", false));
            var str = "api/employees/" + employeeId.ToString();
            HttpResponseMessage response = await client.GetAsync(str);
            var jsonString = await response.Content.ReadAsStringAsync();
            var employees = JsonConvert.DeserializeObject<Employee>(jsonString);

            return employees;
        }

        public async Task<Employee> GetEmployeeByUserName(string userName)
        {
            var client = CommonHelper.GetClient(AddressResolver.GetAddress("EmployeeApi", false));
            var str = "api/accessevents/byUserName?UserName=" + userName;
            HttpResponseMessage response = await client.GetAsync(str);
            var jsonString = await response.Content.ReadAsStringAsync();
            var employees = JsonConvert.DeserializeObject<Employee>(jsonString);

            return employees;
        }

    }
}
