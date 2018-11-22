using System;
using System.Windows;
using Klipper.Desktop.WPF.Views.Main;

namespace Klipper.Desktop.WPF.Connectors.Main
{
    public class WorkTimeConnector : AbstractConnector
    {
        public WorkTimeConnector(string tag, MainConnector parent, WorkTimeControl control)
            : base(parent, control)
        {
            this.Tag = tag;
            Ui.Loaded += UILoaded;
        }

        private void UILoaded(object sender, RoutedEventArgs e)
        {
            Initialize();
        }

        public override void Initialize() 
        {
            if (Initialized)
            {
                return;
            }



            Initialized = true;
        }

    }
}
