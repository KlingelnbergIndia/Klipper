using LeaveReportApi.DataAccess.Services;
using LeaveReportApi.LeaveReportDataAccess.Services;
using Microsoft.AspNetCore.Mvc;
using Models.Core.Employment;
using Models.Core.HR.Attendance;
using Models.Core.Operationals;
using System;
using System.Collections.Generic;

namespace EmployeeApi.Controllers
{
    [Route("api/[controller]")]
    public class LeaveReportController : Controller
    {
        private ILeaveRecordRepository _leaveRecordRepository;
        private IDepartmentRepository _departmentRepository;

        public LeaveReportController(ILeaveRecordRepository leaveRecordRepository, IDepartmentRepository departmentRepository)
        {
            _leaveRecordRepository = leaveRecordRepository;
            _departmentRepository = departmentRepository;
        }

        // GET api/LeaveReport/TotalLeavesTaken
        [HttpGet]
        [Route("TotalLeavesTaken")]
        public IActionResult GetTotalLeavesTaken()
        {
            List<LeaveRecord> listOfLeaveRecord = _leaveRecordRepository.LeaveOfAllEmployee();
            return Ok(listOfLeaveRecord);
        }

        // GET api/LeaveReport/TotalLeavesTakenByEmpId/empId?empId=63
        [HttpGet]
        [Route("TotalLeavesTakenByEmpId/empId")]
        public IActionResult GetTotalLeavesTakenByEmpId(int empId)
        {
            int employeeId = 0;
            List<Department> listOfDepartment = _departmentRepository.GetAllDepartment();
            foreach(var department in listOfDepartment)
            {
                employeeId = department.Employees.Find(k=>k.Equals(empId));
            }
            if (employeeId==0)
            {
                throw new Exception("Employee Id is not valid");
            }
            var leavesOfEmployee = _leaveRecordRepository.GetTotalLeaveByEmpId(empId);
            return Ok(leavesOfEmployee);
        }

        // GET api/LeaveReport/TotalLeavesTakenInTeam/deptId?deptId=13
        [HttpGet]
        [Route("TotalLeavesTakenInTeam/DeptId")]
        public IActionResult GetTotalLeavesTakenInTeam(int deptId)
        {
            Department department = _departmentRepository.GetDepartment(deptId);
            if (department == null)
            {
                throw new Exception("Department Id is not valid");
            }
            List<LeaveRecord> listOfLeaveRecord = _leaveRecordRepository.GetTotalLeavesInTeam(department);
            if (listOfLeaveRecord.Count.Equals(0))
            {
                return Ok("NO LEAVE TAKEN");
            }
            var response = new { listOfLeaveRecord, listOfLeaveRecord.Count };
            return Ok(response);
        }

        // GET api/LeaveReport/LeaveBalanceByEmpId/empId?empId=63
        [HttpGet]
        [Route("LeaveBalanceByEmpId/empId")]
        public IActionResult GetLeaveBalanceByEmpId(int empId)
        {
            int employeeId = 0;
            List<Department> listOfDepartment = _departmentRepository.GetAllDepartment();
            foreach (var department in listOfDepartment)
            {
                employeeId = department.Employees.Find(k => k.Equals(empId));
            }
            if (employeeId == 0)
            {
                throw new Exception("Employee Id is not valid");
            }
            LeaveBalance leaveBalance;
            leaveBalance = _leaveRecordRepository.GetLeaveBalanceByEmployee(empId);
            return Ok(leaveBalance);
        }

        // GET api/LeaveReport/LeaveBalanceByDeptId/deptId?deptId=13
        [HttpGet]
        [Route("LeaveBalanceByDeptId/deptId")]
        public IActionResult GetLeaveBalanceByDeptId(int deptId)
        {
            Department department = _departmentRepository.GetDepartment(deptId);
            if (department == null)
            {
                throw new Exception("Department Id is not valid");
            }
            List<LeaveBalance> listOfLeaveBalance;
            listOfLeaveBalance = _leaveRecordRepository.GetLeaveBalanceByDepartment(department);
            return Ok(listOfLeaveBalance);
        }

        // GET api/LeaveReport/LeaveBalanceOfAllEmp
        [HttpGet]
        [Route("LeaveBalanceOfAllEmp")]
        public IActionResult GetLeaveBalanceOfAllEmployee()
        {
            List<Department> listOfDepartment = _departmentRepository.GetAllDepartment();
            Dictionary<int, List<LeaveBalance>> listOfLeaveBalanceByDept;
            listOfLeaveBalanceByDept = _leaveRecordRepository.GetLeaveBalanceOfAllEmployee(listOfDepartment);
            return Ok(listOfLeaveBalanceByDept);
        }
    }
}