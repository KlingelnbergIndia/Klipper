using System;

namespace Klipper.Desktop.Service.WorkTime.Policies.SoftwareGroup
{
    internal class OddAccessEventsRule : IWorkTimeRule
    {
        public Tuple<bool, WorkTimeViolation, object> Validate(WorkDay context)
        {
            throw new NotImplementedException();
        }
    }
}