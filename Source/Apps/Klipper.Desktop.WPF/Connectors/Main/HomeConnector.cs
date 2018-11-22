using Klipper.Desktop.WPF.Views.Main;

namespace Klipper.Desktop.WPF.Connectors.Main
{
    public class HomeConnector : AbstractConnector
    {
        public HomeConnector(string tag, MainConnector parent, HomeControl control)
            :base(parent, control)
        {
            this.Tag = tag;
        }
    }
}
