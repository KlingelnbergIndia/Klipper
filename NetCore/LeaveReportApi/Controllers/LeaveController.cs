using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveReportApi.LeaveReportDataAccess.Repository;
using LeaveReportApi.LeaveReportDataAccess.Services;
using Microsoft.AspNetCore.Mvc;
using Models.Core.HR.Attendance;

namespace LeaveReportApi.Controllers
{
    [Route("api/[controller]")]
    public class LeaveController : Controller
    {
        private ILeaveRepositorycs _repository;

        public LeaveController(ILeaveRepositorycs repository)
        {
            _repository = repository;
        }

        //POST api/Leave
        [HttpPost]
        public IActionResult Add([FromBody] Leave leave)
        {
            if (leave == null)
            {
                return BadRequest();
            }
            _repository.AddLeave(leave);
            return Ok("data is inserted");
        }
    }
}