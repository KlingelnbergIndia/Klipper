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

        public Dictionary<string, int> GetLeaveBalanceByEmployee(int empId)
        {
            int personalLeave = 21, sickLeave = 6, totalLeave;
            var approvedLeaves = _context.LeaveCollection.Find(y => y.EmployeeID.Equals(empId) && y.LeaveStatus == LeaveStatus.Approved).ToList();
            personalLeave -= approvedLeaves.Where(leave => leave.LeaveType == LeaveType.PersonalLeave).Count();
            sickLeave -= approvedLeaves.Where(leave => leave.LeaveType == LeaveType.SickLeave).Count();
            totalLeave = personalLeave + sickLeave;
            Dictionary<string, int> leavesRemaining = new Dictionary<string, int>();
            leavesRemaining.Add("personalLeave", personalLeave);
            leavesRemaining.Add("sickLeave", sickLeave);
            leavesRemaining.Add("totalLeave", totalLeave);
            return leavesRemaining;
        }

        public List<Dictionary<int, Dictionary<string, int>>> GetLeaveBalanceByDepartment(Department department)
        {
            var distinctEmployees = department.Employees;
            List<Dictionary<int, Dictionary<string, int>>> listOfLeavesRemaining = new List<Dictionary<int, Dictionary<string, int>>>();
            foreach (var employee in distinctEmployees)
            {
                int personalLeave = 21, sickLeave = 6, totalLeave;
                var approvedLeaves = _context.LeaveCollection.Find(y => y.EmployeeID.Equals(employee) && y.LeaveStatus == LeaveStatus.Approved).ToList();
                personalLeave -= approvedLeaves.Where(leave => leave.LeaveType == LeaveType.PersonalLeave).Count();
                sickLeave -= approvedLeaves.Where(leave => leave.LeaveType == LeaveType.SickLeave).Count();
                totalLeave = personalLeave + sickLeave;
                Dictionary<int, Dictionary<string, int>> leavesRemainingByEmployeeId = new Dictionary<int, Dictionary<string, int>>();
                Dictionary<string, int> leavesRemaining = new Dictionary<string, int>();
                leavesRemaining.Add("personalLeave", personalLeave);
                leavesRemaining.Add("sickLeave", sickLeave);
                leavesRemaining.Add("totalLeave", totalLeave);
                leavesRemainingByEmployeeId.Add(employee, leavesRemaining);
                listOfLeavesRemaining.Add(leavesRemainingByEmployeeId);
            }
            return listOfLeavesRemaining;
        }

        public Dictionary<int, List<Dictionary<int, Dictionary<string, int>>>> GetLeaveBalanceOfAllEmployee(List<Department> departments)
        {
            Dictionary<int, List<Dictionary<int, Dictionary<string, int>>>> leavesRemainingByDepartmentId = new Dictionary<int, List<Dictionary<int, Dictionary<string, int>>>>();
            foreach (var department in departments)
            {
                List<Dictionary<int, Dictionary<string, int>>> listOfLeavesRemaining = new List<Dictionary<int, Dictionary<string, int>>>();
                foreach (var employee in department.Employees)
                {
                    int personalLeave = 21, sickLeave = 6, totalLeave;
                    var leavesofEmp = _context.LeaveCollection.Find(y => y.EmployeeID.Equals(employee) && y.LeaveStatus == LeaveStatus.Approved).ToList();
                    personalLeave -= leavesofEmp.Where(leave => leave.LeaveType == LeaveType.PersonalLeave).Count();
                    sickLeave -= leavesofEmp.Where(leave => leave.LeaveType == LeaveType.SickLeave).Count();
                    totalLeave = personalLeave + sickLeave;
                    Dictionary<int, Dictionary<string, int>> leavesRemainingByEmployeeId = new Dictionary<int, Dictionary<string, int>>();
                    Dictionary<string, int> dictionary = new Dictionary<string, int>();
                    dictionary.Add("personalLeave", personalLeave);
                    dictionary.Add("sickLeave", sickLeave);
                    dictionary.Add("totalLeave", totalLeave);
                    leavesRemainingByEmployeeId.Add(employee, dictionary);
                    listOfLeavesRemaining.Add(leavesRemainingByEmployeeId);
                }
                leavesRemainingByDepartmentId.Add(department.ID, listOfLeavesRemaining);
            }
            return leavesRemainingByDepartmentId;
        }
    }
}
