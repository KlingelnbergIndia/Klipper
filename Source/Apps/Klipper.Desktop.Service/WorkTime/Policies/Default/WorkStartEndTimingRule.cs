using System;
using System.Collections.Generic;
using System.Text;

namespace Klipper.Desktop.Service.WorkTime.Policies.Default
{
    public class  WorkStartEndTimingRule : IWorkTimeRule
    {
        public Tuple<bool, WorkTimeViolation, object> Validate(WorkDay context)
        {


            return null;
        }
    }
}
