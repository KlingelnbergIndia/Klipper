using Klipper.Desktop.WPF.Views.Main;

namespace Klipper.Desktop.WPF.Connectors.Main
{
    public class HelpConnector : AbstractConnector
    {
        public HelpConnector(string tag, MainConnector parent, HelpControl control)
            : base(parent, control)
        {
            this.Tag = tag;
        }
    }
}
