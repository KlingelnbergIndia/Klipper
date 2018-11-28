using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Klipper.Desktop.Service.WorkTime.Policies
{
    public class BaseWorkTimePolicy : IWorkTimePolicy
    {
        protected WorkDay _workdayContext = null;

        protected Dictionary<string, Type> Rules { get; private set; } = new Dictionary<string, Type>();

        public BaseWorkTimePolicy()
        {
            PopulateRules();
        }

        public IWorkTimeRule GetRule(string ruleName)
        {
            if(Rules.Keys.Contains(ruleName))
            {
                var type = Rules[ruleName];
                ConstructorInfo emptyConstructor = type.GetConstructor(Type.EmptyTypes);
                var obj = emptyConstructor.Invoke(new object[] { });
                return (IWorkTimeRule) type;
            }
            return null;
        }

        public void SetWorkdayContext(WorkDay workDay)
        {
            _workdayContext = workDay;
        }

        public bool Validate(string ruleName)
        {
            IWorkTimeRule rule = GetRule(ruleName);
            if(rule == null)
            {
                return false;
            }
            return rule.Validate(_workdayContext);
        }

        protected virtual void PopulateRules()
        {
            Rules.Add(WorkTimeRules.WorkStartEndTimingRule, typeof(Klipper.Desktop.Service.WorkTime.Policies.CommonRules.FixedWorkStartEndTimingRule));
            Rules.Add(WorkTimeRules.TotalWorkHoursPerDayRule, typeof(Klipper.Desktop.Service.WorkTime.Policies.Default.TotalWorkHoursPerDayRule));
            Rules.Add(WorkTimeRules.GymnasiumUsageRule, typeof(Klipper.Desktop.Service.WorkTime.Policies.Default.GymnasiumUsageRule));
            Rules.Add(WorkTimeRules.RecreationUsageRule, typeof(Klipper.Desktop.Service.WorkTime.Policies.CommonRules.RecreationUsageRule));
        }

        public virtual bool IsWeekend(DateTime date)
        {
            var dayOfWeek = date.DayOfWeek;
            if (dayOfWeek == DayOfWeek.Sunday)
            {
                return true;
            }
            var year = date.Year;
            var month = date.Month;
            var totalDaysInMonth = DateTime.DaysInMonth(year, month);
            var allSaturdays = Enumerable.Range(1, totalDaysInMonth)
                        .Select(day => new DateTime(year, month, day))
                        .Where(d => d.DayOfWeek == DayOfWeek.Saturday)
                        .ToList<DateTime>();
            var secondSaturday = allSaturdays[1];
            var fourthSaturday = allSaturdays[3];

            if (date.Year == secondSaturday.Year &&
               date.Month == secondSaturday.Month &&
               date.Day == secondSaturday.Day)
            {
                return true;
            }
            if (date.Year == fourthSaturday.Year &&
               date.Month == fourthSaturday.Month &&
               date.Day == fourthSaturday.Day)
            {
                return true;
            }
            return false;
        }

    }
}

