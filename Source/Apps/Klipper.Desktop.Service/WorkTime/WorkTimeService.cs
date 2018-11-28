using Newtonsoft.Json;
using System;
using System.Net.Http;
using Models.Core.Employment;
using Klipper.Desktop.Service.Login;
using Common;
using Klipper.Desktop.Service.WorkTime;
using Klipper.Desktop.Service.WorkTime.Policies;
using Models.Core.Operationals;
using System.Collections.Generic;

namespace Klipper.Desktop.Service.WorkTime
{
    public class WorkTimeService
    {
        #region Instance

        static WorkTimeService _instance = null;

        public static WorkTimeService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WorkTimeService();
                }
                return _instance;
            }
        }

        public static void DeleteInstance()
        {
            _instance = null;
        }

        private WorkTimeService()
        {
        }

        #endregion

        #region Public methods

        public List<DateTime> GetHolidaysByYear(int year)
        {
            var client = CommonHelper.GetClient(AddressResolver.GetAddress("KlipperApi", false), Auth.SessionToken);
            var str = "api/worktime/holidays/" + year.ToString();

            HttpResponseMessage response = client.GetAsync(str).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                var holidays = JsonConvert.DeserializeObject<List<DateTime>>(jsonString);
                return holidays;
            }
            else
            {
                return null;
            }
        }

        #endregion

    }
}
