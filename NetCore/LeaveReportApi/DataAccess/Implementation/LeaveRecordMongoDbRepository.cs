using Common.DataAccess;
using LeaveReportApi.LeaveDataAccess.Repository;
using LeaveReportApi.LeaveReportDataAccess.Services;
using Microsoft.Extensions.Options;
using Models.Core.HR.Attendance;
using Models.Core.Operationals;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace LeaveReportApi.LeaveReportDataAccess.Repository
{
    public class LeaveRecordMongoDbRepository : ILeaveRecordRepository
    {
        private LeaveDbContext _context;

        public LeaveRecordMongoDbRepository(IOptions<DBConnectionSettings> settings)
        {
            _context = LeaveDbContext.GetInstance(settings);
        }

        public List<LeaveRecord> LeaveOfAllEmployee()
        {
            var distinctEmployees = _context.LeaveCollection.Distinct(K => K.EmployeeID, K => true).ToList();
            List<LeaveRecord> listOfLeaveRecord = new List<LeaveRecord>();
            foreach (var employee in distinctEmployees)
            {
                var leavesOfEmployee = _context.LeaveCollection.Find(y => y.EmployeeID.Equals(employee) && y.LeaveStatus != LeaveStatus.Rejected);
                var appliedOrPaindingLeaves = leavesOfEmployee.ToList();
                LeaveRecord leaveRecord = new LeaveRecord();
                leaveRecord.EmployeeId = employee;
                leaveRecord.ApprovedEarnedLeaves = appliedOrPaindingLeaves.Where(leave => leave.LeaveStatus == LeaveStatus.Approved && leave.LeaveType != LeaveType.SickLeave).ToList();
                leaveRecord.ApprovedSickLeaves = appliedOrPaindingLeaves.Where(leave => leave.LeaveStatus == LeaveStatus.Approved && leave.LeaveType == LeaveType.SickLeave).ToList();
                leaveRecord.PendingAppliedEarnedLeaves = appliedOrPaindingLeaves.Where(leave => leave.LeaveStatus == LeaveStatus.Applied).ToList();
                listOfLeaveRecord.Add(leaveRecord);
            }
            return listOfLeaveRecord;
        }

        public LeaveRecord GetTotalLeaveByEmpId(int empId)
        {
            var leavesOfEmployee = _context.LeaveCollection.Find(y => y.EmployeeID.Equals(empId) && y.LeaveStatus != LeaveStatus.Rejected);
            var appliedOrPaindingLeaves = leavesOfEmployee.ToList();
            LeaveRecord leaveRecordOfEmployee = new LeaveRecord();
            leaveRecordOfEmployee.EmployeeId = empId;
            leaveRecordOfEmployee.ApprovedEarnedLeaves = appliedOrPaindingLeaves.Where(leave => leave.LeaveStatus == LeaveStatus.Approved && leave.LeaveType != LeaveType.SickLeave).ToList();
            leaveRecordOfEmployee.ApprovedSickLeaves = appliedOrPaindingLeaves.Where(leave => leave.LeaveStatus == LeaveStatus.Approved && leave.LeaveType == LeaveType.SickLeave).ToList();
            leaveRecordOfEmployee.PendingAppliedEarnedLeaves = appliedOrPaindingLeaves.Where(leave => leave.LeaveStatus == LeaveStatus.Applied).ToList();
            return leaveRecordOfEmployee;
        }

        public List<LeaveRecord> GetTotalLeavesInTeam(Department department)
        {
            var distinctEmployees = department.Employees;
            List<LeaveRecord> listOfLeaveRecord = new List<LeaveRecord>();
            foreach (var employee in distinctEmployees)
            {
                var leavesofEmployee = _context.LeaveCollection.Find(y => y.EmployeeID.Equals(employee) && y.LeaveStatus != LeaveStatus.Rejected);
                var appliedOrPaindingLeaves = leavesofEmployee.ToList();
                LeaveRecord leaveRecordOfEmployee = new LeaveRecord();
                leaveRecordOfEmployee.EmployeeId = employee;
                leaveRecordOfEmployee.ApprovedEarnedLeaves = appliedOrPaindingLeaves.Where(leave => leave.LeaveStatus == LeaveStatus.Approved && leave.LeaveType != LeaveType.SickLeave).ToList();
                leaveRecordOfEmployee.ApprovedSickLeaves = appliedOrPaindingLeaves.Where(leave => leave.LeaveStatus == LeaveStatus.Approved && leave.LeaveType == LeaveType.SickLeave).ToList();
                leaveRecordOfEmployee.PendingAppliedEarnedLeaves = appliedOrPaindingLeaves.Where(leave => leave.LeaveStatus == LeaveStatus.Applied).ToList();
                listOfLeaveRecord.Add(leaveRecordOfEmployee);
            }
            return listOfLeaveRecord;
        }

        public LeaveBalance GetLeaveBalanceByEmployee(int empId)
        {
            int personalLeave = 21, sickLeave = 6, totalLeave;
            var leavesofEmp = _context.LeaveCollection.Find(y => y.EmployeeID.Equals(empId) && y.LeaveStatus == LeaveStatus.Approved).ToList();
            personalLeave -= leavesofEmp.Where(leave => leave.LeaveType == LeaveType.PersonalLeave).Count();
            sickLeave -= leavesofEmp.Where(leave => leave.LeaveType == LeaveType.SickLeave).Count();
            LeaveBalance leaveBalance = new LeaveBalance();
            leaveBalance.EmployeeID = empId;
            leaveBalance.Personal = personalLeave;
            leaveBalance.Sick = sickLeave;
            return leaveBalance;
        }

        public List<LeaveBalance> GetLeaveBalanceByDepartment(Department department)
        {
            var distinctEmployees = department.Employees;
            List<LeaveBalance> listOfLeaveBalance = new List<LeaveBalance>();
            foreach (var employee in distinctEmployees)
            {
                int personalLeave = 21, sickLeave = 6, totalLeave;
                var leavesofEmp = _context.LeaveCollection.Find(y => y.EmployeeID.Equals(employee) && y.LeaveStatus == LeaveStatus.Approved).ToList();
                personalLeave -= leavesofEmp.Where(leave => leave.LeaveType == LeaveType.PersonalLeave).Count();
                sickLeave -= leavesofEmp.Where(leave => leave.LeaveType == LeaveType.SickLeave).Count();
                LeaveBalance leaveBalance = new LeaveBalance();
                leaveBalance.EmployeeID = employee;
                leaveBalance.Personal = personalLeave;
                leaveBalance.Sick = sickLeave;
                listOfLeaveBalance.Add(leaveBalance);

            }
            return listOfLeaveBalance;
        }

        public Dictionary<int,List<LeaveBalance>> GetLeaveBalanceOfAllEmployee(List<Department> departments)
        {
            Dictionary<int, List<LeaveBalance>> leaveBalanceByDept = new Dictionary<int, List<LeaveBalance>>();
            foreach (var department in departments)
            {
                List<LeaveBalance> listOfLeaveBalance = new List<LeaveBalance>();
                foreach (var employee in department.Employees)
                {
                    int personalLeave = 21, sickLeave = 6, totalLeave;
                    var leavesofEmp = _context.LeaveCollection.Find(y => y.EmployeeID.Equals(employee) && y.LeaveStatus == LeaveStatus.Approved).ToList();
                    personalLeave -= leavesofEmp.Where(leave => leave.LeaveType == LeaveType.PersonalLeave).Count();
                    sickLeave -= leavesofEmp.Where(leave => leave.LeaveType == LeaveType.SickLeave).Count();
                    LeaveBalance leaveBalance = new LeaveBalance();
                    leaveBalance.EmployeeID = employee;
                    leaveBalance.Personal = personalLeave;
                    leaveBalance.Sick = sickLeave;
                    listOfLeaveBalance.Add(leaveBalance);
                }
                leaveBalanceByDept.Add(department.ID, listOfLeaveBalance);
            }
            return leaveBalanceByDept;
        }
    }
}
