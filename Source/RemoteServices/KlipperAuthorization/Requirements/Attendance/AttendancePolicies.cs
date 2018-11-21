using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace KlipperAuthorization.Requirements.Attendance
{
    public static class AttendancePolicies
    {
        static public void Load(AuthorizationOptions options)
        {
            options.AddPolicy("ReadAttendance", p =>
            {
                p.AddAuthenticationSchemes("Bearer");
                p.RequireAuthenticatedUser();
                p.RequireRole("Employee");
                p.Requirements.Add(new ReadAttendanceRequirement());
            }
            );
        }
    }
}
