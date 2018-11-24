using Common;
using Models.Core.Authentication;
using MongoDB.Driver;

namespace KlipperApi.Controllers.Auth
{
    public class AuthContext
    {
        private readonly IMongoDatabase _database = null;
        private static AuthContext _instance = null;

        public AuthContext()
        {
            var connectionString = DBConfigurator.GetConnectionString("AuthDB");
            var mongoClient = new MongoClient(connectionString);
            _database = mongoClient.GetDatabase("AuthDB");
        }

        public static AuthContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    var context = new AuthContext();
                    _instance = context;
                }
                return _instance;
            }
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    }
}
