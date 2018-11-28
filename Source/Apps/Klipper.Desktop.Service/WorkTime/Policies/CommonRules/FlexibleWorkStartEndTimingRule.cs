using System;
using System.Collections.Generic;
using System.Linq;

namespace Klipper.Desktop.Service.WorkTime.Policies.CommonRules
{
    internal class FlexibleWorkStartEndTimingRule : IWorkTimeRule
    {
        public bool Validate(WorkDay context)
        {
            var events = context.AllAccessEvents.OrderBy(x => x.EventTime).ToList();
            if (events.Count < 2)
            {
                return false;
            }
            var first = events[0];
            var last = events[events.Count - 1];

            var d = context.Date;
            var allowedDayStart = new DateTime(d.Year, d.Month, d.Day, 9, 0, 0);
            var allowedLateTiming = new DateTime(d.Year, d.Month, d.Day, 19, 30, 0);
            var lateMostTiming = new DateTime(d.Year, d.Month, d.Day, 20, 30, 0);

            var validationStatus = true;

            if (first.EventTime > allowedDayStart)
            {
                context.Violations.Add(WorkTimeViolation.GetViolation(WorkTimeViolationType.LateEntry));
                validationStatus = false;
            }
            if (context.WorkTime < TimeSpan.FromHours(6.0))
            {
                context.Violations.Add(WorkTimeViolation.GetViolation(WorkTimeViolationType.TimeDurationViolation_TotalDurationLessThan6Hours));
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