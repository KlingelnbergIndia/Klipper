using System.Net.Http;
using System.Threading.Tasks;
using Common;
using KlipperApi.Controllers.Auth;
using Models.Core.Authentication;
using Models.Core.Employment;
using Newtonsoft.Json;

namespace KlipperApi.Controllers.Employees
{
    public class EmployeesAccessor : IEmployeesAccessor
    {
        private readonly IUserRepository _userRepository = null;

        public EmployeesAccessor(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Employee> GetEmployeeByIdAsync(int employeeId)
        {
            var client = CommonHelper.GetClient(AddressResolver.GetAddress("EmployeeApi", false));
            var str = "api/employees/" + employeeId.ToString();
            HttpResponseMessage response = await client.GetAsync(str);
            var jsonString = await response.Content.ReadAsStringAsync();
            var employee = JsonConvert.DeserializeObject<Employee>(jsonString);

            return employee;
        }

        public async Task<Employee> GetEmployeeByUserName(string userName)
        {
            User user = _userRepository.GetByUserName(userName).Result;
            if ( user == null)
            {
                Serilog.Log.Logger.Error("Username is not found.");
                return null;
            }
            var employeeId = user.ID;
            return await GetEmployeeByIdAsync(employeeId);
        }

    }
}
