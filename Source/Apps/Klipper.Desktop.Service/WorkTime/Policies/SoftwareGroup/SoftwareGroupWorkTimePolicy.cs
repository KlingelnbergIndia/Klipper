using System;
using System.Collections.Generic;
using System.Linq;
using Models.Core.HR.Attendance;

namespace Klipper.Desktop.Service.WorkTime.Policies.SoftwareGroup
{
    public class SoftwareGroupWorkTimePolicy : BaseWorkTimePolicy
    {
        public SoftwareGroupWorkTimePolicy()
        {
            PopulateRules();
        }

        protected override void PopulateRules()
        {
            base.PopulateRules();

            //Override following rules for the software group
            Rules[WorkTimeRules.WorkStartEndTimingRule] = typeof(Klipper.Desktop.Service.WorkTime.Policies.CommonRules.FlexibleWorkStartEndTimingRule);
            Rules[WorkTimeRules.TotalWorkHoursPerDayRule] = typeof(Klipper.Desktop.Service.WorkTime.Policies.SoftwareGroup.TotalWorkHoursPerDayRule);
            Rules[WorkTimeRules.GymnasiumUsageRule] = typeof(Klipper.Desktop.Service.WorkTime.Policies.SoftwareGroup.GymnasiumUsageRule);
        }

        public override bool IsWeekend(DateTime date)
        {
            var dayOfWeek = date.DayOfWeek;
            if (dayOfWeek == DayOfWeek.Sunday || dayOfWeek == DayOfWeek.Saturday)
            {
                return true;
            }
            return false;
        }

    }
}

