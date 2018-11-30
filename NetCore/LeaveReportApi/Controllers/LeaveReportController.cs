using LeaveReportApi.DataAccess.Services;
using LeaveReportApi.LeaveReportDataAccess.Services;
using Microsoft.AspNetCore.Mvc;
using Models.Core.HR.Attendance;
using Models.Core.Operationals;
using System;
using System.Collections.Generic;

namespace EmployeeApi.Controllers
{
    [Route("api/[controller]")]
    public class LeaveReportController : Controller
    {
        private ILeaveRecordService _leaveReportService;
        private IDepartmentRepository _departmentRepository;

        public LeaveReportController(ILeaveRecordService leaveRepository1, IDepartmentRepository departmentRepository)
        {
            _leaveReportService = leaveRepository1;
            _departmentRepository = departmentRepository;
        }

        // GET api/LeaveReport/TotalLeave
        [HttpGet]
        [Route("TotalLeave")]
        public IActionResult GetTotalLeave()
        {
            List<LeaveRecord> list = _leaveReportService.LeavesForAllEmployee();
            return Ok(list);
        }

        // GET api/LeaveReport/empId?empId=63
        [HttpGet]
        [Route("empId")]
        public IActionResult GetTotalLeaveByEmpId(int empId)
        {
            //if (!_employeeRepository.Exists(empId))
            //{
            //        throw new Exception("Employee Id is not valid");
            // }
            var result = _leaveReportService.TotalLeaveByEmpId(empId);
            return Ok(result);
        }

        // GET api/LeaveReport/deptId?deptId=13
        [HttpGet]
        [Route("deptId")]
        public IActionResult GetTotalLeaveInTeam(int deptId)
        {
            Department department = _departmentRepository.GetDepartment(deptId);

            if (department == null)
            {
                throw new Exception("Department Id is not valid");
            }

            List<LeaveRecord> listOfRecord = _leaveReportService.TotalLeaveInTeam(department);

            if (listOfRecord.Count.Equals(0))
            {
                return Ok("NO LEAVE TAKEN");
            }
            var data = new { listOfRecord, listOfRecord.Count };
            return Ok(data);

        }

        // GET api/LeaveReport/CheckBalance/empId?empId=63
        [HttpGet]
        [Route("CheckBalance/empId")]
        public IActionResult GetBalance(int empId)
        {
            Dictionary< string,int> dictionary ;
            dictionary = _leaveReportService.GetCheckBalance(empId);
            return Ok(dictionary);
        }
    }
}