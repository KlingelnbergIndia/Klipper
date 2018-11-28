using System;
using System.Collections.Generic;

namespace Klipper.Desktop.Service.WorkTime.Policies.CommonRules
{
    internal class TotalLunchDurationRule : IWorkTimeRule
    {
        public bool Validate(WorkDay context)
        {


            return true;
        }
    }
}