using Models.Core.Employment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeApi.DataAccess.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployees();

        Task<Employee> Get(int id);

        Task<bool> Exists(int id);

        Task<bool> Add(Employee item);

        Task<bool> Remove(int id);

        Task<bool> RemoveAll();

        Task<bool> Update(int id, Employee item);
    }
}
