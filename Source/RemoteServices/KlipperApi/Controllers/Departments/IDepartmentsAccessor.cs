using Models.Core.Operationals;
using System.Threading.Tasks;

namespace KlipperApi.Controllers.Departments
{
    public interface IDepartmentsAccessor
    {
        Task<Department> GetDepartmentByIdAsync(int departmentId);
        Task<Department> GetDepartmentByNameAsync(string departmentName);
    }
}