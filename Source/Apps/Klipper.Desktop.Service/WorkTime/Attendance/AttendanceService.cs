using Common;
using Klipper.Desktop.Service.Login;
using Models.Core.HR.Attendance;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Klipper.Desktop.Service.WorkTime.Attendance
{
    public class AttendanceService
    {

        #region Instance

        static AttendanceService _instance = null;

        public static AttendanceService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AttendanceService();
                }
                return _instance;
            }
        }

        public static void DeleteInstance()
        {
            _instance = null;
        }

        private AttendanceService()
        {
        }

        #endregion

        #region Public methods

        public IEnumerable<AccessEvent> GetAccessEvents(int employeeId, DateTime startDate, DateTime endDate)
        {
            var client = CommonHelper.GetClient(AddressResolver.GetAddress("KlipperApi", false), Auth.SessionToken);

            var startStr = startDate.Year.ToString() + "-" + startDate.Month.ToString() + "-" + startDate.Day.ToString();
            var endStr = endDate.Year.ToString() + "-" + endDate.Month.ToString() + "-" + endDate.Day.ToString();
            var str = "api/attendance/" + employeeId.ToString() + "/" + startStr + "/" + endStr;

            HttpResponseMessage response = client.GetAsync(str).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                var accessEvents = JsonConvert.DeserializeObject<IEnumerable<AccessEvent>>(jsonString);
                return accessEvents;
            }
            else
            {
                return new List<AccessEvent>();
            }
        }

        public IEnumerable<AccessEvent> GetAccessEvents(int employeeId, DateTime date)
        {
            var client = CommonHelper.GetClient(AddressResolver.GetAddress("KlipperApi", false), Auth.SessionToken);

            var startStr = date.Year.ToString() + "-" + date.Month.ToString() + "-" + date.Day.ToString();
            var endStr = startStr;
            var str = "api/attendance/" + employeeId.ToString() + "/" + startStr + "/" + endStr;

            HttpResponseMessage response = client.GetAsync(str).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                var accessEvents = JsonConvert.DeserializeObject<IEnumerable<AccessEvent>>(jsonString);
                return accessEvents;
            }
            else
            {
                return new List<AccessEvent>();
            }
        }

        public List<WorkDay> GetWorkdays(int employeeId, DateTime startDate, DateTime endDate)
        {
            List<WorkDay> workDays = new List<WorkDay>();

            var accessEvents = GetAccessEvents(employeeId, startDate, endDate);
            var totalDays = (endDate - startDate).Days + 1;
            var startDay = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0);
            for(var i = 0; i < totalDays; i++)
            {
                var day = startDate.AddDays(i);
                var dayAccessEvents = accessEvents.Where(x => 
                (
                    x.EventTime.Year == day.Year &&
                    x.EventTime.Month == day.Month &&
                    x.EventTime.Day == day.Day
                )).ToList();

                var workDay = WorkDay.GetWorkDay(employeeId, day, dayAccessEvents);
                workDays.Add(workDay);
            }

            return workDays;
        }

        #endregion
    }
}
