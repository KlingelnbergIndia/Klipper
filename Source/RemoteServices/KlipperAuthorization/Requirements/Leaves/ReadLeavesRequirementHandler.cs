using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace KlipperAuthorization.Requirements.Leaves
{
    internal class ReadLeavesRequirementHandler : AuthorizationHandler<ReadLeavesRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ReadLeavesRequirement requirement)
        {
            //To be implemented...
            return Task.CompletedTask;
        }
    }
}
