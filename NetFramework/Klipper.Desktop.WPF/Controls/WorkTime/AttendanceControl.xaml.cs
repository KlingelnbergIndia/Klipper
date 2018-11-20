using Models.Core.HR.Attendance;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace Klipper.Desktop.WPF.Controls.WorkTime
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
                var events = GetAccessEvents(startDate, endDate, employeeId);
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

        private IEnumerable<AccessEvent> GetAccessEvents(DateTime startDate, DateTime endDate, int employeeId)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:7000/");
            client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer", Auth.SessionToken);

            var startStr = startDate.Year.ToString() + "-" + startDate.Month.ToString() + "-" + startDate.Day.ToString();
            var endStr = endDate.Year.ToString() + "-" + endDate.Month.ToString() + "-" + endDate.Day.ToString();
            var str = "api/attendance/" + employeeId.ToString() + "/" + startStr + "/" + endStr;

            HttpResponseMessage response = client.GetAsync(str).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                var accessEvents = JsonConvert.DeserializeObject<IEnumerable<AccessEvent>>(jsonString);
                return accessEvents;
            }
            else
            {
                MessageBox.Show(response.Content.ReadAsStringAsync().Result);
                return new List<AccessEvent>();
            }
        }
    }
}
