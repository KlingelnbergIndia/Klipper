using Klipper.Desktop.WPF.Views.Main;

namespace Klipper.Desktop.WPF.Connectors.Main
{
    public class DocumentsConnector : AbstractConnector
    {
        public DocumentsConnector(string tag, MainConnector parent, DocumentsControl control)
            : base(parent, control)
        {
            this.Tag = tag;
        }
    }
}
