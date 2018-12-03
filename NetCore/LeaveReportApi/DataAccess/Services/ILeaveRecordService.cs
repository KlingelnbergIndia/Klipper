using Models.Core.Employment;
using Models.Core.HR.Attendance;
using Models.Core.Operationals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveReportApi.LeaveReportDataAccess.Services
{
    public interface ILeaveRecordService
    {
        List<LeaveRecord> LeavesForAllEmployee();
        LeaveRecord TotalLeaveByEmpId(int empId);
        List<LeaveRecord> TotalLeaveInTeam(Department department);
        Dictionary<string, int> GetBalanceByEmp(int empId);
        List<Dictionary<int, Dictionary<string, int>>> GetBalanceByDept(Department department);
        Dictionary<int, List<Dictionary<int, Dictionary<string, int>>>> GetBalanceOfAllEmp(List<Department> departments);
    }
}
