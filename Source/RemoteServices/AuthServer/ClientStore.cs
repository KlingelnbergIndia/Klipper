using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Configuration;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace AuthServer
{
    public class ClientStore : IdentityServer4.Stores.IClientStore
    {
        List<Client> _clients = new List<Client>();

        public ClientStore(IConfiguration configuration)
        {
            _clients = (List<Client>) ConfigReader.GetClients(configuration);
        }

        public Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = _clients.Where(c => c.ClientId == clientId).FirstOrDefault();
            return Task.FromResult(client);
        }
    }
}
