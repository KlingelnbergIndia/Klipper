using System;
using System.Collections.Generic;
using System.Text;

namespace Klipper.Desktop.Service.WorkTime.Policies
{
    public static class PolicyHelper
    {
        static public bool CheckGymnasiumUsage(WorkDay context, int hour)
        {
            var swipes = context.SwipesAtAccessPoint("Gym");
            if (swipes.Count == 0)
            {
                return true;
            }

            var validationStatus = true;

            var d = context.Date;

            var dayStart = new DateTime(d.Year, d.Month, d.Day, 9, 0, 0);
            var dayEnd = new DateTime(d.Year, d.Month, d.Day, hour, 0, 0);
            var grace = TimeSpan.FromMinutes(5);

            foreach (var s in swipes)
            {
                if (s.EventTime > (dayStart + grace) && s.EventTime < (dayEnd - grace))
                {
                    context.Violations.Add(new WorkTimeViolation(WorkTimeViolationType.TimeSlotViolation_GymnasiumUsedDuringWorkHours, swipes));
                    validationStatus = false;
                    break;
                }
            }

            return validationStatus;
        }

    }
}
