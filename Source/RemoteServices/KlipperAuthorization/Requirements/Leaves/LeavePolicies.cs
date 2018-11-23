using Microsoft.AspNetCore.Authorization;

namespace KlipperAuthorization.Requirements.Leaves
{
    public static class LeavePolicies
    {
        static public void Load(AuthorizationOptions options)
        {
            options.AddPolicy("ReadLeaves", p =>
            {
                p.AddAuthenticationSchemes("Bearer");
                p.RequireAuthenticatedUser();
                p.RequireRole("Employee");
                p.Requirements.Add(new ReadLeavesRequirement());
            }
            );
        }
    }
}
