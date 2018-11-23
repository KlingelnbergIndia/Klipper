using KlipperAuthorization.Requirements.Attendance;
using KlipperAuthorization.Requirements.Employees;
using KlipperAuthorization.Requirements.Leaves;
using Microsoft.AspNetCore.Authorization;

namespace KlipperAuthorization
{
    public class AuthorizationPolicyLoader
    {
        public AuthorizationPolicyLoader()
        {
        }

        public void Load(AuthorizationOptions options)
        {
            AttendancePolicies.Load(options);
            EmployeePolicies.Load(options);
            LeavePolicies.Load(options);
        }
    }
}
