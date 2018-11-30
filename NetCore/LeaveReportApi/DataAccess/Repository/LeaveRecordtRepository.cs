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
    public class LeaveRecordRepository : ILeaveRecordService
    {
        private DbContext _context;
        public LeaveRecordRepository(IOptions<DBConnectionSettings> settings)
        {
            _context = DbContext.GetInstance(settings);
        }
        public List<LeaveRecord> LeavesForAllEmployee()
        {
            var leaves = _context.LeaveCollection.Find(_=>true);
            var totalLeavesOfAllEmployees=leaves.ToList();

            var distinctEmployees = _context.LeaveCollection.Distinct(K=>K.EmployeeID,K=>true).ToList();

            List<LeaveRecord> totalLeavesRecord = new List<LeaveRecord>();

            foreach(var emp in distinctEmployees)
            {
                var leavesofEmp = _context.LeaveCollection.Find(y => y.EmployeeID.Equals(emp) && y.LeaveStatus != LeaveStatus.Rejected);
                var appliedOrPaindingLeaves = leavesofEmp.ToList();
                LeaveRecord leaveRecord = new LeaveRecord();
                leaveRecord.EmployeeId = emp;
                leaveRecord.ApprovedEarnedLeaves = appliedOrPaindingLeaves.Where(leave => leave.LeaveStatus == LeaveStatus.Approved && leave.LeaveType != LeaveType.SickLeave).ToList();
                leaveRecord.ApprovedSickLeaves = appliedOrPaindingLeaves.Where(leave => leave.LeaveStatus == LeaveStatus.Approved && leave.LeaveType == LeaveType.SickLeave).ToList();
                leaveRecord.PendingAppliedEarnedLeaves = appliedOrPaindingLeaves.Where(leave => leave.LeaveStatus == LeaveStatus.Applied).ToList();
                totalLeavesRecord.Add(leaveRecord);
            }
            return totalLeavesRecord;
        }

        public LeaveRecord TotalLeaveByEmpId(int empId)
        {
            var leaves = _context.LeaveCollection.Find(y => y.EmployeeID.Equals(empId) && y.LeaveStatus != LeaveStatus.Rejected);
            var appliedOrPaindingLeaves = leaves.ToList();
            LeaveRecord leaveRecord = new LeaveRecord();
            leaveRecord.EmployeeId = empId;
            leaveRecord.ApprovedEarnedLeaves = appliedOrPaindingLeaves.Where(leave => leave.LeaveStatus == LeaveStatus.Approved && leave.LeaveType != LeaveType.SickLeave).ToList();
            leaveRecord.ApprovedSickLeaves = appliedOrPaindingLeaves.Where(leave => leave.LeaveStatus == LeaveStatus.Approved && leave.LeaveType == LeaveType.SickLeave).ToList();
            leaveRecord.PendingAppliedEarnedLeaves = appliedOrPaindingLeaves.Where(leave => leave.LeaveStatus == LeaveStatus.Applied).ToList();
            return leaveRecord;

        }
        public List<LeaveRecord> TotalLeaveInTeam(Department department)
        {
            var distinctEmployees = department.Employees;
            List<LeaveRecord> totalLeavesRecord = new List<LeaveRecord>();

            foreach (var emp in distinctEmployees)
            {
                var leavesofEmp = _context.LeaveCollection.Find(y => y.EmployeeID.Equals(emp) && y.LeaveStatus != LeaveStatus.Rejected);
                var appliedOrPaindingLeaves = leavesofEmp.ToList();
                LeaveRecord leaveRecord = new LeaveRecord();
                leaveRecord.EmployeeId = emp;
                leaveRecord.ApprovedEarnedLeaves = appliedOrPaindingLeaves.Where(leave => leave.LeaveStatus == LeaveStatus.Approved && leave.LeaveType != LeaveType.SickLeave).ToList();
                leaveRecord.ApprovedSickLeaves = appliedOrPaindingLeaves.Where(leave => leave.LeaveStatus == LeaveStatus.Approved && leave.LeaveType == LeaveType.SickLeave).ToList();
                leaveRecord.PendingAppliedEarnedLeaves = appliedOrPaindingLeaves.Where(leave => leave.LeaveStatus == LeaveStatus.Applied).ToList();
                totalLeavesRecord.Add(leaveRecord);
            }
            return totalLeavesRecord;
        }
        public Dictionary<string, int> GetCheckBalance(int empId)
        {
            int personalLeave = 21, sickLeave = 6,totalLeave;
            var leavesofEmp = _context.LeaveCollection.Find(y => y.EmployeeID.Equals(empId) && y.LeaveStatus==LeaveStatus.Approved).ToList();
            personalLeave -= leavesofEmp.Where(leave => leave.LeaveType == LeaveType.PersonalLeave).Count();
            sickLeave -= leavesofEmp.Where(leave => leave.LeaveType == LeaveType.SickLeave).Count();
            totalLeave = personalLeave + sickLeave;
            Dictionary< string,int> dictionary = new Dictionary<string, int>();

            dictionary.Add("personalLeave", personalLeave);

            dictionary.Add("sickLeave", sickLeave);

            dictionary.Add("totalLeave", totalLeave);

            return dictionary;
        }
    }
}
