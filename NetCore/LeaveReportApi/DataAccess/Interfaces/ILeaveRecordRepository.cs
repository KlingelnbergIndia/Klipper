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
        Dictionary<string, int> GetLeaveBalanceByEmployee(int empId);
        List<Dictionary<int, Dictionary<string, int>>> GetLeaveBalanceByDepartment(Department department);
        Dictionary<int, List<Dictionary<int, Dictionary<string, int>>>> GetLeaveBalanceOfAllEmployee(List<Department> departments);
    }
}
