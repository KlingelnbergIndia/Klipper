using System;
using System.Collections.Generic;

namespace Klipper.Desktop.Service.WorkTime.Policies.Default
{
    internal class TotalWorkHoursPerDayRule : IWorkTimeRule
    {
        public bool Validate(WorkDay context)
        {
            if(context.WorkTime < TimeSpan.FromHours(8.5))
            {
                context.Violations.Add(new WorkTimeViolation(WorkTimeViolationType.TimeDurationViolation_TotalDurationLessThan9Hours));
                return false;
            }
            return true;
        }
    }
}