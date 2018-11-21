using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KlipperApi.Controllers.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Core.HR.Attendance;

namespace KlipperApi.Controllers.Attendance
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAttendanceAccessor _attendanceAccessor;
        private readonly IAuthorizationService _authorizationService;

        public AttendanceController(
            IAuthorizationService authorizationService,
            IUserRepository userRepository, 
            IAttendanceAccessor attendanceAccessor)
        {
            _authorizationService = authorizationService;
            _userRepository = userRepository;
            _attendanceAccessor = attendanceAccessor;
        }

        [Route("{employeeId}/{startDate}/{endDate}")]
        [HttpGet]
        [Authorize(Policy = "ReadAttendance")]
        public async Task<IActionResult> Get(int employeeId, string startDate, string endDate)
        {
            var start = DateTime.Parse(startDate);
            var end = DateTime.Parse(endDate);
            var accessEvents = await _attendanceAccessor.GetAttendanceByEmployeeIdAsync(employeeId) as List<AccessEvent>;
            var filteredEvents = accessEvents
                .Where(e => e.EventTime >= start && e.EventTime <= end)
                .ToList();

            return Ok(filteredEvents);
        }
    }
}
