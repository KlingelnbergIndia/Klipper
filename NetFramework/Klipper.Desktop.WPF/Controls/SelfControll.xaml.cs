using Klipper.Desktop.WPF.Controls.CustomControls;
using Sparkle.Appearance;
using Sparkle.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Klipper.Desktop.WPF.Controls
{
    /// <summary>
    /// Interaction logic for SelfControll.xaml
    /// </summary>
    public partial class SelfControll : UserControl
    {
        public SelfControll()
        {
            InitializeComponent();

            LeaveStatusDetailList.Content = new LeaveList(); //load dynamic panel
          
        }
        private void applyNewLeave(object sender, RoutedEventArgs e)
        {
            AbsoluteModalDialog dialog = new AbsoluteModalDialog();

            var control = new ApplyLeaveControl();
            control.Closed += (s, args) =>
            {
                dialog?.Close();
            };

            dialog.Child = control;
            AppearanceManager.SetAppearance(dialog);
            dialog.ShowDialog();
        }
    }
}
