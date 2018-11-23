using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using KlipperAuthorization.Requirements.Attendance;
using KlipperAuthorization.Requirements.Leaves;
using KlipperAuthorization.Requirements.Employees;

namespace KlipperAuthorization
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddAuthorizationPolicyRequirements(this IServiceCollection services)
        {
            services.AddTransient<IAuthorizationHandler, ReadAttendanceRequirementHandler>();
            services.AddTransient<IAuthorizationHandler, ReadLeavesRequirementHandler>();
            services.AddTransient<IAuthorizationHandler, ReadBasicEmployeeInfoRequirementHandler>();
            return services;
        }

    }
}
