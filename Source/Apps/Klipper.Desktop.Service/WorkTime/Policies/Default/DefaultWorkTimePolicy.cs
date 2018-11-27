using System;
using System.Collections.Generic;
using System.Linq;
using Models.Core.HR.Attendance;

namespace Klipper.Desktop.Service.WorkTime.Policies.Default
{
    public abstract class DefaultWorkTimePolicy : BaseWorkTimePolicy
    {
        public DefaultWorkTimePolicy()
        {
            PopulateRules();
        }

        protected override void PopulateRules()
        {
            Rules.Add("WorkStartEndTimingRule", new WorkStartEndTimingRule());
            Rules.Add("TotalWorkHoursPerDayRule", new TotalWorkHoursPerDayRule());
            Rules.Add("TotalLunchDurationRule", new TotalLunchDurationRule());
            Rules.Add("GymnasiumUsageRule", new GymnasiumUsageRule());
            Rules.Add("RecreationUsageRule", new RecreationUsageRule());
            Rules.Add("OddAccessEventsRule", new OddAccessEventsRule());
        }


    }
}

