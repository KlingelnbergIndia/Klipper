using Sparkle.Appearance;
using Sparkle.Controls.Dialogs;
using System.Windows;
using System.Windows.Controls;

namespace Klipper.Desktop.WPF.Views.WorkTime
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
