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

namespace Klipper.Desktop.WPF.Views.WorkTime
{
    /// <summary>
    /// Interaction logic for ApplyLeaveControl.xaml
    /// </summary>
    public partial class ApplyLeaveControl : UserControl
    {
        public event EventHandler Closed = null;

        public ApplyLeaveControl()
        {
            InitializeComponent();
        }

        private void CreateTestButton_Click(object sender, RoutedEventArgs e)
        {
            Closed?.Invoke(this, null);
        }
    }
}
