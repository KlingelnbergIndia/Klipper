using Models.Core.HR.Attendance;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KlipperApi.Controllers.Attendance
{
    public interface IAttendanceAccessor
    {
        Task<IEnumerable<AccessEvent>> GetAttendanceByEmployeeIdAsync(int employeeId);
    }
}