using Common.DataAccess;
using Models.Core.HR.Attendance;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AttendanceApi.DataAccess.Implementation
{
    public class AttendanceDBContext : DBContext
    {
        #region Instance

        public static AttendanceDBContext Instance { get; private set; } = null;

        public static AttendanceDBContext GetInstance(IOptions<DBConnectionSettings> settings)
        {
            if (Instance == null)
            {
                Instance = new AttendanceDBContext(settings);
            }
            return Instance;
        }

        #endregion

        public AttendanceDBContext(IOptions<DBConnectionSettings> settings) : base(settings) { }

        public IMongoCollection<AccessEvent> AccessEvents => _database.GetCollection<AccessEvent>("AccessEvents");
        public IMongoCollection<AccessPoint> AccessPoints => _database.GetCollection<AccessPoint>("AccessPoints");
    }
}
