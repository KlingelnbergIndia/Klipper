using Common.DataAccess;
using Microsoft.Extensions.Options;
using Models.Core.Employment;
using Models.Core.HR.Attendance;
using Models.Core.Operationals;
using MongoDB.Driver;

namespace LeaveReportApi.LeaveDataAccess.Repository
{
    public class LeaveDbContext 
    {
        protected readonly IMongoDatabase _database = null;
        public LeaveDbContext(IOptions<DBConnectionSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
            {
                _database = client.GetDatabase(settings.Value.Database);
            }
        }
        public static LeaveDbContext Instance { get; private set; } = null;

        public static LeaveDbContext GetInstance(IOptions<DBConnectionSettings> settings)
        {
          
            if (Instance == null)
            {
                Instance = new LeaveDbContext(settings);
            }
            return Instance;
        }
       
        
     
        public IMongoCollection<Leave> LeaveCollection => _database.GetCollection<Leave>("Leave");
        
        
       // public IMongoCollection<Department> DepartmentCollection => _database.GetCollection<Department>("Departments");
       // public IMongoCollection<Employee> EmployeesCollection => _database.GetCollection<Employee>("Employees");

    }
}
