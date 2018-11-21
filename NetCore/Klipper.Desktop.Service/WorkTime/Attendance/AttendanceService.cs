﻿using Klipper.Desktop.Service.Login;
using Models.Core.HR.Attendance;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        #region Fields

        HttpClient _httpClient = new HttpClient() { BaseAddress = new Uri("http://localhost:7000/") };

        #endregion

        #region Public methods

        public IEnumerable<AccessEvent> GetAccessEvents(int employeeId, DateTime startDate, DateTime endDate)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer", Auth.SessionToken);

            var startStr = startDate.Year.ToString() + "-" + startDate.Month.ToString() + "-" + startDate.Day.ToString();
            var endStr = endDate.Year.ToString() + "-" + endDate.Month.ToString() + "-" + endDate.Day.ToString();
            var str = "api/attendance/" + employeeId.ToString() + "/" + startStr + "/" + endStr;

            HttpResponseMessage response = _httpClient.GetAsync(str).Result;
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
        
        #endregion


    }
}