using Models.Core.HR.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveReportApi.LeaveReportDataAccess.Services
{
    public interface ILeaveService
    {
       void AddLeave(Leave leave);
    }
}
