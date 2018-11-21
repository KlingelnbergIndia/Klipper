using Microsoft.AspNetCore.Authorization;

namespace KlipperAuthorization.Requirements.Attendance
{
    internal class ReadAttendanceRequirement : IAuthorizationRequirement
    {
        public ReadAttendanceRequirement()
        {
        }
    }
}