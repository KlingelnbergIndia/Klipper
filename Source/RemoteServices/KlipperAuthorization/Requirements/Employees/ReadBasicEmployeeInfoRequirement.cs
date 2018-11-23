using Microsoft.AspNetCore.Authorization;

namespace KlipperAuthorization.Requirements.Employees
{
    internal class ReadBasicEmployeeInfoRequirement : IAuthorizationRequirement
    {
        public ReadBasicEmployeeInfoRequirement()
        {
        }
    }
}