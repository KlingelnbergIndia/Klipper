using System;
using System.Collections.Generic;
using System.Linq;

namespace Klipper.Desktop.Service.WorkTime.Policies.CommonRules
{
    internal class RecreationUsageRule : IWorkTimeRule
    {
        public bool Validate(WorkDay context)
        {
            var swipes = context.SwipesAtAccessPoint("Recreation");
            if(swipes.Count == 0)
            {
                return true;
            }

            var validationStatus = true;

            if (context.OutsidePremisesTimeSpan + context.RecreationTimeSpan > TimeSpan.FromMinutes(45))
            {
                context.Violations.Add(new WorkTimeViolation(WorkTimeViolationType.TimeDurationViolation_RecreationLunchTimeMoreThan45Min, swipes));
            }

            var d = context.Date;

            var lunchSlotStart = new DateTime(d.Year, d.Month, d.Day, 13, 0, 0);
            var lunchSlotEnd = new DateTime(d.Year, d.Month, d.Day, 14, 0, 0);
            var dayEnd = new DateTime(d.Year, d.Month, d.Day, 18, 0, 0);
            var grace = TimeSpan.FromMinutes(5);

            foreach (var s in swipes)
            {
                if (s.EventTime < (lunchSlotStart - grace))
                {
                    context.Violations.Add(new WorkTimeViolation(WorkTimeViolationType.TimeSlotViolation_RecreationLunchTime, swipes));
                    validationStatus = false;
                    break;
                }
                else if (s.EventTime > (lunchSlotEnd + grace) && s.EventTime < dayEnd)
                {
                    context.Violations.Add(new WorkTimeViolation(WorkTimeViolationType.TimeSlotViolation_RecreationLunchTime, swipes));
                    validationStatus = false;
                    break;
                }
            }

            return validationStatus;
        }
    }
}



