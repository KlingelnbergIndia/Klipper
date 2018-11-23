using Sparkle.Controls.Navigators;
using System.Windows.Controls;

namespace Klipper.Desktop.WPF.Views.Main
{
    /// <summary>
    /// Interaction logic for WorkTimeControl.xaml
    /// </summary>
    public partial class WorkTimeControl : UserControl
    {
        public WorkTimeControl()
        {
            InitializeComponent();
        }

        #region Properties

        public HamburgerNavigator Navigator { get { return this.TheNavigator; } }

        #endregion
    }
}
