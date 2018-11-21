using Models.Core.Employment;
using Models.Core.HR.Attendance;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KlipperApi.Controllers.Employees
{
    public interface IEmployeesAccessor
    {
        Task<Employee> GetEmployeeByIdAsync(int employeeId);
        Task<Employee> GetEmployeeByUserName(string userName);
    }
}