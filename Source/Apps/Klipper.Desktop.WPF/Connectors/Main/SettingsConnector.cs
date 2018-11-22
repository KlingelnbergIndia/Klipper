using Klipper.Desktop.WPF.Views.Main;

namespace Klipper.Desktop.WPF.Connectors.Main
{
    public class SettingsConnector : AbstractConnector
    {
        public SettingsConnector(string tag, MainConnector parent, SettingsControl control)
            : base(parent, control)
        {
            this.Tag = tag;
        }
    }
}
