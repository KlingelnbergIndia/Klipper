using System;

namespace Klipper.Desktop.Service.WorkTime.Policies
{
    public interface IWorkTimeRule
    {
        Tuple<bool, WorkTimeViolation, object> Validate(WorkDay context);
    }
}