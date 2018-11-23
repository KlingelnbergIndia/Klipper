using Klipper.Desktop.WPF.Views.Main;

namespace Klipper.Desktop.WPF.Connectors.Main
{
    public class ManagementConnector : AbstractConnector
    {
        public ManagementConnector(string tag, MainConnector parent, ManagementControl control)
            : base(parent, control)
        {
            this.Tag = tag;
        }
    }
}
