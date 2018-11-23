using System.Collections.Generic;
using System.Windows.Controls;

namespace Klipper.Desktop.WPF.Connectors
{
    public interface IConnector
    {
        IConnector Parent { get; }

        ContentControl Ui { get; }

        string Tag { get; set; }

        List<string> Children { get; }

        bool AddChild(string tag, IConnector connector);

        IConnector Child(string tag);

        void Initialize();
    }
}


