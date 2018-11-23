using System;
using Klipper.Desktop.WPF.Connectors.Main;
using Klipper.Desktop.WPF.Views.WorkTime;

namespace Klipper.Desktop.WPF.Connectors.WorkTime
{
    public class LeavesConnector : AbstractConnector
    {
        public LeavesConnector(string tag, WorkTimeConnector parent, LeavesControl control)
            : base(parent, control)
        {
            this.Tag = tag;
        }
    }
}

