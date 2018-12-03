using Models.Core;
using Models.Core.Employment;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Klipper.Desktop.WPF.CustomControls
{
    /// <summary>
    /// Interaction logic for EmployeeListPanelControl.xaml
    /// </summary>
    public partial class EmployeeListPanelControl : UserControl
    {
        public EmployeeListPanelControl()
        {
            InitializeComponent();
        }

        private async Task<IQueryable> GetAllEmployeesAsync()
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:6001/")
            };
            HttpResponseMessage response = await client.GetAsync("/api/Employees");
            string jsonString = await response.Content.ReadAsStringAsync();
            IQueryable jsonData = JsonConvert.DeserializeObject<Employee[]>(jsonString)
                .Select(x => new {
                    x.ID,
                    x.FirstName,
                    x.LastName,
                    x.Title,
                    BirthDate = x.BirthDate.ToShortDateString(),
                    x.Email,
                    FullName = $"{x.Prefix} {x.FirstName} {x.LastName}",
                    Gender = ((Gender)x.Gender).ToString(),
                }).AsQueryable();
            return jsonData;
        }

        private async void EmployeeList_LoadedAsync(object sender, System.Windows.RoutedEventArgs e)
        {
            DataLoaderAnimation.LoadingText = "Loading data...";
            DataLoaderAnimation.SwitchToLoader();
            employeeList.ItemsSource = await GetAllEmployeesAsync();
            await Task.Delay(3000);
            LoaderPanel.Visibility = Visibility.Hidden;
            employeeList.Visibility = Visibility.Visible;
        }
    }
}
