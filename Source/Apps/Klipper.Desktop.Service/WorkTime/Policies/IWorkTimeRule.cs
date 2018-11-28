using System;
using System.Collections.Generic;

namespace Klipper.Desktop.Service.WorkTime.Policies
{
    public interface IWorkTimeRule
    {
        bool Validate(WorkDay context);
    }
}