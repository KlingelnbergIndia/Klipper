using System;

namespace Klipper.Desktop.Service.WorkTime.Policies.Default
{
    internal class OddAccessEventsRule : IWorkTimeRule
    {
        public Tuple<bool, WorkTimeViolation, object> Validate(WorkDay context)
        {


            return null;
        }
    }
}