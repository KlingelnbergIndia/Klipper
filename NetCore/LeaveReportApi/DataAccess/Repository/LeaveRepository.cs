using Common.DataAccess;
using LeaveReportApi.LeaveDataAccess.Repository;
using LeaveReportApi.LeaveReportDataAccess.Services;
using Microsoft.Extensions.Options;
using Models.Core.HR.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveReportApi.LeaveReportDataAccess.Repository
{
    public class LeaveRepository:ILeaveService
    {
        public DbContext _context;

        public LeaveRepository(IOptions<DBConnectionSettings> settings)
        {
            _context = DbContext.GetInstance(settings);
        }
        public void AddLeave(Leave leave)
        {
            _context.LeaveCollection.InsertOne(leave);
        }
    }
}
