using System.Threading.Tasks;
using KlipperApi.Controllers.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Core.Employment;

namespace KlipperApi.Controllers.Employees
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmployeesAccessor _employeesAccessor;
        private readonly IAuthorizationService _authorizationService;

        public EmployeesController(
            IAuthorizationService authorizationService,
            IUserRepository userRepository, 
            IEmployeesAccessor employeesAccessor)
        {
            _authorizationService = authorizationService;
            _userRepository = userRepository;
            _employeesAccessor = employeesAccessor;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ReadBasicEmployeeInfo")]
        public async Task<IActionResult> Get(int employeeId)
        {
            var e = await _employeesAccessor.GetEmployeeByIdAsync(employeeId) as Employee;
            return Ok(e);
        }

        [HttpGet("ByUserName")]
        [Authorize(Policy = "ReadBasicEmployeeInfo")]
        public async Task<IActionResult> Get(string userName)
        {
            var e = await _employeesAccessor.GetEmployeeByUserName(userName) as Employee;
            return Ok(e);
        }
    }
}
