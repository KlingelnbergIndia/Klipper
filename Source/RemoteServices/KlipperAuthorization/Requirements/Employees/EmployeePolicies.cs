using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace KlipperAuthorization.Requirements.Employees
{
    public static class EmployeePolicies
    {
        static public void Load(AuthorizationOptions options)
        {
            options.AddPolicy("ReadBasicEmployeeInfo", p =>
            {
                p.AddAuthenticationSchemes("Bearer");
                p.RequireAuthenticatedUser();
                p.RequireRole("Employee");
                p.Requirements.Add(new ReadBasicEmployeeInfoRequirement());
            }
            );
        }
    }
}
