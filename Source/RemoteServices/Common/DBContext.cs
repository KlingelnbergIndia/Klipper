using Models.Core.HR.Attendance;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Common.DataAccess
{
    public class DBContext
    {
        protected readonly IMongoDatabase _database = null;

        public DBContext(IOptions<DBConnectionSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
            {
                _database = client.GetDatabase(settings.Value.Database);
            }
        }
    }
}
