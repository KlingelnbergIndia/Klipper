using KlipperAuthorization.Requirements.Attendance;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;

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
        }
    }
}
