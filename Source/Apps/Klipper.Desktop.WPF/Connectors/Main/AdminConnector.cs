using Klipper.Desktop.WPF.Views.Main;

namespace Klipper.Desktop.WPF.Connectors.Main
{
    public class AdminConnector : AbstractConnector
    {
        public AdminConnector(string tag, MainConnector parent, AdminControl control)
            : base(parent, control)
        {
            this.Tag = tag;
        }
    }
}
