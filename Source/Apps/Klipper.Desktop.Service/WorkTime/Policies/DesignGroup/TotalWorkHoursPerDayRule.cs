using System;
using System.Collections.Generic;

namespace Klipper.Desktop.Service.WorkTime.Policies.DesignGroup
{
    internal class TotalWorkHoursPerDayRule : IWorkTimeRule
    {
        public bool Validate(WorkDay context)
        {
            if (context.WorkTime < TimeSpan.FromHours(6))
            {
                context.Violations.Add(new WorkTimeViolation(WorkTimeViolationType.TimeDurationViolation_TotalDurationLessThan9Hours));
                return false;
            }
            return true;
        }
    }
}