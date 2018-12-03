using Models.Core.Operationals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveReportApi.DataAccess.Services
{
    public interface IDepartmentRepository
    {
        Department GetDepartment(int deptid);
        List<Department> GetAllDepartment();
    }
}
