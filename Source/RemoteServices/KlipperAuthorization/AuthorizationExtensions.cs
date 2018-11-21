using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using KlipperAuthorization.Requirements.Attendance;
using KlipperAuthorization.Requirements.Leaves;

namespace KlipperAuthorization
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddAuthorizationPolicyRequirements(this IServiceCollection services)
        {
            services.AddTransient<IAuthorizationHandler, ReadAttendanceRequirementHandler>();
            services.AddTransient<IAuthorizationHandler, ReadLeavesRequirementHandler>();
            return services;
        }

    }
}
