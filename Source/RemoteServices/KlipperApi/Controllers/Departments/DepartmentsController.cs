using System.Threading.Tasks;
using KlipperApi.Controllers.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Core.Employment;
using Models.Core.Operationals;

namespace KlipperApi.Controllers.Departments
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentsAccessor _employeesAccessor;
        private readonly IAuthorizationService _authorizationService;

        public DepartmentsController(
            IAuthorizationService authorizationService,
            IUserRepository userRepository, 
            IDepartmentsAccessor employeesAccessor)
        {
            _authorizationService = authorizationService;
            _userRepository = userRepository;
            _employeesAccessor = employeesAccessor;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ReadBasicDepartmentInfo")]
        public async Task<IActionResult> Get(int employeeId)
        {
            var e = await _employeesAccessor.GetDepartmentByIdAsync(employeeId) as Department;
            return Ok(e);
        }

        [HttpGet("ByDepartmentName")]
        [Authorize(Policy = "ReadBasicDepartmentInfo")]
        public async Task<IActionResult> Get(string departmentName)
        {
            var e = await _employeesAccessor.GetDepartmentByNameAsync(departmentName) as Department;
            return Ok(e);
        }
    }
}
