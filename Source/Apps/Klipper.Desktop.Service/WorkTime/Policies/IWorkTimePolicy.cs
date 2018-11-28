using System;
using System.Collections.Generic;

namespace Klipper.Desktop.Service.WorkTime.Policies
{
    public interface IWorkTimePolicy
    {
        IWorkTimeRule GetRule(string ruleName);

        bool Validate(string ruleName);

        void SetWorkdayContext(WorkDay workDay);

        bool IsWeekend(DateTime day);
    }
}
