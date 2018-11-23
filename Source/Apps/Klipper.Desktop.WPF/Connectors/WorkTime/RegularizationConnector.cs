using Klipper.Desktop.WPF.Connectors.Main;
using Klipper.Desktop.WPF.Views.WorkTime;

namespace Klipper.Desktop.WPF.Connectors.WorkTime
{
    public class RegularizationConnector : AbstractConnector
    {
        public RegularizationConnector(string tag, WorkTimeConnector parent, RegularizationControl control)
            : base(parent, control)
        {
            this.Tag = tag;
        }
    }
}

