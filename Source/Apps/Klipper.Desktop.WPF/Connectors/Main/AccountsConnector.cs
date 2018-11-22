using Klipper.Desktop.WPF.Views.Main;

namespace Klipper.Desktop.WPF.Connectors.Main
{
    public class AccountsConnector : AbstractConnector
    {
        public AccountsConnector(string tag, MainConnector parent, AccountsControl control)
            : base(parent, control)
        {
            this.Tag = tag;
        }
    }
}
