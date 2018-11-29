using Models.Core.Employment;
using Models.Core.Operationals;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Common;

namespace OperationalsApi.DataAccess.Implementation
{
    public class DepartmentDBContext
    {
        protected readonly IMongoDatabase _database = null;

        #region Instance

        private static DepartmentDBContext _instance = null;

        public static DepartmentDBContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DepartmentDBContext();
                }
                return _instance;
            }
        }

        #endregion

        public DepartmentDBContext()
        {
            var connectionString = DBConfigurator.GetConnectionString("OperationalsDB");
            var mongoClient = new MongoClient(connectionString);
            _database = mongoClient.GetDatabase("OperationalsDB");
        }

        public IMongoCollection<Department> Departments => _database.GetCollection<Department>("Departments");
    }
}
