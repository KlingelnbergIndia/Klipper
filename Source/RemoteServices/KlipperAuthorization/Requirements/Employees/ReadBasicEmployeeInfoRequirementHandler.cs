using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace KlipperAuthorization.Requirements.Employees
{
    internal class ReadBasicEmployeeInfoRequirementHandler : AuthorizationHandler<ReadBasicEmployeeInfoRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ReadBasicEmployeeInfoRequirement requirement)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }


    }
}