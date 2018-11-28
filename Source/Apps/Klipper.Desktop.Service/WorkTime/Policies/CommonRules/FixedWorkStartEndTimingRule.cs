using System;
using System.Linq;

namespace Klipper.Desktop.Service.WorkTime.Policies.CommonRules
{
    internal class FixedWorkStartEndTimingRule : IWorkTimeRule
    {
        public bool Validate(WorkDay context)
        {
            var events = context.AllAccessEvents.OrderBy(x => x.EventTime).ToList();
            if(events.Count < 2)
            {
                return false;
            }
            var first = events[0];
            var last = events[events.Count - 1];

            var d = context.Date;
            var idealDayStart = new DateTime(d.Year, d.Month, d.Day, 9, 0, 0);
            var idealDayEnd = new DateTime(d.Year, d.Month, d.Day, 18, 0, 0);
            var allowedLateTiming = new DateTime(d.Year, d.Month, d.Day, 19, 30, 0);
            var lateMostTiming = new DateTime(d.Year, d.Month, d.Day, 20, 30, 0);

            var validationStatus = true;

            if (first.EventTime > (idealDayStart + TimeSpan.FromMinutes(15.0)))
            {
                context.Violations.Add(WorkTimeViolation.GetViolation(WorkTimeViolationType.LateEntry));
                validationStatus = false;
            }
            if (last.EventTime < (idealDayEnd - TimeSpan.FromMinutes(15.0)))
            {
                context.Violations.Add(WorkTimeViolation.GetViolation(WorkTimeViolationType.EarlyLeaving));
                validationStatus = false;
            }
            if (context.WorkTime < TimeSpan.FromHours(8.5))
            {
                context.Violations.Add(WorkTimeViolation.GetViolation(WorkTimeViolationType.TimeDurationViolation_TotalDurationLessThan9Hours));
                validationStatus = false;
            }
            if (last.EventTime > allowedLateTiming)
            {
                context.Violations.Add(WorkTimeViolation.GetViolation(WorkTimeViolationType.StayingLate));
                validationStatus = false;
            }
            if (last.EventTime > lateMostTiming)
            {
                context.Violations.Add(WorkTimeViolation.GetViolation(WorkTimeViolationType.StayingVeryLate));
                validationStatus = false;
            }
            return validationStatus;
        }
    }
}