using Models.Core.Operationals;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OperationalsApi.DataAccess.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllDepartments();

        Task<Department> GetDepartment(int id);

        Task<Department> GetDepartmentByName(string departmentName);

        Task AddDepartment(Department item);

        Task<bool> RemoveDepartment(int id);

        Task<bool> UpdateDepartment(int id, Department item);

        Task<bool> RemoveAllDepartments();
    }
}
