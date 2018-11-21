using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace KlipperAuthorization.Requirements.Attendance
{
    internal class ReadAttendanceRequirementHandler : AuthorizationHandler<ReadAttendanceRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ReadAttendanceRequirement requirement)
        {
            var roleClaims = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();

            bool isHR = roleClaims.Where(c => c.Value == "HR").Count() > 0;
            bool isTeamLeader = roleClaims.Where(c => c.Value == "TeamLeader").Count() > 0;
            bool isEmployee = roleClaims.Where(c => c.Value == "Employee").Count() > 0;
            if(isHR)
            {
                //HR can see the attendance data for everybody
                context.Succeed(requirement);
            }
            else if(isTeamLeader)
            {
                //Team leader can only see the attendance data for his team reportees
                var loggedInUser = RequirementHelper.GetLoggedInUser(context);
                var employeeId = int.Parse(RequirementHelper.GetParameterVaueFromRequest(context, "employeeId"));
                if(employeeId != -1 && loggedInUser.IsReportee(employeeId))
                {
                    context.Succeed(requirement);
                }
            }
            else
            {
                //Regular employee can only see his own attendance data
                var loggedInUser = RequirementHelper.GetLoggedInUser(context);
                var employeeId = int.Parse(RequirementHelper.GetParameterVaueFromRequest(context, "employeeId"));
                if (employeeId != -1 && loggedInUser.ID == employeeId)
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }


    }
}