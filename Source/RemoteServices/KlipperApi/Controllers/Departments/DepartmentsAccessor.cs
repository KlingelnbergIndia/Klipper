using System.Net.Http;
using System.Threading.Tasks;
using Common;
using KlipperApi.Controllers.Auth;
using Models.Core.Authentication;
using Models.Core.Employment;
using Models.Core.Operationals;
using Newtonsoft.Json;

namespace KlipperApi.Controllers.Departments
{
    public class DepartmentsAccessor : IDepartmentsAccessor
    {
        private readonly IUserRepository _userRepository = null;

        public DepartmentsAccessor(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Department> GetDepartmentByIdAsync(int departmentId)
        {
            var client = CommonHelper.GetClient(AddressResolver.GetAddress("OperationalsApi", false));
            var str = "api/departments/" + departmentId.ToString();
            HttpResponseMessage response = await client.GetAsync(str);
            var jsonString = await response.Content.ReadAsStringAsync();
            var department = JsonConvert.DeserializeObject<Department>(jsonString);

            return department;
        }

        public async Task<Department> GetDepartmentByNameAsync(string departmentName)
        {
            var client = CommonHelper.GetClient(AddressResolver.GetAddress("OperationalsApi", false));
            var str = "api/departments/ByDepartmentName?departmentName=" + departmentName;
            HttpResponseMessage response = await client.GetAsync(str);
            var jsonString = await response.Content.ReadAsStringAsync();
            var department = JsonConvert.DeserializeObject<Department>(jsonString);

            return department;
        }
    }
}
