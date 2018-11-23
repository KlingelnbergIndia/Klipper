using Models.Core.HR.Attendance;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceApi.DataAccess.Interfaces
{
    public interface IAccessEventRepository
    {
        Task<IEnumerable<AccessEvent>> GetAllAccessEvents();

        Task<AccessEvent> Get(int id);

        Task<IEnumerable<AccessEvent>> GetByEmployeeId(int employeeId);

        Task<bool> Exists(int id);

        Task<bool> Add(AccessEvent item);

        Task<bool> Remove(int id);

        Task<bool> RemoveAll();

        Task<bool> Update(int id, AccessEvent item);
    }
}
