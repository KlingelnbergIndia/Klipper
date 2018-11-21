using Microsoft.AspNetCore.Authorization;
using Models.Core.Employment;
using System;
using System.Linq;
using System.Security.Claims;

namespace KlipperAuthorization.Requirements
{
    internal static class RequirementHelper
    {
        public static Employee GetLoggedInUser(AuthorizationHandlerContext context)
        {
            var claims = context.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).ToList();
            if (claims.Count() > 0)
            {
                var claim = claims.First();
                var username = claim.Value;
                if (SessionCache.Employees.Keys.Contains(username))
                {
                    return SessionCache.Employees[username];
                }
            }
            return null;
        }

        public static string GetParameterVaueFromRequest(AuthorizationHandlerContext context, string parameterTag)
        {
            var mvcContext = context.Resource as
                        Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext;
            var containsParameter = mvcContext.RouteData.Values.ContainsKey(parameterTag);
            if (containsParameter)
            {
                return mvcContext.RouteData.Values[parameterTag].ToString();
            }
            return string.Empty;
        }

    }
}
