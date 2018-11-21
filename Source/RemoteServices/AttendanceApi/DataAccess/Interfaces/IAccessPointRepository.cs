using Models.Core.HR.Attendance;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceApi.DataAccess.Interfaces
{
    public interface IAccessPointRepository
    {
        Task<IEnumerable<AccessPoint>> GetAllAccessPoints();

        Task<AccessPoint> Get(int id);

        Task<bool> Exists(int id);

        Task<bool> Add(AccessPoint item);

        Task<bool> Remove(int id);

        Task<bool> RemoveAll();

        Task<bool> Update(int id, AccessPoint item);
    }
}
