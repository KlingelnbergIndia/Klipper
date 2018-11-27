using System;

namespace Klipper.Desktop.Service.WorkTime.Policies.DesignGroup
{
    internal class RecreationUsageRule : IWorkTimeRule
    {
        public Tuple<bool, WorkTimeViolation, object> Validate(WorkDay context)
        {
            throw new NotImplementedException();
        }
    }
}