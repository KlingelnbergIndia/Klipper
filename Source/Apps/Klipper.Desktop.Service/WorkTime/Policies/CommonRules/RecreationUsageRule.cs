using System;
using System.Collections.Generic;

namespace Klipper.Desktop.Service.WorkTime.Policies.CommonRules
{
    internal class RecreationUsageRule : IWorkTimeRule
    {
        public bool Validate(WorkDay context)
        {

            //if (GymnasiumDuration > TimeSpan.FromMinutes(45))
            //{
            //    FlaggedForViolation = true;
            //    Violations.Add(new WorkTimeViolation(WorkTimeViolationType.TimeDurationViolation_RecreationLunchTimeMoreThan45Min, swipes));
            //}

            return true;
        }
    }
}
