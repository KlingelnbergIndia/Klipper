using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace AuthServer.Configuration
{
    public class ConfigReader
    {
        internal static IEnumerable<ApiResource> GetApiResources(IConfiguration configuration)
        {
            var apis = new List<ApiResource>();
            var section = configuration.GetSection("ApiProfile:Apis");
            foreach(var a in section.GetChildren())
            {
                var name = a.GetSection("name").Value;
                var displayName = a.GetSection("DisplayName").Value;
                apis.Add(new ApiResource(name, displayName));
            }
            return apis;
        }

        internal static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            var clients = new List<Client>();
            var section = configuration.GetSection("ClientProfile:Clients");
            foreach (var a in section.GetChildren())
            {
                var id = a.GetSection("Id").Value;
                var name = a.GetSection("Name").Value;
                var secret = new Secret(a.GetSection("Secret").Value.Sha256());
                var allowedScopeSection = a.GetSection("AllowedScopes");
                var scopes = new List<string>();
                foreach(var s in allowedScopeSection.GetChildren())
                {
                    scopes.Add(s.Value);
                }
                var c = new Client()
                {
                    ClientId = id,
                    ClientName = name,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { secret },
                    AllowedScopes = scopes
                };
                clients.Add(c);
            }
            return clients;
        }

    }
}
