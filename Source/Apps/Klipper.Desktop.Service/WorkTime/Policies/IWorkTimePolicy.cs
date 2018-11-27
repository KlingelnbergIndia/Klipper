using System;
using System.Collections.Generic;

namespace Klipper.Desktop.Service.WorkTime.Policies
{
    public interface IWorkTimePolicy
    {

        Dictionary<string, IWorkTimeRule> Rules { get; }

        IWorkTimeRule GetRule(string ruleName);

        Tuple<bool, WorkTimeViolation, object> Validate(string ruleName);

        void SetWorkdayContext(WorkDay workDay);
    }
}
