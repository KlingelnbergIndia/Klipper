using Models.Core.HR.Attendance;
using MongoDB.Driver;
using Common;

namespace AttendanceApi.DataAccess.Implementation
{
    public class AttendanceDBContext
    {
        protected readonly IMongoDatabase _database = null;

        #region Instance

        private static AttendanceDBContext _instance = null;

        public static AttendanceDBContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AttendanceDBContext();
                }
                return _instance;
            }
        }

        #endregion

        public AttendanceDBContext()
        {
            var connectionString = DBConfigurator.GetConnectionString("AttendanceDB");
            var mongoClient = new MongoClient(connectionString);
            _database = mongoClient.GetDatabase("AttendanceDB");
        }

        public IMongoCollection<AccessEvent> AccessEvents => _database.GetCollection<AccessEvent>("AccessEvents");
        public IMongoCollection<AccessPoint> AccessPoints => _database.GetCollection<AccessPoint>("AccessPoints");
    }
}
