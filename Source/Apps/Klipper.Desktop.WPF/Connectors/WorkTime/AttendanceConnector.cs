using Klipper.Desktop.WPF.Connectors.Main;
using Klipper.Desktop.WPF.Views.WorkTime;

namespace Klipper.Desktop.WPF.Connectors.WorkTime
{
    public class AttendanceConnector : AbstractConnector
    {
        public AttendanceConnector(string tag, WorkTimeConnector parent, AttendanceControl control)
            : base(parent, control)
        {
            this.Tag = tag;
        }
    }
}

