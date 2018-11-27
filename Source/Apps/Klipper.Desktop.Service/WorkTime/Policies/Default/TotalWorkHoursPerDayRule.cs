using System;

namespace Klipper.Desktop.Service.WorkTime.Policies.Default
{
    internal class TotalWorkHoursPerDayRule : IWorkTimeRule
    {
        public Tuple<bool, WorkTimeViolation, object> Validate(WorkDay context)
        {


            return null;
        }
    }
}