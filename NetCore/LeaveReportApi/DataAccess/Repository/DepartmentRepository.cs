using Common.DataAccess;
using LeaveReportApi.DataAccess.Services;
using LeaveReportApi.LeaveDataAccess.Repository;
using Microsoft.Extensions.Options;
using Models.Core.Operationals;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveReportApi.DataAccess.Repository
{
    public class DepartmentRepository:IDepartmentRepository
    {
        private DeptDbContext _context;

        public DepartmentRepository(IOptions<DBConnectionSettings> settings)
        {
            _context = DeptDbContext.GetInstance(settings);
        }
        public Department GetDepartment(int deptId)
        {
            return _context.DepartmentCollection.Find(k => k.ID.Equals(deptId)).FirstOrDefault();
        }
    }
}
