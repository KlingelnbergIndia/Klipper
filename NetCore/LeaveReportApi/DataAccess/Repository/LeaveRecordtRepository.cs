using Common.DataAccess;
using LeaveReportApi.LeaveDataAccess.Repository;
using LeaveReportApi.LeaveReportDataAccess.Services;
using Microsoft.Extensions.Options;
using Models.Core.Employment;
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
        public Dictionary<string, int> GetBalanceByEmp(int empId)
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
        public List<Dictionary<int, Dictionary<string,int>>> GetBalanceByDept(Department department)
        {
            var distinctEmployees = department.Employees;
            List<Dictionary<int, Dictionary<string, int>>> listOfEmp = new List<Dictionary<int, Dictionary<string, int>>>();
            foreach (var emp in distinctEmployees)
            {
                int personalLeave = 21, sickLeave = 6, totalLeave;
                var leavesofEmp = _context.LeaveCollection.Find(y => y.EmployeeID.Equals(emp) && y.LeaveStatus == LeaveStatus.Approved).ToList();
                personalLeave -= leavesofEmp.Where(leave => leave.LeaveType == LeaveType.PersonalLeave).Count();
                sickLeave -= leavesofEmp.Where(leave => leave.LeaveType == LeaveType.SickLeave).Count();
                totalLeave = personalLeave + sickLeave;
                Dictionary<int, Dictionary<string, int> > empData= new Dictionary<int, Dictionary<string, int>>();

                Dictionary<string, int> dictionary = new Dictionary<string, int>();
                dictionary.Add("personalLeave", personalLeave);
                dictionary.Add("sickLeave", sickLeave);
                dictionary.Add("totalLeave", totalLeave);

                empData.Add(emp,dictionary);
                listOfEmp.Add(empData);
            }
            return listOfEmp;
        }
        public Dictionary<int, List<Dictionary<int, Dictionary<string, int>>>> GetBalanceOfAllEmp(List<Department> departments)
        {
            Dictionary<int, List<Dictionary<int, Dictionary<string, int>>>> listOfDeptData = new Dictionary<int, List<Dictionary<int, Dictionary<string, int>>>>();
            //List < List<Dictionary<Employee, Dictionary<string, int>>> > listOfDeptData = new List<List<Dictionary<Employee, Dictionary<string, int>>>>();
            foreach (var department in departments)
            {
                List<Dictionary<int, Dictionary<string, int>>> listOfEmp = new List<Dictionary<int, Dictionary<string, int>>>();
                foreach (var emp in department.Employees)
                {
                    

                    int personalLeave = 21, sickLeave = 6, totalLeave;
                    var leavesofEmp = _context.LeaveCollection.Find(y => y.EmployeeID.Equals(emp) && y.LeaveStatus == LeaveStatus.Approved).ToList();
                    personalLeave -= leavesofEmp.Where(leave => leave.LeaveType == LeaveType.PersonalLeave).Count();
                    sickLeave -= leavesofEmp.Where(leave => leave.LeaveType == LeaveType.SickLeave).Count();
                    totalLeave = personalLeave + sickLeave;
                    Dictionary<int, Dictionary<string, int>> empData = new Dictionary<int, Dictionary<string, int>>();

                    Dictionary<string, int> dictionary = new Dictionary<string, int>();
                    dictionary.Add("personalLeave", personalLeave);
                    dictionary.Add("sickLeave", sickLeave);
                    dictionary.Add("totalLeave", totalLeave);

                    empData.Add(emp, dictionary);
                    listOfEmp.Add(empData);
                }
                listOfDeptData.Add(department.ID,listOfEmp);
            }
            return listOfDeptData;
        }
    }
}
