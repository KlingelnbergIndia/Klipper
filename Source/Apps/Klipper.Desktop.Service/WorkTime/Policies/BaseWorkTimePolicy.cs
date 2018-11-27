using System;
using System.Collections.Generic;
using System.Linq;
using Models.Core.HR.Attendance;

namespace Klipper.Desktop.Service.WorkTime.Policies
{
    public abstract class BaseWorkTimePolicy : IWorkTimePolicy
    {
        protected WorkDay _workdayContext = null;
        public Dictionary<string, IWorkTimeRule> Rules { get; private set; } = new Dictionary<string, IWorkTimeRule>();

        public BaseWorkTimePolicy()
        {
            PopulateRules();
        }

        public IWorkTimeRule GetRule(string ruleName)
        {
            if(Rules.Keys.Contains(ruleName))
            {
                return Rules[ruleName];
            }
            return null;
        }

        public void SetWorkdayContext(WorkDay workDay)
        {
            _workdayContext = workDay;
        }

        public Tuple<bool, WorkTimeViolation, object> Validate(string ruleName)
        {
            IWorkTimeRule rule = GetRule(ruleName);
            if(rule == null)
            {
                return null;
            }
            return rule.Validate(_workdayContext);
        }

        protected abstract void PopulateRules();


    }
}

