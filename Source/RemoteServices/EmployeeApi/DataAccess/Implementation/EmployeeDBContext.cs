using Models.Core.Employment;
using MongoDB.Driver;
using Common;

namespace EmployeeApi.DataAccess.Implementation
{
    public class EmployeeDBContext
    {
        protected readonly IMongoDatabase _database = null;

        #region Instance

        private static EmployeeDBContext _instance = null;

        public static EmployeeDBContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EmployeeDBContext();
                }
                return _instance;
            }
        }

        #endregion

        public EmployeeDBContext()
        {
            var connectionString = DBConfigurator.GetConnectionString("EmployeeDB");
            var mongoClient = new MongoClient(connectionString);
            _database = mongoClient.GetDatabase("EmployeeDB");
        }

        public IMongoCollection<Employee> Employees => _database.GetCollection<Employee>("Employees");
    }
}
