using Models.Core.HR.Attendance;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Klipper.Desktop.Service.WorkTime.Attendance;

namespace Klipper.Desktop.WPF.Views.WorkTime
{
    /// <summary>
    /// Interaction logic for AttendanceControl.xaml
    /// </summary>
    public partial class AttendanceControl : UserControl
    {
        public AttendanceControl()
        {
            InitializeComponent();
        }

        private void GetAttendanceButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var startDate = StartDatePicker.SelectedDate.Value;
                var endDate = EndDatePicker.SelectedDate.Value;
                int employeeId = int.Parse(EmployeeIdTextbox.Text);
                var events = AttendanceService.Instance.GetAccessEvents(employeeId, startDate, endDate);
                var accessEvents = (List<AccessEvent>)events;
                if (accessEvents.Count > 0)
                {
                    var json = JsonConvert.SerializeObject(accessEvents, Formatting.Indented);
                    var filename = "C:/Temp/accessEvents.json";
                    File.WriteAllText(filename, json);
                    Process.Start("notepad.exe", filename);
                }
                else
                {
                    MessageBox.Show("No records found!");
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

    }
}
