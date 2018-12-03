using Models.Core.Employment;
using Models.Core.HR.Attendance;
using Models.Core.Operationals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveReportApi.LeaveReportDataAccess.Services
{
    public interface ILeaveRecordRepository
    {
        List<LeaveRecord> LeaveOfAllEmployee();
        LeaveRecord GetTotalLeaveByEmpId(int empId);
        List<LeaveRecord> GetTotalLeavesInTeam(Department department);
        LeaveBalance GetLeaveBalanceByEmployee(int empId);
        List<LeaveBalance> GetLeaveBalanceByDepartment(Department department);
        Dictionary<int, List<LeaveBalance>> GetLeaveBalanceOfAllEmployee(List<Department> departments);
    }
}
