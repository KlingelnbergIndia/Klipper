using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Common;
using Models.Core.HR.Attendance;
using Newtonsoft.Json;

namespace KlipperApi.Controllers.Attendance
{
    public class AttendanceAccessor : IAttendanceAccessor
    {
        public async Task<IEnumerable<AccessEvent>> GetAttendanceByEmployeeIdAsync(int employeeId)
        {
            var client = CommonHelper.GetClient(AddressResolver.GetAddress("AttendanceApi", false));
            var str = "api/accessevents/byEmployeeId?employeeId=" + employeeId.ToString();
            HttpResponseMessage response = await client.GetAsync(str);
            var jsonString = await response.Content.ReadAsStringAsync();
            var accessEvents = JsonConvert.DeserializeObject<List<AccessEvent>>(jsonString);

            return accessEvents;
        }
    }
}
