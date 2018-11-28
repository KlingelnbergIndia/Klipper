using System;
using System.Collections.Generic;
using System.Linq;
using Models.Core.HR.Attendance;

namespace Klipper.Desktop.Service.WorkTime.Policies.DesignGroup
{
    public class DesignGroupWorkTimePolicy : BaseWorkTimePolicy
    {
        public DesignGroupWorkTimePolicy():base() {}

        protected override void PopulateRules()
        {
            base.PopulateRules();

            //Override following rules for the design group
            Rules[WorkTimeRules.WorkStartEndTimingRule] = typeof(Klipper.Desktop.Service.WorkTime.Policies.CommonRules.FlexibleWorkStartEndTimingRule);
            Rules[WorkTimeRules.TotalWorkHoursPerDayRule] = typeof(Klipper.Desktop.Service.WorkTime.Policies.DesignGroup.TotalWorkHoursPerDayRule);
            Rules[WorkTimeRules.GymnasiumUsageRule] = typeof(Klipper.Desktop.Service.WorkTime.Policies.DesignGroup.GymnasiumUsageRule);
        }

    }
}

