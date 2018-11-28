using System;
using System.Collections.Generic;

namespace Klipper.Desktop.Service.WorkTime.Policies.DesignGroup
{
    internal class GymnasiumUsageRule : IWorkTimeRule
    {
        public bool Validate(WorkDay context)
        {
            return PolicyHelper.CheckGymnasiumUsage(context, 17);
        }
    }
}